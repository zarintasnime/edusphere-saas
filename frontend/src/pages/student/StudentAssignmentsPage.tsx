import { useMemo, useState, type FormEvent } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';

import { api, errorMessage } from '../../lib/api';
import { deadlineInfo, formatDateTime } from '../../lib/format';
import { useMyEnrollments, useStudentProfile } from '../../lib/hooks';
import { useDataTable } from '../../lib/useDataTable';
import { PaginationBar, SearchInput } from '../../components/DataTableControls';
import { DeadlineChip } from '../../components/StatusChips';
import { Avatar } from '../../components/Avatar';
import {
  Badge,
  Button,
  Card,
  Code,
  EmptyState,
  ErrorNote,
  Field,
  Input,
  Modal,
  PageHeader,
  Select,
  Spinner,
  Textarea,
} from '../../components/ui';
import type { Assignment, Submission } from '../../lib/types';

export default function StudentAssignmentsPage() {
  const queryClient = useQueryClient();

  const profile = useStudentProfile();
  const enrollments = useMyEnrollments(Boolean(profile.data));

  const [yearId, setYearId] = useState<number | null>(null);
  const [submitting, setSubmitting] = useState<Assignment | null>(null);
  const [formError, setFormError] = useState<string | null>(null);
  const [statusFilter, setStatusFilter] = useState<string>('all');

  const activeYearId =
    yearId ??
    enrollments.data?.find((item) => item.isActive)?.academicYearId ??
    enrollments.data?.[0]?.academicYearId ??
    null;

  const assignments = useQuery({
    queryKey: ['assignments', 'student', activeYearId],
    enabled: Boolean(activeYearId),
    queryFn: async () => {
      const { data } = await api.get<Assignment[]>(
        `/api/Assignment/student/${activeYearId}`,
      );
      return data;
    },
  });

  const mySubmissions = useQuery({
    queryKey: ['submissions', 'student', profile.data?.studentId],
    enabled: Boolean(profile.data),
    queryFn: async () => {
      const { data } = await api.get<Submission[]>(
        `/api/Submission/student/${profile.data!.studentId}`,
      );
      return data;
    },
  });

  const submittedIds = useMemo(
    () => new Set((mySubmissions.data ?? []).map((item) => item.assignmentId)),
    [mySubmissions.data],
  );

  const rawList = useMemo(
    () =>
      [...(assignments.data ?? [])]
        .filter((item) => item.assignmentStatus === 'Published')
        .sort((a, b) => a.dueDate.localeCompare(b.dueDate)),
    [assignments.data],
  );

  const filteredByStatus = useMemo(() => {
    return rawList.filter((item) => {
      const isDone = submittedIds.has(item.assignmentId);
      if (statusFilter === 'pending') return !isDone;
      if (statusFilter === 'submitted') return isDone;
      return true;
    });
  }, [rawList, submittedIds, statusFilter]);

  const dataTable = useDataTable<Assignment>({
    data: filteredByStatus,
    searchFields: [
      (a) => a.title,
      (a) => a.subjectName,
      (a) => a.teacherName,
    ],
    pageSize: 10,
  });

  const submit = useMutation({
    mutationFn: async (input: {
      assignment: Assignment;
      text: string;
      file: File | null;
    }) => {
      const { data } = await api.post<{ submissionId: number }>('/api/Submission', {
        institutionId: profile.data!.institutionId,
        studentId: profile.data!.studentId,
        assignmentId: input.assignment.assignmentId,
        submissionText: input.text || null,
      });

      if (input.file) {
        const body = new FormData();
        body.append('InstitutionId', String(profile.data!.institutionId));
        body.append('SubmissionId', String(data.submissionId));
        body.append('File', input.file);

        await api.post('/api/SubmissionAttachment', body, {
          headers: { 'Content-Type': undefined },
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['submissions'] });
      setSubmitting(null);
    },
    onError: (error) => setFormError(errorMessage(error)),
  });

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError(null);

    if (!submitting || !profile.data) return;

    const form = new FormData(event.currentTarget);
    const text = String(form.get('submissionText') ?? '').trim();
    const file = form.get('file');
    const upload = file instanceof File && file.size > 0 ? file : null;

    if (!text && !upload) {
      setFormError('Add a note, attach a file, or both.');
      return;
    }

    submit.mutate({ assignment: submitting, text, file: upload });
  }

  if (profile.isLoading) return <Spinner label="Loading your profile" />;
  if (profile.isError) return <ErrorNote message={errorMessage(profile.error)} />;

  return (
    <>
      <PageHeader
        title="Assignments"
        subtitle="Everything published for your academic year."
        action={
          (enrollments.data?.length ?? 0) > 1 ? (
            <Select
              value={activeYearId ?? ''}
              onChange={(event) => setYearId(Number(event.target.value))}
              className="w-auto"
            >
              {enrollments.data?.map((item) => (
                <option key={item.enrollmentId} value={item.academicYearId}>
                  {item.academicYearName}
                </option>
              ))}
            </Select>
          ) : undefined
        }
      />

      {enrollments.isSuccess && enrollments.data.length === 0 && (
        <ErrorNote message="You are not enrolled in an academic year yet, so there is nothing to show. An administrator can add the enrolment." />
      )}

      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <SearchInput
          value={dataTable.searchQuery}
          onChange={dataTable.setSearchQuery}
          placeholder="Search assignments or subjects..."
        />

        <div className="flex items-center gap-2">
          <span className="font-mono text-[11px] uppercase tracking-[0.14em] text-body-muted">Filter:</span>
          <Select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="w-auto text-xs py-1.5"
          >
            <option value="all">All assignments</option>
            <option value="pending">To hand in</option>
            <option value="submitted">Handed in</option>
          </Select>
        </div>
      </div>

      <Card>
        {(assignments.isLoading || enrollments.isLoading) && <Spinner />}

        {assignments.isSuccess && rawList.length === 0 && (
          <EmptyState
            title="Nothing set right now"
            description="When a teacher publishes an assignment for your year, it appears here."
          />
        )}

        {assignments.isSuccess && rawList.length > 0 && dataTable.paginatedItems.length === 0 && (
          <EmptyState
            title="No matching assignments"
            description="Try clearing your search query or status filter."
          />
        )}

        <ul className="divide-y divide-rule">
          {dataTable.paginatedItems.map((assignment) => {
            const state = deadlineInfo(assignment).state;
            const done = submittedIds.has(assignment.assignmentId);

            return (
              <li key={assignment.assignmentId} className="px-4 py-4">
                <div className={`rail rail-${state} flex flex-wrap items-start justify-between gap-3`}>
                  <div className="min-w-0">
                    <p className="font-medium text-ink">{assignment.title}</p>
                    <p className="mt-1 flex flex-wrap items-center gap-1.5 text-xs text-body-muted">
                      <span>{assignment.subjectName}</span>
                      <span>·</span>
                      <span className="inline-flex items-center gap-1">
                        <Avatar name={assignment.teacherName} id={assignment.teacherId} size="xs" />
                        <span>{assignment.teacherName}</span>
                      </span>
                      <span>·</span>
                      <Code>{assignment.totalMarks} marks</Code>
                    </p>
                    <p className="mt-0.5 text-xs text-body-faint">
                      Due <Code>{formatDateTime(assignment.dueDate)}</Code>
                      {assignment.allowLateSubmission &&
                        ` · late until ${formatDateTime(assignment.lateSubmissionDeadline)}`}
                    </p>

                    {assignment.description && (
                      <p className="mt-2 line-clamp-2 text-sm text-body-muted">
                        {assignment.description}
                      </p>
                    )}
                  </div>

                  <div className="flex flex-wrap items-center gap-2">
                    <DeadlineChip assignment={assignment} />

                    {done ? (
                      <Badge tone="open">Handed in</Badge>
                    ) : (
                      <Button
                        variant="secondary"
                        onClick={() => {
                          setFormError(null);
                          setSubmitting(assignment);
                        }}
                      >
                        Hand in
                      </Button>
                    )}
                  </div>
                </div>
              </li>
            );
          })}
        </ul>

        <PaginationBar
          currentPage={dataTable.currentPage}
          totalPages={dataTable.totalPages}
          totalItems={dataTable.totalItems}
          startIndex={dataTable.startIndex}
          endIndex={dataTable.endIndex}
          onPageChange={dataTable.setCurrentPage}
          pageSize={dataTable.pageSize}
          onPageSizeChange={dataTable.setPageSize}
        />
      </Card>

      <Modal
        open={Boolean(submitting)}
        onClose={() => setSubmitting(null)}
        title={submitting ? `Hand in: ${submitting.title}` : 'Hand in'}
      >
        <form onSubmit={handleSubmit} className="space-y-4">
          {formError && <ErrorNote message={formError} />}

          {submitting && deadlineInfo(submitting).state === 'over' && (
            <div className="rounded-lg border border-amberish/20 bg-amberish-ghost px-3 py-2 text-sm text-amberish">
              The deadline has passed. This will be recorded as a late submission.
            </div>
          )}

          <Field label="Notes for your teacher">
            <Textarea
              name="submissionText"
              placeholder="Summarise what you did, and flag anything unfinished."
            />
          </Field>

          <Field label="Attachment" hint="One file, up to 20 MB.">
            <Input type="file" name="file" />
          </Field>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="secondary" onClick={() => setSubmitting(null)}>
              Cancel
            </Button>
            <Button type="submit" loading={submit.isPending}>
              Hand in
            </Button>
          </div>
        </form>
      </Modal>
    </>
  );
}

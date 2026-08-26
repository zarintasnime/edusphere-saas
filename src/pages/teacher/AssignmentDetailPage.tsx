import { useMemo, useState, type FormEvent } from 'react';
import { Link, useParams } from 'react-router-dom';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { ArrowLeft, Paperclip } from 'lucide-react';

import { api, errorMessage, fileUrl } from '../../lib/api';
import { formatBytes, formatDateTime } from '../../lib/format';
import { useTeacherProfile } from '../../lib/hooks';
import { useDataTable } from '../../lib/useDataTable';
import { PaginationBar, SearchInput } from '../../components/DataTableControls';
import { SubmissionChip } from '../../components/StatusChips';
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
import type { Assessment, Assignment, Submission } from '../../lib/types';

export default function AssignmentDetailPage() {
  const { assignmentId } = useParams();
  const id = Number(assignmentId);

  const queryClient = useQueryClient();
  const profile = useTeacherProfile();

  const [grading, setGrading] = useState<Submission | null>(null);
  const [formError, setFormError] = useState<string | null>(null);
  const [gradeFilter, setGradeFilter] = useState<string>('all');

  const assignment = useQuery({
    queryKey: ['assignment', id],
    queryFn: async () => {
      const { data } = await api.get<Assignment>(`/api/Assignment/${id}`);
      return data;
    },
  });

  const submissions = useQuery({
    queryKey: ['submissions', 'assignment', id],
    queryFn: async () => {
      const { data } = await api.get<Submission[]>(
        `/api/Submission/assignment/${id}`,
      );
      return data;
    },
  });

  const teacherAssessments = useQuery({
    queryKey: ['assessments', 'teacher', profile.data?.teacherId],
    enabled: Boolean(profile.data?.teacherId),
    queryFn: async () => {
      const { data } = await api.get<Assessment[]>(
        `/api/Assessment/teacher/${profile.data!.teacherId}`,
      );
      return data;
    },
  });

  const assessmentsMap = useMemo(() => {
    const map = new Map<number, Assessment>();
    if (teacherAssessments.data) {
      for (const item of teacherAssessments.data) {
        map.set(item.submissionId, item);
      }
    }
    return map;
  }, [teacherAssessments.data]);

  const rawSubmissions = submissions.data ?? [];
  const filteredByGraded = rawSubmissions.filter((s) => {
    const isGraded = assessmentsMap.has(s.submissionId);
    if (gradeFilter === 'graded') return isGraded;
    if (gradeFilter === 'ungraded') return !isGraded;
    return true;
  });

  const dataTable = useDataTable<Submission>({
    data: filteredByGraded,
    searchFields: [(s) => s.studentName, (s) => s.studentCode],
    initialSortField: (s) => s.submittedAt,
    initialSortDirection: 'desc',
    pageSize: 10,
  });

  const grade = useMutation({
    mutationFn: async (input: {
      submission: Submission;
      marksObtained: number;
      feedback: string;
    }) => {
      await api.post('/api/Assessment', {
        institutionId: input.submission.institutionId,
        submissionId: input.submission.submissionId,
        teacherId: profile.data!.teacherId,
        policyId: null,
        marksObtained: input.marksObtained,
        feedback: input.feedback || null,
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['submissions'] });
      queryClient.invalidateQueries({ queryKey: ['assessments'] });
      setGrading(null);
    },
    onError: (error) => setFormError(errorMessage(error)),
  });

  function handleGrade(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError(null);

    if (!grading || !profile.data) return;

    const form = new FormData(event.currentTarget);
    const marksObtained = Number(form.get('marksObtained'));
    const total = assignment.data?.totalMarks ?? 0;

    if (marksObtained > total) {
      setFormError(`Marks cannot be above the total of ${total}.`);
      return;
    }

    grade.mutate({
      submission: grading,
      marksObtained,
      feedback: String(form.get('feedback') ?? '').trim(),
    });
  }

  if (assignment.isLoading) return <Spinner label="Loading assignment" />;
  if (assignment.isError) return <ErrorNote message={errorMessage(assignment.error)} />;

  const detail = assignment.data!;

  return (
    <>
      <Link
        to="/teacher/assignments"
        className="mb-4 inline-flex items-center gap-1.5 text-sm text-body-muted hover:text-ink"
      >
        <ArrowLeft className="h-4 w-4" />
        Assignments
      </Link>

      <PageHeader
        title={detail.title}
        subtitle={`${detail.subjectName} · ${detail.academicYearName} · ${detail.totalMarks} marks`}
      />

      <Card className="p-4">
        <p className="whitespace-pre-line text-sm leading-relaxed text-body-muted">
          {detail.description || 'No instructions were added.'}
        </p>

        <dl className="mt-4 grid gap-3 border-t border-rule pt-4 text-xs sm:grid-cols-3">
          <div>
            <dt className="uppercase tracking-wide text-body-faint">Due</dt>
            <dd className="mt-0.5 font-mono text-ink">
              {formatDateTime(detail.dueDate)}
            </dd>
          </div>
          <div>
            <dt className="uppercase tracking-wide text-body-faint">Late window</dt>
            <dd className="mt-0.5 font-mono text-ink">
              {detail.allowLateSubmission
                ? formatDateTime(detail.lateSubmissionDeadline)
                : 'Not accepted'}
            </dd>
          </div>
          <div>
            <dt className="uppercase tracking-wide text-body-faint">Status</dt>
            <dd className="mt-0.5">
              <Badge tone={detail.assignmentStatus === 'Published' ? 'ink' : 'neutral'}>
                {detail.assignmentStatus}
              </Badge>
            </dd>
          </div>
        </dl>
      </Card>

      <div className="mb-3 mt-8 flex flex-wrap items-center justify-between gap-3">
        <h2 className="font-display text-sm font-semibold uppercase tracking-wide text-body-muted">
          Submissions ({rawSubmissions.length})
        </h2>

        <div className="flex flex-wrap items-center gap-3">
          <SearchInput
            value={dataTable.searchQuery}
            onChange={dataTable.setSearchQuery}
            placeholder="Search student or code..."
          />
          <Select
            value={gradeFilter}
            onChange={(e) => setGradeFilter(e.target.value)}
            className="w-auto text-xs py-1.5"
          >
            <option value="all">All submissions</option>
            <option value="graded">Graded</option>
            <option value="ungraded">Needs grading</option>
          </Select>
        </div>
      </div>

      <Card>
        {submissions.isLoading && <Spinner />}

        {!submissions.isLoading && rawSubmissions.length === 0 && (
          <EmptyState
            title="No submissions yet"
            description="Once students hand work in, it lands here ready to grade."
          />
        )}

        {!submissions.isLoading && rawSubmissions.length > 0 && dataTable.paginatedItems.length === 0 && (
          <EmptyState
            title="No matching submissions"
            description="Try adjusting your search query or grade filter."
          />
        )}

        <ul className="divide-y divide-rule">
          {dataTable.paginatedItems.map((submission) => (
            <SubmissionRow
              key={submission.submissionId}
              submission={submission}
              assessment={assessmentsMap.get(submission.submissionId) ?? null}
              totalMarks={detail.totalMarks}
              onGrade={() => {
                setFormError(null);
                setGrading(submission);
              }}
            />
          ))}
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
        open={Boolean(grading)}
        onClose={() => setGrading(null)}
        title={grading ? `Grade ${grading.studentName}` : 'Grade'}
      >
        <form onSubmit={handleGrade} className="space-y-4">
          {formError && <ErrorNote message={formError} />}

          {grading?.isLateSubmission && (
            <div className="rounded-lg border border-amberish/20 bg-amberish-ghost px-3 py-2 text-sm text-amberish">
              Handed in after the deadline. Apply any penalty to the marks yourself.
            </div>
          )}

          <Field label={`Marks out of ${detail.totalMarks}`}>
            <Input
              name="marksObtained"
              type="number"
              min={0}
              max={detail.totalMarks}
              step="0.5"
              required
              autoFocus
            />
          </Field>

          <Field label="Feedback">
            <Textarea
              name="feedback"
              placeholder="What was done well, and what would earn more marks next time?"
            />
          </Field>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="secondary" onClick={() => setGrading(null)}>
              Cancel
            </Button>
            <Button type="submit" loading={grade.isPending}>
              Save grade
            </Button>
          </div>
        </form>
      </Modal>
    </>
  );
}

function SubmissionRow({
  submission,
  assessment,
  totalMarks,
  onGrade,
}: {
  submission: Submission;
  assessment: Assessment | null;
  totalMarks: number;
  onGrade: () => void;
}) {
  return (
    <li className="px-4 py-4">
      <div className="flex flex-wrap items-start justify-between gap-3">
        <div className="flex items-center gap-3 min-w-0">
          <Avatar name={submission.studentName} id={submission.studentId} size="md" />
          <div className="min-w-0">
            <p className="font-medium text-ink">{submission.studentName}</p>
            <p className="mt-0.5 text-xs text-body-muted">
              <Code>{submission.studentCode}</Code> · handed in{' '}
              <Code>{formatDateTime(submission.submittedAt)}</Code>
              {submission.submissionVersion > 1 &&
                ` · version ${submission.submissionVersion}`}
            </p>
          </div>
        </div>

        <div className="flex flex-wrap items-center gap-2">
          {submission.isLateSubmission && <Badge tone="over">Late</Badge>}
          <SubmissionChip status={submission.submissionStatus} />

          {assessment ? (
            <Badge tone="open">
              {assessment.finalMarks} / {totalMarks}
            </Badge>
          ) : (
            <Button variant="secondary" onClick={onGrade}>
              Grade
            </Button>
          )}
        </div>
      </div>

      {submission.submissionText && (
        <p className="mt-2 whitespace-pre-line rounded-lg bg-paper-warm px-3 py-2 text-sm text-body-muted">
          {submission.submissionText}
        </p>
      )}

      {submission.attachments?.length > 0 && (
        <ul className="mt-2 flex flex-wrap gap-2">
          {submission.attachments.map((file) => (
            <li key={file.attachmentId}>
              <a
                href={fileUrl(file.filePath)}
                target="_blank"
                rel="noreferrer"
                className="inline-flex items-center gap-1.5 rounded-lg border border-rule px-2.5 py-1 text-xs text-body-muted hover:border-ink hover:text-ink"
              >
                <Paperclip className="h-3.5 w-3.5" />
                {file.fileName}
                <span className="text-body-faint">{formatBytes(file.fileSize)}</span>
              </a>
            </li>
          ))}
        </ul>
      )}

      {assessment?.feedback && (
        <p className="mt-2 border-l-2 border-moss pl-3 text-sm text-body-muted">
          {assessment.feedback}
        </p>
      )}
    </li>
  );
}

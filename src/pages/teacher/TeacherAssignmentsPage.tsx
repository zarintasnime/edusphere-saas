import { useState, type FormEvent } from 'react';
import { Link } from 'react-router-dom';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Plus } from 'lucide-react';

import { api, errorMessage } from '../../lib/api';
import { deadlineInfo, formatDateTime, toLocalInputValue } from '../../lib/format';
import {
  useAcademicYears,
  useMyAssignments,
  useTeacherProfile,
  useTeacherSubjects,
} from '../../lib/hooks';
import { useDataTable } from '../../lib/useDataTable';
import { PaginationBar, SearchInput } from '../../components/DataTableControls';
import { AssignmentStatusChip, DeadlineChip } from '../../components/StatusChips';
import {
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
import type { Assignment } from '../../lib/types';

export default function TeacherAssignmentsPage() {
  const queryClient = useQueryClient();

  const profile = useTeacherProfile();
  const assignments = useMyAssignments(Boolean(profile.data));
  const subjects = useTeacherSubjects(profile.data?.teacherId);
  const years = useAcademicYears(Boolean(profile.data));

  const [composerOpen, setComposerOpen] = useState(false);
  const [editing, setEditing] = useState<Assignment | null>(null);
  const [formError, setFormError] = useState<string | null>(null);
  const [statusFilter, setStatusFilter] = useState<string>('all');

  const rawList = assignments.data ?? [];
  const filteredByStatus = statusFilter === 'all'
    ? rawList
    : rawList.filter((a) => a.assignmentStatus.toLowerCase() === statusFilter.toLowerCase());

  const dataTable = useDataTable<Assignment>({
    data: filteredByStatus,
    searchFields: [
      (a) => a.title,
      (a) => a.subjectName,
      (a) => a.academicYearName,
    ],
    initialSortField: (a) => a.createdAt,
    initialSortDirection: 'desc',
    pageSize: 10,
  });

  const invalidate = () => {
    queryClient.invalidateQueries({ queryKey: ['assignments'] });
  };

  const save = useMutation({
    mutationFn: async (payload: Record<string, unknown>) => {
      if (editing) {
        await api.put(`/api/Assignment/${editing.assignmentId}`, payload);
        return;
      }
      await api.post('/api/Assignment', payload);
    },
    onSuccess: () => {
      invalidate();
      closeComposer();
    },
    onError: (error) => setFormError(errorMessage(error)),
  });

  const publish = useMutation({
    mutationFn: async (assignment: Assignment) => {
      const next =
        assignment.assignmentStatus === 'Published' ? 'Closed' : 'Published';

      await api.patch(`/api/Assignment/${assignment.assignmentId}/status`, {
        status: next,
      });
    },
    onSuccess: invalidate,
  });

  function openComposer(assignment?: Assignment) {
    setEditing(assignment ?? null);
    setFormError(null);
    setComposerOpen(true);
  }

  function closeComposer() {
    setComposerOpen(false);
    setEditing(null);
  }

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError(null);

    const form = new FormData(event.currentTarget);
    const teacherSubjectId = Number(form.get('teacherSubjectId'));

    const link = subjects.data?.find(
      (item) => item.teacherSubjectId === teacherSubjectId,
    );

    if (!link || !profile.data) {
      setFormError('Pick a subject you are assigned to.');
      return;
    }

    const allowLate = form.get('allowLateSubmission') === 'on';
    const lateDeadline = String(form.get('lateSubmissionDeadline') ?? '');
    const dueDate = String(form.get('dueDate') ?? '');

    if (allowLate && !lateDeadline) {
      setFormError('Set a date for the late window, or turn late submissions off.');
      return;
    }

    if (allowLate && lateDeadline < dueDate) {
      setFormError('The late window must close on or after the due date.');
      return;
    }

    const payload = {
      institutionId: profile.data.institutionId,
      teacherId: profile.data.teacherId,
      teacherSubjectId,
      courseSubjectId: link.courseSubjectId,
      academicYearId: Number(form.get('academicYearId')),
      title: String(form.get('title') ?? '').trim(),
      description: String(form.get('description') ?? '').trim() || null,
      totalMarks: Number(form.get('totalMarks')),
      dueDate,
      allowLateSubmission: allowLate,
      lateSubmissionDeadline: allowLate && lateDeadline ? lateDeadline : null,
      assignmentStatus: String(form.get('assignmentStatus')),
      isActive: true,
    };

    save.mutate(payload);
  }

  if (profile.isLoading) return <Spinner label="Loading your profile" />;
  if (profile.isError) return <ErrorNote message={errorMessage(profile.error)} />;

  const canCompose = (subjects.data?.length ?? 0) > 0 && (years.data?.length ?? 0) > 0;

  return (
    <>
      <PageHeader
        title="Assignments"
        subtitle="Draft, publish and grade the work you set."
        action={
          <Button onClick={() => openComposer()} disabled={!canCompose}>
            <Plus className="h-4 w-4" />
            New assignment
          </Button>
        }
      />

      {!canCompose && !subjects.isLoading && (
        <div className="mb-4">
          <ErrorNote message="You need at least one assigned subject and one academic year before you can create an assignment. An administrator sets these up." />
        </div>
      )}

      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <SearchInput
          value={dataTable.searchQuery}
          onChange={dataTable.setSearchQuery}
          placeholder="Search assignments by title or subject..."
        />

        <div className="flex items-center gap-2">
          <span className="font-mono text-[11px] uppercase tracking-[0.14em] text-body-muted">Status:</span>
          <Select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="w-auto text-xs py-1.5"
          >
            <option value="all">All statuses</option>
            <option value="draft">Draft</option>
            <option value="published">Published</option>
            <option value="closed">Closed</option>
          </Select>
        </div>
      </div>

      <Card>
        {assignments.isLoading && <Spinner />}

        {!assignments.isLoading && rawList.length === 0 && (
          <EmptyState
            title="No assignments yet"
            description="Create one, keep it as a draft while you write it, then publish when it is ready."
          />
        )}

        {!assignments.isLoading && rawList.length > 0 && dataTable.paginatedItems.length === 0 && (
          <EmptyState
            title="No matching assignments"
            description="Try clearing your search query or status filter."
          />
        )}

        <ul className="divide-y divide-rule">
          {dataTable.paginatedItems.map((assignment) => (
            <li key={assignment.assignmentId} className="px-4 py-4">
              <div
                className={`rail rail-${deadlineInfo(assignment).state} flex flex-wrap items-start justify-between gap-3`}
              >
                <div className="min-w-0">
                  <Link
                    to={`/teacher/assignments/${assignment.assignmentId}`}
                    className="font-medium text-ink hover:text-ink"
                  >
                    {assignment.title}
                  </Link>

                  <p className="mt-1 text-xs text-body-muted">
                    {assignment.subjectName} · {assignment.academicYearName} ·{' '}
                    <Code>{assignment.totalMarks} marks</Code>
                  </p>

                  <p className="mt-0.5 text-xs text-body-faint">
                    Due <Code>{formatDateTime(assignment.dueDate)}</Code>
                  </p>
                </div>

                <div className="flex flex-wrap items-center gap-2">
                  <AssignmentStatusChip assignment={assignment} />
                  {assignment.assignmentStatus === 'Published' && (
                    <DeadlineChip assignment={assignment} />
                  )}

                  <Button variant="ghost" onClick={() => openComposer(assignment)}>
                    Edit
                  </Button>

                  {assignment.assignmentStatus !== 'Archived' && (
                    <Button
                      variant="secondary"
                      loading={
                        publish.isPending &&
                        publish.variables?.assignmentId === assignment.assignmentId
                      }
                      onClick={() => publish.mutate(assignment)}
                    >
                      {assignment.assignmentStatus === 'Published' ? 'Close' : 'Publish'}
                    </Button>
                  )}
                </div>
              </div>
            </li>
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
        open={composerOpen}
        onClose={closeComposer}
        title={editing ? 'Edit assignment' : 'New assignment'}
      >
        <form onSubmit={handleSubmit} className="space-y-4">
          {formError && <ErrorNote message={formError} />}

          <Field label="Title">
            <Input name="title" required defaultValue={editing?.title} maxLength={200} />
          </Field>

          <Field label="Instructions">
            <Textarea
              name="description"
              defaultValue={editing?.description ?? ''}
              placeholder="What should students hand in, and how will it be marked?"
            />
          </Field>

          <div className="grid gap-4 sm:grid-cols-2">
            <Field label="Subject">
              <Select
                name="teacherSubjectId"
                required
                defaultValue={editing?.teacherSubjectId}
              >
                {subjects.data?.map((link) => (
                  <option key={link.teacherSubjectId} value={link.teacherSubjectId}>
                    {link.subjectName} ({link.courseName})
                  </option>
                ))}
              </Select>
            </Field>

            <Field label="Academic year">
              <Select
                name="academicYearId"
                required
                defaultValue={editing?.academicYearId}
              >
                {years.data?.map((year) => (
                  <option key={year.academicYearId} value={year.academicYearId}>
                    {year.yearName}
                  </option>
                ))}
              </Select>
            </Field>

            <Field label="Total marks">
              <Input
                name="totalMarks"
                type="number"
                min={1}
                step="0.5"
                required
                defaultValue={editing?.totalMarks ?? 20}
              />
            </Field>

            <Field label="Status">
              <Select
                name="assignmentStatus"
                defaultValue={editing?.assignmentStatus ?? 'Draft'}
              >
                <option value="Draft">Draft</option>
                <option value="Published">Published</option>
                <option value="Closed">Closed</option>
              </Select>
            </Field>

            <Field label="Due date">
              <Input
                name="dueDate"
                type="datetime-local"
                required
                defaultValue={toLocalInputValue(editing?.dueDate)}
              />
            </Field>

            <Field label="Late window closes" hint="Only used if late work is allowed.">
              <Input
                name="lateSubmissionDeadline"
                type="datetime-local"
                defaultValue={toLocalInputValue(editing?.lateSubmissionDeadline)}
              />
            </Field>
          </div>

          <label className="flex items-center gap-2 text-sm text-ink">
            <input
              type="checkbox"
              name="allowLateSubmission"
              defaultChecked={editing?.allowLateSubmission ?? false}
              className="h-4 w-4 rounded border-rule text-ink focus:ring-ink"
            />
            Accept late submissions
          </label>

          <div className="flex justify-end gap-2 pt-1">
            <Button type="button" variant="secondary" onClick={closeComposer}>
              Cancel
            </Button>
            <Button type="submit" loading={save.isPending}>
              {editing ? 'Save changes' : 'Create assignment'}
            </Button>
          </div>
        </form>
      </Modal>
    </>
  );
}

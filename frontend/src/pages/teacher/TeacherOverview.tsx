import { Link } from 'react-router-dom';

import { useAuth } from '../../auth/AuthContext';
import { useMyAssignments, useTeacherProfile, useTeacherSubjects } from '../../lib/hooks';
import { deadlineInfo, formatDateTime } from '../../lib/format';
import { DeadlineChip } from '../../components/StatusChips';
import {
  Card,
  Code,
  EmptyState,
  ErrorNote,
  PageHeader,
  Spinner,
  StatCard,
} from '../../components/ui';
import { errorMessage } from '../../lib/api';

export default function TeacherOverview() {
  const { user } = useAuth();
  const profile = useTeacherProfile();
  const assignments = useMyAssignments(Boolean(profile.data));
  const subjects = useTeacherSubjects(profile.data?.teacherId);

  if (profile.isLoading) return <Spinner label="Loading your profile" />;

  if (profile.isError) {
    return <ErrorNote message={errorMessage(profile.error)} />;
  }

  const list = assignments.data ?? [];
  const published = list.filter((item) => item.assignmentStatus === 'Published');
  const drafts = list.filter((item) => item.assignmentStatus === 'Draft');

  const upcoming = published
    .filter((item) => deadlineInfo(item).state !== 'over')
    .sort((a, b) => a.dueDate.localeCompare(b.dueDate))
    .slice(0, 4);

  return (
    <>
      <PageHeader
        title={`Good to see you, ${user?.fullName.split(' ')[0]}`}
        subtitle={
          profile.data
            ? `${profile.data.departmentName} · ${profile.data.employeeCode}`
            : undefined
        }
      />

      <div className="grid gap-4 sm:grid-cols-3">
        <StatCard label="Published" value={published.length} caption="visible to students" />
        <StatCard label="Drafts" value={drafts.length} caption="not released yet" />
        <StatCard
          label="Subjects"
          value={subjects.data?.length ?? 0}
          caption="assigned to you"
        />
      </div>

      <Card className="mt-6">
        <div className="flex items-center justify-between border-b border-rule px-4 py-3">
          <h2 className="font-display text-sm font-semibold">Closing soon</h2>
          <Link
            to="/teacher/assignments"
            className="text-sm text-ink hover:text-body-muted"
          >
            All assignments
          </Link>
        </div>

        {assignments.isLoading && <Spinner />}

        {!assignments.isLoading && upcoming.length === 0 && (
          <EmptyState
            title="Nothing due"
            description="Published assignments with an open deadline show up here."
          />
        )}

        <ul className="divide-y divide-rule">
          {upcoming.map((assignment) => (
            <li key={assignment.assignmentId} className="px-4 py-3">
              <Link
                to={`/teacher/assignments/${assignment.assignmentId}`}
                className={`rail rail-${deadlineInfo(assignment).state} block`}
              >
                <div className="flex flex-wrap items-center justify-between gap-2">
                  <span className="font-medium text-ink">{assignment.title}</span>
                  <DeadlineChip assignment={assignment} />
                </div>
                <p className="mt-0.5 text-xs text-body-muted">
                  {assignment.subjectName} · <Code>{formatDateTime(assignment.dueDate)}</Code>
                </p>
              </Link>
            </li>
          ))}
        </ul>
      </Card>
    </>
  );
}

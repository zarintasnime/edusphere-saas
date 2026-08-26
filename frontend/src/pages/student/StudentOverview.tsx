import { Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';

import { api, errorMessage } from '../../lib/api';
import { useAuth } from '../../auth/AuthContext';
import { useMyEnrollments, useStudentProfile } from '../../lib/hooks';
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
import type { Assignment, Submission } from '../../lib/types';

export default function StudentOverview() {
  const { user } = useAuth();
  const profile = useStudentProfile();
  const enrollments = useMyEnrollments(Boolean(profile.data));

  const activeYearId =
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

  const submissions = useQuery({
    queryKey: ['submissions', 'student', profile.data?.studentId],
    enabled: Boolean(profile.data),
    queryFn: async () => {
      const { data } = await api.get<Submission[]>(
        `/api/Submission/student/${profile.data!.studentId}`,
      );
      return data;
    },
  });

  if (profile.isLoading) return <Spinner label="Loading your profile" />;
  if (profile.isError) return <ErrorNote message={errorMessage(profile.error)} />;

  const published = (assignments.data ?? []).filter(
    (item) => item.assignmentStatus === 'Published',
  );

  const handedInIds = new Set((submissions.data ?? []).map((item) => item.assignmentId));

  const outstanding = published
    .filter((item) => !handedInIds.has(item.assignmentId))
    .sort((a, b) => a.dueDate.localeCompare(b.dueDate));

  const graded = (submissions.data ?? []).filter(
    (item) => item.submissionStatus === 'Reviewed',
  );

  return (
    <>
      <PageHeader
        title={`Hello, ${user?.fullName.split(' ')[0]}`}
        subtitle={
          profile.data
            ? `${profile.data.studentCode} · ${profile.data.institutionName}`
            : undefined
        }
      />

      <div className="grid gap-4 sm:grid-cols-3">
        <StatCard
          label="To hand in"
          value={outstanding.length}
          caption="published, not submitted"
        />
        <StatCard
          label="Handed in"
          value={submissions.data?.length ?? 0}
          caption="across all subjects"
        />
        <StatCard label="Graded" value={graded.length} caption="feedback available" />
      </div>

      <Card className="mt-6">
        <div className="flex items-center justify-between border-b border-rule px-4 py-3">
          <h2 className="font-display text-sm font-semibold">Next deadlines</h2>
          <Link
            to="/student/assignments"
            className="text-sm text-ink hover:text-body-muted"
          >
            All assignments
          </Link>
        </div>

        {(assignments.isLoading || enrollments.isLoading) && <Spinner />}

        {assignments.isSuccess && outstanding.length === 0 && (
          <EmptyState
            title="You are all caught up"
            description="Nothing is waiting to be handed in right now."
          />
        )}

        <ul className="divide-y divide-rule">
          {outstanding.slice(0, 5).map((assignment) => (
            <li key={assignment.assignmentId} className="px-4 py-3">
              <div className={`rail rail-${deadlineInfo(assignment).state}`}>
                <div className="flex flex-wrap items-center justify-between gap-2">
                  <span className="font-medium text-ink">{assignment.title}</span>
                  <DeadlineChip assignment={assignment} />
                </div>
                <p className="mt-0.5 text-xs text-body-muted">
                  {assignment.subjectName} ·{' '}
                  <Code>{formatDateTime(assignment.dueDate)}</Code>
                </p>
              </div>
            </li>
          ))}
        </ul>
      </Card>
    </>
  );
}

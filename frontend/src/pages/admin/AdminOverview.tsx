import { Link } from 'react-router-dom';
import { useAuth } from '../../auth/AuthContext';
import {
  useAcademicYears,
  useAuditLogs,
  useCourseSubjects,
  useCourses,
  useDepartments,
  useStudents,
  useSubjects,
  useTeachers,
} from '../../lib/hooks';
import { Badge, Card, Code, EmptyState, PageHeader, Spinner, StatCard } from '../../components/ui';
import { Avatar } from '../../components/Avatar';
import { formatDateTime } from '../../lib/format';

export default function AdminOverview() {
  const { user } = useAuth();

  const departments = useDepartments();
  const courses = useCourses();
  const subjects = useSubjects();
  const teachers = useTeachers();
  const students = useStudents();
  const courseSubjects = useCourseSubjects();
  const years = useAcademicYears();
  const auditLogs = useAuditLogs();

  const setupSteps = [
    {
      label: 'Departments and courses',
      done: (departments.data?.length ?? 0) > 0 && (courses.data?.length ?? 0) > 0,
      to: '/admin/academic',
    },
    {
      label: 'Subjects mapped to a course',
      done: (courseSubjects.data?.length ?? 0) > 0,
      to: '/admin/academic',
    },
    {
      label: 'Batches with an academic year',
      done: (years.data?.length ?? 0) > 0,
      to: '/admin/academic',
    },
    {
      label: 'Teachers assigned to subjects',
      done: (teachers.data?.length ?? 0) > 0,
      to: '/admin/teaching',
    },
    {
      label: 'Students enrolled in a year',
      done: (students.data?.length ?? 0) > 0,
      to: '/admin/teaching',
    },
  ];

  return (
    <>
      <PageHeader
        title={`Welcome back, ${user?.fullName.split(' ')[0]}`}
        subtitle="The academic structure and live system overview."
      />

      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
        <StatCard label="Teachers" value={teachers.data?.length ?? 0} />
        <StatCard label="Students" value={students.data?.length ?? 0} />
        <StatCard label="Courses" value={courses.data?.length ?? 0} />
        <StatCard label="Subjects" value={subjects.data?.length ?? 0} />
      </div>

      <div className="mt-6 grid gap-6 lg:grid-cols-2">
        <Card className="p-4">
          <h2 className="font-display text-sm font-semibold">Setup checklist</h2>
          <p className="mt-1 text-sm text-body-muted">
            A teacher can only publish an assignment once every step below is done.
          </p>

          <ul className="mt-4 space-y-3">
            {setupSteps.map((step) => (
              <li key={step.label} className="flex items-center justify-between gap-3">
                <span className="flex items-center gap-2.5 text-sm">
                  <span
                    aria-hidden
                    className={
                      step.done
                        ? 'h-2 w-2 rounded-full bg-acid-deep'
                        : 'h-2 w-2 rounded-full bg-rule'
                    }
                  />
                  <span className={step.done ? 'text-body-muted' : 'text-ink'}>
                    {step.label}
                  </span>
                </span>

                <Link
                  to={step.to}
                  className="text-sm text-ink hover:text-body-muted font-medium"
                >
                  {step.done ? 'Review' : 'Set up'}
                </Link>
              </li>
            ))}
          </ul>
        </Card>

        <Card className="p-4">
          <div className="flex items-center justify-between">
            <h2 className="font-display text-sm font-semibold">System Audit & Activity Feed</h2>
            <Badge tone="neutral">{auditLogs.data?.length ?? 0} Events</Badge>
          </div>
          <p className="mt-1 text-sm text-body-muted">
            Live log of actions taken across administrative and academic sections.
          </p>

          {auditLogs.isLoading && <Spinner label="Loading activity feed" />}

          {auditLogs.isSuccess && (auditLogs.data?.length ?? 0) === 0 && (
            <EmptyState
              title="No activity yet"
              description="System actions and audit events will show up here."
            />
          )}

          {auditLogs.isSuccess && (auditLogs.data?.length ?? 0) > 0 && (
            <ul className="mt-4 max-h-[280px] overflow-y-auto divide-y divide-rule pr-1 space-y-2">
              {(auditLogs.data ?? []).slice(0, 10).map((log) => (
                <li key={log.auditLogId} className="pt-2 text-xs">
                  <div className="flex items-center justify-between gap-2">
                    <div className="flex items-center gap-2">
                      <Avatar name={log.userName || 'System'} id={log.userId} size="xs" />
                      <span className="font-medium text-ink">{log.userName || 'System'}</span>
                    </div>
                    <Badge tone={log.action === 'CREATE' || log.action === 'PUBLISH' ? 'open' : 'neutral'}>
                      {log.action}
                    </Badge>
                  </div>
                  <p className="mt-1 text-body-muted line-clamp-1">
                    {log.newValues || `${log.action} on ${log.entityName}`}
                  </p>
                  <p className="mt-0.5 text-body-faint">
                    <Code>{formatDateTime(log.createdAt)}</Code>
                  </p>
                </li>
              ))}
            </ul>
          )}
        </Card>
      </div>
    </>
  );
}

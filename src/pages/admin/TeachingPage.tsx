import { useState, type FormEvent } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { Plus } from 'lucide-react';

import { api, errorMessage } from '../../lib/api';
import { useAuth } from '../../auth/AuthContext';
import {
  useAcademicYears,
  useCourseSubjects,
  useStudents,
  useTeachers,
} from '../../lib/hooks';
import { useDataTable } from '../../lib/useDataTable';
import { PaginationBar, SearchInput, SortTh } from '../../components/DataTableControls';
import { Avatar } from '../../components/Avatar';
import { cx, formatDate } from '../../lib/format';
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
  Td,
} from '../../components/ui';
import type { StudentEnrollment, TeacherSubject } from '../../lib/types';

type TabKey = 'subjects' | 'enrolments';

export default function TeachingPage() {
  const { user } = useAuth();
  const queryClient = useQueryClient();

  const [tab, setTab] = useState<TabKey>('subjects');
  const [open, setOpen] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);
  const [teacherId, setTeacherId] = useState<number | null>(null);
  const [yearId, setYearId] = useState<number | null>(null);

  const teachers = useTeachers();
  const students = useStudents();
  const years = useAcademicYears();
  const courseSubjects = useCourseSubjects();

  const selectedTeacher = teacherId ?? teachers.data?.[0]?.teacherId ?? null;
  const selectedYear = yearId ?? years.data?.[0]?.academicYearId ?? null;

  const teacherSubjects = useQuery({
    queryKey: ['teacher-subjects', selectedTeacher],
    enabled: Boolean(selectedTeacher),
    queryFn: async () => {
      const { data } = await api.get<TeacherSubject[]>(
        `/api/TeacherSubject/teacher/${selectedTeacher}`,
      );
      return data;
    },
  });

  const enrolments = useQuery({
    queryKey: ['enrolments', selectedYear],
    enabled: Boolean(selectedYear),
    queryFn: async () => {
      const { data } = await api.get<StudentEnrollment[]>(
        `/api/StudentEnrollment/academic-year/${selectedYear}`,
      );
      return data;
    },
  });

  const tsTable = useDataTable<TeacherSubject>({
    data: teacherSubjects.data ?? [],
    searchFields: [(ts) => ts.subjectName, (ts) => ts.courseName, (ts) => ts.teacherName],
    pageSize: 10,
  });

  const enrolTable = useDataTable<StudentEnrollment>({
    data: enrolments.data ?? [],
    searchFields: [
      (e) => e.rollNumber,
      (e) => e.studentName,
      (e) => e.studentCode,
      (e) => e.academicYearName,
    ],
    pageSize: 10,
  });

  const create = useMutation({
    mutationFn: async ({ url, body }: { url: string; body: unknown }) => {
      await api.post(url, body);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['teacher-subjects'] });
      queryClient.invalidateQueries({ queryKey: ['enrolments'] });
      setOpen(false);
    },
    onError: (error) => setFormError(errorMessage(error)),
  });

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError(null);

    const form = new FormData(event.currentTarget);
    const institutionId = user?.institutionId ?? 0;

    if (tab === 'subjects') {
      create.mutate({
        url: '/api/TeacherSubject',
        body: {
          institutionId,
          teacherId: Number(form.get('teacherId')),
          courseSubjectId: Number(form.get('courseSubjectId')),
        },
      });
      return;
    }

    const enrollmentDate = String(form.get('enrollmentDate') ?? '').trim();

    create.mutate({
      url: '/api/StudentEnrollment',
      body: {
        institutionId,
        studentId: Number(form.get('studentId')),
        academicYearId: Number(form.get('academicYearId')),
        rollNumber: String(form.get('rollNumber') ?? '').trim(),
        enrollmentDate: enrollmentDate || null,
        isActive: true,
      },
    });
  }

  return (
    <>
      <PageHeader
        title="Teaching and enrolment"
        subtitle="Who teaches what, and who sits in which academic year."
        action={
          <Button
            onClick={() => {
              setFormError(null);
              setOpen(true);
            }}
          >
            <Plus className="h-4 w-4" />
            {tab === 'subjects' ? 'Assign subject' : 'Enrol student'}
          </Button>
        }
      />

      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <div className="flex flex-wrap items-center gap-1.5">
          {(
            [
              ['subjects', 'Teacher subjects'],
              ['enrolments', 'Enrolments'],
            ] as [TabKey, string][]
          ).map(([key, label]) => (
            <button
              key={key}
              type="button"
              onClick={() => setTab(key)}
              className={cx(
                'rounded-lg px-3 py-1.5 text-sm transition-colors',
                tab === key
                  ? 'bg-ink text-paper font-medium'
                  : 'border border-rule bg-white text-body-muted hover:text-ink',
              )}
            >
              {label}
            </button>
          ))}
        </div>

        <div className="flex flex-wrap items-center gap-3">
          {tab === 'subjects' ? (
            <>
              <SearchInput
                value={tsTable.searchQuery}
                onChange={tsTable.setSearchQuery}
                placeholder="Filter assigned subjects..."
              />
              <Select
                value={selectedTeacher ?? ''}
                onChange={(event) => setTeacherId(Number(event.target.value))}
                className="w-auto"
              >
                {teachers.data?.map((row) => (
                  <option key={row.teacherId} value={row.teacherId}>
                    {row.teacherName}
                  </option>
                ))}
              </Select>
            </>
          ) : (
            <>
              <SearchInput
                value={enrolTable.searchQuery}
                onChange={enrolTable.setSearchQuery}
                placeholder="Filter enrolments by roll or student..."
              />
              <Select
                value={selectedYear ?? ''}
                onChange={(event) => setYearId(Number(event.target.value))}
                className="w-auto"
              >
                {years.data?.map((row) => (
                  <option key={row.academicYearId} value={row.academicYearId}>
                    {row.batchName} · {row.yearName}
                  </option>
                ))}
              </Select>
            </>
          )}
        </div>
      </div>

      <Card>
        {tab === 'subjects' &&
          (teacherSubjects.isLoading ? (
            <Spinner />
          ) : (teacherSubjects.data?.length ?? 0) === 0 ? (
            <EmptyState
              title="No subjects assigned"
              description="A teacher can only create assignments for subjects assigned to them."
            />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Subject"
                        sortKey="subject"
                        currentSortKey={tsTable.sortKey}
                        currentSortDirection={tsTable.sortDirection}
                        onSort={(k) => tsTable.toggleSort(k, (r) => r.subjectName)}
                      />
                      <SortTh
                        label="Course"
                        sortKey="course"
                        currentSortKey={tsTable.sortKey}
                        currentSortDirection={tsTable.sortDirection}
                        onSort={(k) => tsTable.toggleSort(k, (r) => r.courseName)}
                      />
                      <SortTh
                        label="Teacher"
                        sortKey="teacher"
                        currentSortKey={tsTable.sortKey}
                        currentSortDirection={tsTable.sortDirection}
                        onSort={(k) => tsTable.toggleSort(k, (r) => r.teacherName)}
                      />
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {tsTable.paginatedItems.map((row) => (
                      <tr key={row.teacherSubjectId}>
                        <Td className="font-medium text-ink">{row.subjectName}</Td>
                        <Td className="text-body-muted">{row.courseName}</Td>
                        <Td className="text-body-muted">
                          <div className="flex items-center gap-2.5">
                            <Avatar name={row.teacherName} id={row.teacherId} size="xs" />
                            <span>{row.teacherName}</span>
                          </div>
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={tsTable.currentPage}
                totalPages={tsTable.totalPages}
                totalItems={tsTable.totalItems}
                startIndex={tsTable.startIndex}
                endIndex={tsTable.endIndex}
                onPageChange={tsTable.setCurrentPage}
                pageSize={tsTable.pageSize}
                onPageSizeChange={tsTable.setPageSize}
              />
            </>
          ))}

        {tab === 'enrolments' &&
          (enrolments.isLoading ? (
            <Spinner />
          ) : (enrolments.data?.length ?? 0) === 0 ? (
            <EmptyState
              title="Nobody enrolled"
              description="Students see assignments only for the year they are enrolled in."
            />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Roll"
                        sortKey="roll"
                        currentSortKey={enrolTable.sortKey}
                        currentSortDirection={enrolTable.sortDirection}
                        onSort={(k) => enrolTable.toggleSort(k, (r) => r.rollNumber)}
                      />
                      <SortTh
                        label="Student"
                        sortKey="student"
                        currentSortKey={enrolTable.sortKey}
                        currentSortDirection={enrolTable.sortDirection}
                        onSort={(k) => enrolTable.toggleSort(k, (r) => r.studentName)}
                      />
                      <SortTh
                        label="Year"
                        sortKey="year"
                        currentSortKey={enrolTable.sortKey}
                        currentSortDirection={enrolTable.sortDirection}
                        onSort={(k) => enrolTable.toggleSort(k, (r) => r.academicYearName)}
                      />
                      <SortTh
                        label="Enrolled"
                        sortKey="date"
                        currentSortKey={enrolTable.sortKey}
                        currentSortDirection={enrolTable.sortDirection}
                        onSort={(k) => enrolTable.toggleSort(k, (r) => r.enrollmentDate)}
                      />
                      <th className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted">
                        Status
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {enrolTable.paginatedItems.map((row) => (
                      <tr key={row.enrollmentId}>
                        <Td>
                          <Code>{row.rollNumber}</Code>
                        </Td>
                        <Td>
                          <div className="flex items-center gap-3">
                            <Avatar name={row.studentName} id={row.studentId} size="sm" />
                            <div>
                              <span className="text-ink font-medium">{row.studentName}</span>
                              <span className="block text-xs text-body-faint">
                                {row.studentCode}
                              </span>
                            </div>
                          </div>
                        </Td>
                        <Td className="text-body-muted">{row.academicYearName}</Td>
                        <Td className="text-body-muted">{formatDate(row.enrollmentDate)}</Td>
                        <Td>
                          <Badge tone={row.isActive ? 'open' : 'neutral'}>
                            {row.isActive ? 'Active' : 'Inactive'}
                          </Badge>
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={enrolTable.currentPage}
                totalPages={enrolTable.totalPages}
                totalItems={enrolTable.totalItems}
                startIndex={enrolTable.startIndex}
                endIndex={enrolTable.endIndex}
                onPageChange={enrolTable.setCurrentPage}
                pageSize={enrolTable.pageSize}
                onPageSizeChange={enrolTable.setPageSize}
              />
            </>
          ))}
      </Card>

      <Modal
        open={open}
        onClose={() => setOpen(false)}
        title={tab === 'subjects' ? 'Assign a subject' : 'Enrol a student'}
      >
        <form onSubmit={handleSubmit} className="space-y-4">
          {formError && <ErrorNote message={formError} />}

          {tab === 'subjects' ? (
            <>
              <Field label="Teacher">
                <Select name="teacherId" required defaultValue={selectedTeacher ?? ''}>
                  {teachers.data?.map((row) => (
                    <option key={row.teacherId} value={row.teacherId}>
                      {row.teacherName}
                    </option>
                  ))}
                </Select>
              </Field>

              <Field label="Course subject">
                <Select name="courseSubjectId" required>
                  {courseSubjects.data?.map((row) => (
                    <option key={row.courseSubjectId} value={row.courseSubjectId}>
                      {row.subjectName} — {row.courseName}
                    </option>
                  ))}
                </Select>
              </Field>
            </>
          ) : (
            <>
              <Field label="Student">
                <Select name="studentId" required>
                  {students.data?.map((row) => (
                    <option key={row.studentId} value={row.studentId}>
                      {row.studentName} ({row.studentCode})
                    </option>
                  ))}
                </Select>
              </Field>

              <Field label="Academic year">
                <Select name="academicYearId" required defaultValue={selectedYear ?? ''}>
                  {years.data?.map((row) => (
                    <option key={row.academicYearId} value={row.academicYearId}>
                      {row.batchName} · {row.yearName}
                    </option>
                  ))}
                </Select>
              </Field>

              <div className="grid gap-4 sm:grid-cols-2">
                <Field label="Roll number">
                  <Input name="rollNumber" required maxLength={20} />
                </Field>
                <Field label="Enrolment date">
                  <Input name="enrollmentDate" type="date" />
                </Field>
              </div>
            </>
          )}

          <div className="flex justify-end gap-2">
            <Button type="button" variant="secondary" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button type="submit" loading={create.isPending}>
              Save
            </Button>
          </div>
        </form>
      </Modal>
    </>
  );
}

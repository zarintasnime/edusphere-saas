import { useState, type FormEvent } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Plus } from 'lucide-react';

import { api, errorMessage } from '../../lib/api';
import { useAuth } from '../../auth/AuthContext';
import { useDepartments, useStudents, useTeachers } from '../../lib/hooks';
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
import type { StudentProfile, TeacherProfile } from '../../lib/types';

type TabKey = 'teachers' | 'students';

export default function PeoplePage() {
  const { user } = useAuth();
  const queryClient = useQueryClient();

  const [tab, setTab] = useState<TabKey>('teachers');
  const [open, setOpen] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  const teachers = useTeachers();
  const students = useStudents();
  const departments = useDepartments();

  const teacherTable = useDataTable<TeacherProfile>({
    data: teachers.data ?? [],
    searchFields: [
      (t) => t.employeeCode,
      (t) => t.teacherName,
      (t) => t.email,
      (t) => t.departmentName,
    ],
    pageSize: 10,
  });

  const studentTable = useDataTable<StudentProfile>({
    data: students.data ?? [],
    searchFields: [
      (s) => s.studentCode,
      (s) => s.studentName,
      (s) => s.email,
    ],
    pageSize: 10,
  });

  const create = useMutation({
    mutationFn: async ({ url, body }: { url: string; body: unknown }) => {
      await api.post(url, body);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [tab] });
      setOpen(false);
    },
    onError: (error) => setFormError(errorMessage(error)),
  });

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError(null);

    const form = new FormData(event.currentTarget);
    const text = (key: string) => String(form.get(key) ?? '').trim();
    const institutionId = user?.institutionId ?? 0;

    if (tab === 'teachers') {
      create.mutate({
        url: '/api/Teacher',
        body: {
          institutionId,
          departmentId: Number(form.get('departmentId')),
          teacherName: text('name'),
          email: text('email'),
          password: text('password'),
          employeeCode: text('code'),
          qualification: text('qualification') || null,
          joiningDate: text('joiningDate') || null,
          isActive: true,
        },
      });
      return;
    }

    create.mutate({
      url: '/api/Student',
      body: {
        institutionId,
        studentName: text('name'),
        email: text('email'),
        password: text('password'),
        studentCode: text('code'),
        admissionDate: text('admissionDate') || null,
        isActive: true,
      },
    });
  }

  return (
    <>
      <PageHeader
        title="People"
        subtitle="Creating someone here also creates their sign-in account."
        action={
          <Button
            onClick={() => {
              setFormError(null);
              setOpen(true);
            }}
          >
            <Plus className="h-4 w-4" />
            Add {tab === 'teachers' ? 'teacher' : 'student'}
          </Button>
        }
      />

      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <div className="flex gap-1.5">
          {(['teachers', 'students'] as TabKey[]).map((key) => (
            <button
              key={key}
              type="button"
              onClick={() => setTab(key)}
              className={cx(
                'rounded-lg px-3 py-1.5 text-sm capitalize transition-colors',
                tab === key
                  ? 'bg-ink text-paper font-medium'
                  : 'border border-rule bg-white text-body-muted hover:text-ink',
              )}
            >
              {key}
            </button>
          ))}
        </div>

        {tab === 'teachers' ? (
          <SearchInput
            value={teacherTable.searchQuery}
            onChange={teacherTable.setSearchQuery}
            placeholder="Search teachers by code, name, email, department..."
          />
        ) : (
          <SearchInput
            value={studentTable.searchQuery}
            onChange={studentTable.setSearchQuery}
            placeholder="Search students by code, name, email..."
          />
        )}
      </div>

      <Card>
        {tab === 'teachers' &&
          (teachers.isLoading ? (
            <Spinner />
          ) : teachers.data?.length === 0 ? (
            <EmptyState
              title="No teachers yet"
              description="Add a teacher so assignments can be created."
            />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Employee code"
                        sortKey="code"
                        currentSortKey={teacherTable.sortKey}
                        currentSortDirection={teacherTable.sortDirection}
                        onSort={(k) => teacherTable.toggleSort(k, (row) => row.employeeCode)}
                      />
                      <SortTh
                        label="Name"
                        sortKey="name"
                        currentSortKey={teacherTable.sortKey}
                        currentSortDirection={teacherTable.sortDirection}
                        onSort={(k) => teacherTable.toggleSort(k, (row) => row.teacherName)}
                      />
                      <SortTh
                        label="Department"
                        sortKey="department"
                        currentSortKey={teacherTable.sortKey}
                        currentSortDirection={teacherTable.sortDirection}
                        onSort={(k) => teacherTable.toggleSort(k, (row) => row.departmentName)}
                      />
                      <SortTh
                        label="Joined"
                        sortKey="joined"
                        currentSortKey={teacherTable.sortKey}
                        currentSortDirection={teacherTable.sortDirection}
                        onSort={(k) => teacherTable.toggleSort(k, (row) => row.joiningDate)}
                      />
                      <th className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted">
                        Status
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {teacherTable.paginatedItems.map((row) => (
                      <tr key={row.teacherId}>
                        <Td>
                          <Code>{row.employeeCode}</Code>
                        </Td>
                        <Td>
                          <div className="flex items-center gap-3">
                            <Avatar email={row.email} name={row.teacherName} id={row.teacherId} size="sm" />
                            <div>
                              <span className="text-ink font-medium">{row.teacherName}</span>
                              <span className="block text-xs text-body-faint">{row.email}</span>
                            </div>
                          </div>
                        </Td>
                        <Td className="text-body-muted">{row.departmentName}</Td>
                        <Td className="text-body-muted">{formatDate(row.joiningDate)}</Td>
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
                currentPage={teacherTable.currentPage}
                totalPages={teacherTable.totalPages}
                totalItems={teacherTable.totalItems}
                startIndex={teacherTable.startIndex}
                endIndex={teacherTable.endIndex}
                onPageChange={teacherTable.setCurrentPage}
                pageSize={teacherTable.pageSize}
                onPageSizeChange={teacherTable.setPageSize}
              />
            </>
          ))}

        {tab === 'students' &&
          (students.isLoading ? (
            <Spinner />
          ) : students.data?.length === 0 ? (
            <EmptyState
              title="No students yet"
              description="Add a student, then enrol them in an academic year."
            />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Student code"
                        sortKey="code"
                        currentSortKey={studentTable.sortKey}
                        currentSortDirection={studentTable.sortDirection}
                        onSort={(k) => studentTable.toggleSort(k, (row) => row.studentCode)}
                      />
                      <SortTh
                        label="Name"
                        sortKey="name"
                        currentSortKey={studentTable.sortKey}
                        currentSortDirection={studentTable.sortDirection}
                        onSort={(k) => studentTable.toggleSort(k, (row) => row.studentName)}
                      />
                      <SortTh
                        label="Admitted"
                        sortKey="admitted"
                        currentSortKey={studentTable.sortKey}
                        currentSortDirection={studentTable.sortDirection}
                        onSort={(k) => studentTable.toggleSort(k, (row) => row.admissionDate)}
                      />
                      <th className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted">
                        Status
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {studentTable.paginatedItems.map((row) => (
                      <tr key={row.studentId}>
                        <Td>
                          <Code>{row.studentCode}</Code>
                        </Td>
                        <Td>
                          <div className="flex items-center gap-3">
                            <Avatar email={row.email} name={row.studentName} id={row.studentId} size="sm" />
                            <div>
                              <span className="text-ink font-medium">{row.studentName}</span>
                              <span className="block text-xs text-body-faint">{row.email}</span>
                            </div>
                          </div>
                        </Td>
                        <Td className="text-body-muted">{formatDate(row.admissionDate)}</Td>
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
                currentPage={studentTable.currentPage}
                totalPages={studentTable.totalPages}
                totalItems={studentTable.totalItems}
                startIndex={studentTable.startIndex}
                endIndex={studentTable.endIndex}
                onPageChange={studentTable.setCurrentPage}
                pageSize={studentTable.pageSize}
                onPageSizeChange={studentTable.setPageSize}
              />
            </>
          ))}
      </Card>

      <Modal
        open={open}
        onClose={() => setOpen(false)}
        title={tab === 'teachers' ? 'Add teacher' : 'Add student'}
      >
        <form onSubmit={handleSubmit} className="space-y-4">
          {formError && <ErrorNote message={formError} />}

          <Field label="Full name">
            <Input name="name" required maxLength={100} />
          </Field>

          <div className="grid gap-4 sm:grid-cols-2">
            <Field label="Email">
              <Input name="email" type="email" required maxLength={150} />
            </Field>

            <Field label="Temporary password" hint="At least 6 characters.">
              <Input name="password" type="text" minLength={6} required />
            </Field>
          </div>

          {tab === 'teachers' ? (
            <>
              <Field label="Department">
                <Select name="departmentId" required>
                  {departments.data?.map((row) => (
                    <option key={row.departmentId} value={row.departmentId}>
                      {row.departmentName}
                    </option>
                  ))}
                </Select>
              </Field>

              <div className="grid gap-4 sm:grid-cols-2">
                <Field label="Employee code">
                  <Input name="code" required maxLength={30} placeholder="EMP-1043" />
                </Field>
                <Field label="Joining date">
                  <Input name="joiningDate" type="date" />
                </Field>
              </div>

              <Field label="Qualification">
                <Input name="qualification" maxLength={150} />
              </Field>
            </>
          ) : (
            <div className="grid gap-4 sm:grid-cols-2">
              <Field label="Student code">
                <Input name="code" required maxLength={30} placeholder="2023-1-60-041" />
              </Field>
              <Field label="Admission date">
                <Input name="admissionDate" type="date" />
              </Field>
            </div>
          )}

          <div className="flex justify-end gap-2">
            <Button type="button" variant="secondary" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button type="submit" loading={create.isPending}>
              Create account
            </Button>
          </div>
        </form>
      </Modal>
    </>
  );
}

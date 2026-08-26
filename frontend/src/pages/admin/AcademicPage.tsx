import { useState, type FormEvent } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { Plus } from 'lucide-react';

import { api, errorMessage } from '../../lib/api';
import { useAuth } from '../../auth/AuthContext';
import {
  useAcademicYears,
  useBatches,
  useCourseSubjects,
  useCourses,
  useDepartments,
  useSubjects,
} from '../../lib/hooks';
import { useDataTable } from '../../lib/useDataTable';
import { PaginationBar, SearchInput, SortTh } from '../../components/DataTableControls';
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
import { cx } from '../../lib/format';
import type { AcademicYear, Batch, Course, CourseSubject, Department, Subject } from '../../lib/types';

type TabKey = 'departments' | 'courses' | 'subjects' | 'mapping' | 'batches' | 'years';

const tabs: { key: TabKey; label: string }[] = [
  { key: 'departments', label: 'Departments' },
  { key: 'courses', label: 'Courses' },
  { key: 'subjects', label: 'Subjects' },
  { key: 'mapping', label: 'Course subjects' },
  { key: 'batches', label: 'Batches' },
  { key: 'years', label: 'Academic years' },
];

export default function AcademicPage() {
  const { user } = useAuth();
  const queryClient = useQueryClient();

  const [tab, setTab] = useState<TabKey>('departments');
  const [open, setOpen] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  const departments = useDepartments();
  const courses = useCourses();
  const subjects = useSubjects();
  const batches = useBatches();
  const years = useAcademicYears();
  const mapping = useCourseSubjects();

  const deptTable = useDataTable<Department>({
    data: departments.data ?? [],
    searchFields: [(d) => d.departmentCode, (d) => d.departmentName, (d) => d.description ?? ''],
    pageSize: 10,
  });

  const courseTable = useDataTable<Course>({
    data: courses.data ?? [],
    searchFields: [(c) => c.courseCode, (c) => c.courseName, (c) => c.departmentName],
    pageSize: 10,
  });

  const subjectTable = useDataTable<Subject>({
    data: subjects.data ?? [],
    searchFields: [(s) => s.subjectCode, (s) => s.subjectName],
    pageSize: 10,
  });

  const mappingTable = useDataTable<CourseSubject>({
    data: mapping.data ?? [],
    searchFields: [(m) => m.courseCode, (m) => m.courseName, (m) => m.subjectCode, (m) => m.subjectName],
    pageSize: 10,
  });

  const batchTable = useDataTable<Batch>({
    data: batches.data ?? [],
    searchFields: [(b) => b.batchCode, (b) => b.batchName, (b) => b.courseName],
    pageSize: 10,
  });

  const yearTable = useDataTable<AcademicYear>({
    data: years.data ?? [],
    searchFields: [(y) => y.yearName, (y) => y.batchName],
    pageSize: 10,
  });

  const create = useMutation({
    mutationFn: async ({ url, body }: { url: string; body: unknown }) => {
      await api.post(url, body);
    },
    onSuccess: () => {
      queryClient.invalidateQueries();
      setOpen(false);
    },
    onError: (error) => setFormError(errorMessage(error)),
  });

  const institutionId = user?.institutionId ?? 0;

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError(null);

    const form = new FormData(event.currentTarget);
    const text = (key: string) => String(form.get(key) ?? '').trim();
    const num = (key: string) => Number(form.get(key));

    if (tab === 'departments') {
      create.mutate({
        url: '/api/Academic/department',
        body: {
          institutionId,
          departmentCode: text('departmentCode'),
          departmentName: text('departmentName'),
          description: text('description') || null,
          isActive: true,
        },
      });
      return;
    }

    if (tab === 'courses') {
      create.mutate({
        url: '/api/Academic/course',
        body: {
          institutionId,
          departmentId: num('departmentId'),
          courseCode: text('courseCode'),
          courseName: text('courseName'),
          description: text('description') || null,
          isActive: true,
        },
      });
      return;
    }

    if (tab === 'subjects') {
      create.mutate({
        url: '/api/Academic/subject',
        body: {
          institutionId,
          subjectCode: text('subjectCode'),
          subjectName: text('subjectName'),
          description: text('description') || null,
          isActive: true,
        },
      });
      return;
    }

    if (tab === 'mapping') {
      create.mutate({
        url: '/api/CourseSubject',
        body: {
          institutionId,
          courseId: num('courseId'),
          subjectId: num('subjectId'),
        },
      });
      return;
    }

    if (tab === 'batches') {
      const endYear = text('endYear');
      create.mutate({
        url: '/api/Academic/batch',
        body: {
          institutionId,
          courseId: num('courseId'),
          batchCode: text('batchCode'),
          batchName: text('batchName'),
          startYear: num('startYear'),
          endYear: endYear ? Number(endYear) : null,
          isActive: true,
        },
      });
      return;
    }

    create.mutate({
      url: '/api/Academic/academic-year',
      body: {
        institutionId,
        batchId: num('batchId'),
        yearName: text('yearName'),
        yearOrder: num('yearOrder'),
        isActive: true,
      },
    });
  }

  const activeLabel = tabs.find((item) => item.key === tab)!.label;

  return (
    <>
      <PageHeader
        title="Academic structure"
        subtitle="Departments hold courses, courses hold subjects, batches hold years."
        action={
          <Button
            onClick={() => {
              setFormError(null);
              setOpen(true);
            }}
          >
            <Plus className="h-4 w-4" />
            Add
          </Button>
        }
      />

      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <div className="flex flex-wrap gap-1.5">
          {tabs.map((item) => (
            <button
              key={item.key}
              type="button"
              onClick={() => setTab(item.key)}
              className={cx(
                'rounded-lg px-3 py-1.5 text-sm transition-colors',
                tab === item.key
                  ? 'bg-ink text-paper font-medium'
                  : 'border border-rule bg-white text-body-muted hover:text-ink',
              )}
            >
              {item.label}
            </button>
          ))}
        </div>

        {tab === 'departments' && (
          <SearchInput
            value={deptTable.searchQuery}
            onChange={deptTable.setSearchQuery}
            placeholder="Search departments..."
          />
        )}
        {tab === 'courses' && (
          <SearchInput
            value={courseTable.searchQuery}
            onChange={courseTable.setSearchQuery}
            placeholder="Search courses..."
          />
        )}
        {tab === 'subjects' && (
          <SearchInput
            value={subjectTable.searchQuery}
            onChange={subjectTable.setSearchQuery}
            placeholder="Search subjects..."
          />
        )}
        {tab === 'mapping' && (
          <SearchInput
            value={mappingTable.searchQuery}
            onChange={mappingTable.setSearchQuery}
            placeholder="Search course subjects..."
          />
        )}
        {tab === 'batches' && (
          <SearchInput
            value={batchTable.searchQuery}
            onChange={batchTable.setSearchQuery}
            placeholder="Search batches..."
          />
        )}
        {tab === 'years' && (
          <SearchInput
            value={yearTable.searchQuery}
            onChange={yearTable.setSearchQuery}
            placeholder="Search academic years..."
          />
        )}
      </div>

      <Card>
        {tab === 'departments' &&
          (departments.isLoading ? (
            <Spinner />
          ) : departments.data?.length === 0 ? (
            <EmptyState title="Nothing here yet" description="Use Add to create the first record." />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Code"
                        sortKey="code"
                        currentSortKey={deptTable.sortKey}
                        currentSortDirection={deptTable.sortDirection}
                        onSort={(k) => deptTable.toggleSort(k, (r) => r.departmentCode)}
                      />
                      <SortTh
                        label="Department"
                        sortKey="name"
                        currentSortKey={deptTable.sortKey}
                        currentSortDirection={deptTable.sortDirection}
                        onSort={(k) => deptTable.toggleSort(k, (r) => r.departmentName)}
                      />
                      <th className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted">
                        Status
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {deptTable.paginatedItems.map((row) => (
                      <tr key={row.departmentId}>
                        <Td>
                          <Code>{row.departmentCode}</Code>
                        </Td>
                        <Td className="font-medium text-ink">{row.departmentName}</Td>
                        <Td>
                          <ActiveBadge active={row.isActive} />
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={deptTable.currentPage}
                totalPages={deptTable.totalPages}
                totalItems={deptTable.totalItems}
                startIndex={deptTable.startIndex}
                endIndex={deptTable.endIndex}
                onPageChange={deptTable.setCurrentPage}
                pageSize={deptTable.pageSize}
                onPageSizeChange={deptTable.setPageSize}
              />
            </>
          ))}

        {tab === 'courses' &&
          (courses.isLoading ? (
            <Spinner />
          ) : courses.data?.length === 0 ? (
            <EmptyState title="Nothing here yet" description="Use Add to create the first record." />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Code"
                        sortKey="code"
                        currentSortKey={courseTable.sortKey}
                        currentSortDirection={courseTable.sortDirection}
                        onSort={(k) => courseTable.toggleSort(k, (r) => r.courseCode)}
                      />
                      <SortTh
                        label="Course"
                        sortKey="name"
                        currentSortKey={courseTable.sortKey}
                        currentSortDirection={courseTable.sortDirection}
                        onSort={(k) => courseTable.toggleSort(k, (r) => r.courseName)}
                      />
                      <SortTh
                        label="Department"
                        sortKey="department"
                        currentSortKey={courseTable.sortKey}
                        currentSortDirection={courseTable.sortDirection}
                        onSort={(k) => courseTable.toggleSort(k, (r) => r.departmentName)}
                      />
                      <th className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted">
                        Status
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {courseTable.paginatedItems.map((row) => (
                      <tr key={row.courseId}>
                        <Td>
                          <Code>{row.courseCode}</Code>
                        </Td>
                        <Td className="font-medium text-ink">{row.courseName}</Td>
                        <Td className="text-body-muted">{row.departmentName}</Td>
                        <Td>
                          <ActiveBadge active={row.isActive} />
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={courseTable.currentPage}
                totalPages={courseTable.totalPages}
                totalItems={courseTable.totalItems}
                startIndex={courseTable.startIndex}
                endIndex={courseTable.endIndex}
                onPageChange={courseTable.setCurrentPage}
                pageSize={courseTable.pageSize}
                onPageSizeChange={courseTable.setPageSize}
              />
            </>
          ))}

        {tab === 'subjects' &&
          (subjects.isLoading ? (
            <Spinner />
          ) : subjects.data?.length === 0 ? (
            <EmptyState title="Nothing here yet" description="Use Add to create the first record." />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Code"
                        sortKey="code"
                        currentSortKey={subjectTable.sortKey}
                        currentSortDirection={subjectTable.sortDirection}
                        onSort={(k) => subjectTable.toggleSort(k, (r) => r.subjectCode)}
                      />
                      <SortTh
                        label="Subject"
                        sortKey="name"
                        currentSortKey={subjectTable.sortKey}
                        currentSortDirection={subjectTable.sortDirection}
                        onSort={(k) => subjectTable.toggleSort(k, (r) => r.subjectName)}
                      />
                      <th className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted">
                        Status
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {subjectTable.paginatedItems.map((row) => (
                      <tr key={row.subjectId}>
                        <Td>
                          <Code>{row.subjectCode}</Code>
                        </Td>
                        <Td className="font-medium text-ink">{row.subjectName}</Td>
                        <Td>
                          <ActiveBadge active={row.isActive} />
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={subjectTable.currentPage}
                totalPages={subjectTable.totalPages}
                totalItems={subjectTable.totalItems}
                startIndex={subjectTable.startIndex}
                endIndex={subjectTable.endIndex}
                onPageChange={subjectTable.setCurrentPage}
                pageSize={subjectTable.pageSize}
                onPageSizeChange={subjectTable.setPageSize}
              />
            </>
          ))}

        {tab === 'mapping' &&
          (mapping.isLoading ? (
            <Spinner />
          ) : mapping.data?.length === 0 ? (
            <EmptyState title="Nothing here yet" description="Use Add to create the first record." />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Course"
                        sortKey="course"
                        currentSortKey={mappingTable.sortKey}
                        currentSortDirection={mappingTable.sortDirection}
                        onSort={(k) => mappingTable.toggleSort(k, (r) => r.courseName)}
                      />
                      <SortTh
                        label="Subject"
                        sortKey="subject"
                        currentSortKey={mappingTable.sortKey}
                        currentSortDirection={mappingTable.sortDirection}
                        onSort={(k) => mappingTable.toggleSort(k, (r) => r.subjectName)}
                      />
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {mappingTable.paginatedItems.map((row) => (
                      <tr key={row.courseSubjectId}>
                        <Td>
                          <span className="font-medium text-ink">{row.courseName}</span> <Code>{row.courseCode}</Code>
                        </Td>
                        <Td>
                          <span className="font-medium text-ink">{row.subjectName}</span> <Code>{row.subjectCode}</Code>
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={mappingTable.currentPage}
                totalPages={mappingTable.totalPages}
                totalItems={mappingTable.totalItems}
                startIndex={mappingTable.startIndex}
                endIndex={mappingTable.endIndex}
                onPageChange={mappingTable.setCurrentPage}
                pageSize={mappingTable.pageSize}
                onPageSizeChange={mappingTable.setPageSize}
              />
            </>
          ))}

        {tab === 'batches' &&
          (batches.isLoading ? (
            <Spinner />
          ) : batches.data?.length === 0 ? (
            <EmptyState title="Nothing here yet" description="Use Add to create the first record." />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Code"
                        sortKey="code"
                        currentSortKey={batchTable.sortKey}
                        currentSortDirection={batchTable.sortDirection}
                        onSort={(k) => batchTable.toggleSort(k, (r) => r.batchCode)}
                      />
                      <SortTh
                        label="Batch"
                        sortKey="batch"
                        currentSortKey={batchTable.sortKey}
                        currentSortDirection={batchTable.sortDirection}
                        onSort={(k) => batchTable.toggleSort(k, (r) => r.batchName)}
                      />
                      <SortTh
                        label="Course"
                        sortKey="course"
                        currentSortKey={batchTable.sortKey}
                        currentSortDirection={batchTable.sortDirection}
                        onSort={(k) => batchTable.toggleSort(k, (r) => r.courseName)}
                      />
                      <SortTh
                        label="Years"
                        sortKey="startYear"
                        currentSortKey={batchTable.sortKey}
                        currentSortDirection={batchTable.sortDirection}
                        onSort={(k) => batchTable.toggleSort(k, (r) => r.startYear)}
                      />
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {batchTable.paginatedItems.map((row) => (
                      <tr key={row.batchId}>
                        <Td>
                          <Code>{row.batchCode}</Code>
                        </Td>
                        <Td className="font-medium text-ink">{row.batchName}</Td>
                        <Td className="text-body-muted">{row.courseName}</Td>
                        <Td>
                          <Code>
                            {row.startYear}
                            {row.endYear ? `-${row.endYear}` : ''}
                          </Code>
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={batchTable.currentPage}
                totalPages={batchTable.totalPages}
                totalItems={batchTable.totalItems}
                startIndex={batchTable.startIndex}
                endIndex={batchTable.endIndex}
                onPageChange={batchTable.setCurrentPage}
                pageSize={batchTable.pageSize}
                onPageSizeChange={batchTable.setPageSize}
              />
            </>
          ))}

        {tab === 'years' &&
          (years.isLoading ? (
            <Spinner />
          ) : years.data?.length === 0 ? (
            <EmptyState title="Nothing here yet" description="Use Add to create the first record." />
          ) : (
            <>
              <div className="overflow-x-auto">
                <table className="w-full min-w-[36rem] text-left text-sm">
                  <thead>
                    <tr className="border-b border-ink/15">
                      <SortTh
                        label="Year"
                        sortKey="year"
                        currentSortKey={yearTable.sortKey}
                        currentSortDirection={yearTable.sortDirection}
                        onSort={(k) => yearTable.toggleSort(k, (r) => r.yearName)}
                      />
                      <SortTh
                        label="Batch"
                        sortKey="batch"
                        currentSortKey={yearTable.sortKey}
                        currentSortDirection={yearTable.sortDirection}
                        onSort={(k) => yearTable.toggleSort(k, (r) => r.batchName)}
                      />
                      <SortTh
                        label="Order"
                        sortKey="order"
                        currentSortKey={yearTable.sortKey}
                        currentSortDirection={yearTable.sortDirection}
                        onSort={(k) => yearTable.toggleSort(k, (r) => r.yearOrder)}
                      />
                      <th className="px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted">
                        Status
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-rule">
                    {yearTable.paginatedItems.map((row) => (
                      <tr key={row.academicYearId}>
                        <Td className="font-medium text-ink">{row.yearName}</Td>
                        <Td className="text-body-muted">{row.batchName}</Td>
                        <Td>
                          <Code>{row.yearOrder}</Code>
                        </Td>
                        <Td>
                          <ActiveBadge active={row.isActive} />
                        </Td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <PaginationBar
                currentPage={yearTable.currentPage}
                totalPages={yearTable.totalPages}
                totalItems={yearTable.totalItems}
                startIndex={yearTable.startIndex}
                endIndex={yearTable.endIndex}
                onPageChange={yearTable.setCurrentPage}
                pageSize={yearTable.pageSize}
                onPageSizeChange={yearTable.setPageSize}
              />
            </>
          ))}
      </Card>

      <Modal open={open} onClose={() => setOpen(false)} title={`Add to ${activeLabel}`}>
        <form onSubmit={handleSubmit} className="space-y-4">
          {formError && <ErrorNote message={formError} />}

          {tab === 'departments' && (
            <>
              <Field label="Code">
                <Input name="departmentCode" required maxLength={20} placeholder="CSE" />
              </Field>
              <Field label="Name">
                <Input name="departmentName" required maxLength={100} />
              </Field>
              <Field label="Description">
                <Input name="description" maxLength={250} />
              </Field>
            </>
          )}

          {tab === 'courses' && (
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
              <Field label="Code">
                <Input name="courseCode" required maxLength={20} placeholder="BSCSE" />
              </Field>
              <Field label="Name">
                <Input name="courseName" required maxLength={100} />
              </Field>
              <Field label="Description">
                <Input name="description" maxLength={250} />
              </Field>
            </>
          )}

          {tab === 'subjects' && (
            <>
              <Field label="Code">
                <Input name="subjectCode" required maxLength={20} placeholder="CSE-2101" />
              </Field>
              <Field label="Name">
                <Input name="subjectName" required maxLength={100} />
              </Field>
              <Field label="Description">
                <Input name="description" maxLength={250} />
              </Field>
            </>
          )}

          {tab === 'mapping' && (
            <>
              <Field label="Course">
                <Select name="courseId" required>
                  {courses.data?.map((row) => (
                    <option key={row.courseId} value={row.courseId}>
                      {row.courseName}
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="Subject">
                <Select name="subjectId" required>
                  {subjects.data?.map((row) => (
                    <option key={row.subjectId} value={row.subjectId}>
                      {row.subjectName}
                    </option>
                  ))}
                </Select>
              </Field>
            </>
          )}

          {tab === 'batches' && (
            <>
              <Field label="Course">
                <Select name="courseId" required>
                  {courses.data?.map((row) => (
                    <option key={row.courseId} value={row.courseId}>
                      {row.courseName}
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="Code">
                <Input name="batchCode" required maxLength={20} placeholder="BSCSE-58" />
              </Field>
              <Field label="Name">
                <Input name="batchName" required maxLength={100} />
              </Field>
              <div className="grid gap-4 sm:grid-cols-2">
                <Field label="Start year">
                  <Input name="startYear" type="number" required defaultValue={2024} />
                </Field>
                <Field label="End year">
                  <Input name="endYear" type="number" />
                </Field>
              </div>
            </>
          )}

          {tab === 'years' && (
            <>
              <Field label="Batch">
                <Select name="batchId" required>
                  {batches.data?.map((row) => (
                    <option key={row.batchId} value={row.batchId}>
                      {row.batchName}
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="Year name">
                <Input name="yearName" required maxLength={50} placeholder="2nd Year" />
              </Field>
              <Field label="Order" hint="1 for first year, 2 for second, and so on.">
                <Input name="yearOrder" type="number" min={1} required defaultValue={1} />
              </Field>
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

const ActiveBadge = ({ active }: { active: boolean }) => (
  <Badge tone={active ? 'open' : 'neutral'}>{active ? 'Active' : 'Inactive'}</Badge>
);

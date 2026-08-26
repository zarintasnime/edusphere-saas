import { useQuery } from '@tanstack/react-query';
import { Paperclip } from 'lucide-react';

import { api, errorMessage, fileUrl } from '../../lib/api';
import { formatBytes, formatDateTime } from '../../lib/format';
import { useStudentProfile } from '../../lib/hooks';
import { useDataTable } from '../../lib/useDataTable';
import { PaginationBar, SearchInput } from '../../components/DataTableControls';
import { SubmissionChip } from '../../components/StatusChips';
import { Avatar } from '../../components/Avatar';
import {
  Badge,
  Card,
  Code,
  EmptyState,
  ErrorNote,
  PageHeader,
  Spinner,
} from '../../components/ui';
import type { Assessment, Submission } from '../../lib/types';

export default function StudentSubmissionsPage() {
  const profile = useStudentProfile();

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

  const rawList = submissions.data ?? [];

  const dataTable = useDataTable<Submission>({
    data: rawList,
    searchFields: [
      (s) => s.assignmentTitle ?? '',
      (s) => s.submissionText ?? '',
    ],
    initialSortField: (s) => s.submittedAt,
    initialSortDirection: 'desc',
    pageSize: 10,
  });

  if (profile.isLoading) return <Spinner label="Loading your profile" />;
  if (profile.isError) return <ErrorNote message={errorMessage(profile.error)} />;

  return (
    <>
      <PageHeader
        title="My submissions"
        subtitle="What you handed in, and how it was marked."
      />

      <div className="mb-4">
        <SearchInput
          value={dataTable.searchQuery}
          onChange={dataTable.setSearchQuery}
          placeholder="Search by assignment title or notes..."
        />
      </div>

      <Card>
        {submissions.isLoading && <Spinner />}

        {submissions.isSuccess && rawList.length === 0 && (
          <EmptyState
            title="Nothing handed in yet"
            description="Your submissions and their feedback will be listed here."
          />
        )}

        {submissions.isSuccess && rawList.length > 0 && dataTable.paginatedItems.length === 0 && (
          <EmptyState
            title="No matching submissions"
            description="Try adjusting your search query."
          />
        )}

        <ul className="divide-y divide-rule">
          {dataTable.paginatedItems.map((submission) => (
            <SubmissionRow key={submission.submissionId} submission={submission} />
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
    </>
  );
}

function SubmissionRow({ submission }: { submission: Submission }) {
  const assessment = useQuery({
    queryKey: ['assessment', submission.submissionId],
    queryFn: async () => {
      try {
        const { data } = await api.get<Assessment>(
          `/api/Assessment/submission/${submission.submissionId}`,
        );
        return data;
      } catch {
        return null;
      }
    },
  });

  return (
    <li className="px-4 py-4">
      <div className="flex flex-wrap items-start justify-between gap-3">
        <div className="min-w-0">
          <p className="font-medium text-ink">{submission.assignmentTitle}</p>
          <p className="mt-0.5 text-xs text-body-muted">
            Handed in <Code>{formatDateTime(submission.submittedAt)}</Code>
            {submission.submissionVersion > 1 &&
              ` · version ${submission.submissionVersion}`}
          </p>
        </div>

        <div className="flex flex-wrap items-center gap-2">
          {submission.isLateSubmission && <Badge tone="over">Late</Badge>}
          <SubmissionChip status={submission.submissionStatus} />
          {assessment.data && (
            <Badge tone="open">{assessment.data.finalMarks} marks</Badge>
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

      {assessment.data && (
        <div className="mt-3 rounded-lg border border-rule bg-paper-warm px-3 py-2">
          <div className="flex items-center gap-2 mb-1">
            <Avatar name={assessment.data.teacherName} id={assessment.data.teacherId} size="xs" />
            <p className="text-xs uppercase tracking-wide text-body-faint">
              Feedback from {assessment.data.teacherName}
            </p>
          </div>
          <p className="mt-1 text-sm text-body-muted">
            {assessment.data.feedback || 'No written feedback was left.'}
          </p>
          {assessment.data.penaltyPercentageApplied > 0 && (
            <p className="mt-1 font-mono text-xs text-amberish">
              Late penalty applied: {assessment.data.penaltyPercentageApplied}%
            </p>
          )}
        </div>
      )}
    </li>
  );
}

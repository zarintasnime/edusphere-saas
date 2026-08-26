import { useState } from 'react';
import { CheckCheck } from 'lucide-react';

import {
  useMarkAllNotificationsRead,
  useMarkNotificationRead,
  useNotifications,
} from '../lib/hooks';
import { useDataTable } from '../lib/useDataTable';
import { PaginationBar, SearchInput } from '../components/DataTableControls';
import { cx, formatDateTime, timeAgo } from '../lib/format';
import {
  Badge,
  Button,
  Card,
  EmptyState,
  PageHeader,
  Select,
  Spinner,
} from '../components/ui';
import type { NotificationItem } from '../lib/types';

const label: Record<string, string> = {
  AssignmentPublished: 'Assignment',
  SubmissionReceived: 'Submission',
  AssessmentPublished: 'Grade',
};

const tone: Record<string, 'accent' | 'ink' | 'open'> = {
  AssignmentPublished: 'accent',
  SubmissionReceived: 'ink',
  AssessmentPublished: 'open',
};

export default function NotificationsPage() {
  const notifications = useNotifications();
  const markRead = useMarkNotificationRead();
  const markAll = useMarkAllNotificationsRead();

  const [readFilter, setReadFilter] = useState<string>('all');

  const rawItems = notifications.data ?? [];
  const unreadCount = rawItems.filter((item) => !item.isRead).length;

  const filteredByRead = rawItems.filter((item) => {
    if (readFilter === 'unread') return !item.isRead;
    if (readFilter === 'read') return item.isRead;
    return true;
  });

  const dataTable = useDataTable<NotificationItem>({
    data: filteredByRead,
    searchFields: [(n) => n.title, (n) => n.message],
    initialSortField: (n) => n.createdAt,
    initialSortDirection: 'desc',
    pageSize: 10,
  });

  return (
    <>
      <PageHeader
        title="Activity"
        subtitle="Everything addressed to you, newest first."
        action={
          unreadCount > 0 ? (
            <Button
              variant="secondary"
              loading={markAll.isPending}
              onClick={() => markAll.mutate()}
            >
              <CheckCheck className="h-4 w-4" />
              Mark all read
            </Button>
          ) : undefined
        }
      />

      <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
        <SearchInput
          value={dataTable.searchQuery}
          onChange={dataTable.setSearchQuery}
          placeholder="Search activity by keyword..."
        />

        <div className="flex items-center gap-2">
          <span className="font-mono text-[11px] uppercase tracking-[0.14em] text-body-muted">Filter:</span>
          <Select
            value={readFilter}
            onChange={(e) => setReadFilter(e.target.value)}
            className="w-auto text-xs py-1.5"
          >
            <option value="all">All activities</option>
            <option value="unread">Unread only</option>
            <option value="read">Read only</option>
          </Select>
        </div>
      </div>

      <Card>
        {notifications.isLoading && <Spinner />}

        {notifications.isSuccess && rawItems.length === 0 && (
          <EmptyState
            title="No activity yet"
            description="When an assignment is published, work is handed in, or a grade is released, it shows up here."
          />
        )}

        {notifications.isSuccess && rawItems.length > 0 && dataTable.paginatedItems.length === 0 && (
          <EmptyState
            title="No matching activity"
            description="Try adjusting your search query or read/unread filter."
          />
        )}

        <ul className="divide-y divide-rule">
          {dataTable.paginatedItems.map((item) => (
            <Row
              key={item.notificationId}
              item={item}
              onRead={() => markRead.mutate(item.notificationId)}
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
    </>
  );
}

function Row({ item, onRead }: { item: NotificationItem; onRead: () => void }) {
  return (
    <li
      className={cx(
        'flex flex-wrap items-start gap-x-4 gap-y-2 px-5 py-4 transition-colors',
        !item.isRead && 'bg-acid/[0.06]',
      )}
    >
      <button
        type="button"
        onClick={onRead}
        disabled={item.isRead}
        aria-label={item.isRead ? 'Already read' : 'Mark as read'}
        className={cx(
          'mt-1.5 h-2.5 w-2.5 shrink-0 rounded-full transition-transform',
          item.isRead
            ? 'bg-rule'
            : 'animate-pulseDot bg-acid-deep hover:scale-125',
        )}
      />

      <div className="min-w-0 flex-1">
        <div className="flex flex-wrap items-center gap-2">
          <p className="font-medium text-ink">{item.title}</p>
          <Badge tone={tone[item.notificationType] ?? 'neutral'}>
            {label[item.notificationType] ?? 'Update'}
          </Badge>
        </div>

        <p className="mt-1 text-sm leading-relaxed text-body-muted">
          {item.message}
        </p>
      </div>

      <p className="shrink-0 text-right font-mono text-[11px] text-body-faint">
        {timeAgo(item.createdAt)}
        <span className="block">{formatDateTime(item.createdAt)}</span>
      </p>
    </li>
  );
}

import { useEffect, useRef, useState } from 'react';
import { Link } from 'react-router-dom';
import { Bell, CheckCheck } from 'lucide-react';

import {
  useMarkAllNotificationsRead,
  useMarkNotificationRead,
  useNotifications,
  useUnreadCount,
} from '../lib/hooks';
import { cx, formatDateTime, timeAgo } from '../lib/format';
import type { NotificationItem } from '../lib/types';

/** Each event type gets its own edge colour so the feed is scannable. */
const typeAccent: Record<string, string> = {
  AssignmentPublished: 'border-l-acid-deep',
  SubmissionReceived: 'border-l-ink',
  AssessmentPublished: 'border-l-moss',
};

export function NotificationBell({ homeRoute }: { homeRoute: string }) {
  const [open, setOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  const unread = useUnreadCount();
  const notifications = useNotifications(open);
  const markRead = useMarkNotificationRead();
  const markAll = useMarkAllNotificationsRead();

  // Close on outside click and on Escape - a dropdown that traps you is worse
  // than no dropdown.
  useEffect(() => {
    if (!open) return;

    function onPointerDown(event: MouseEvent) {
      if (!containerRef.current?.contains(event.target as Node)) {
        setOpen(false);
      }
    }

    function onKeyDown(event: KeyboardEvent) {
      if (event.key === 'Escape') setOpen(false);
    }

    document.addEventListener('mousedown', onPointerDown);
    document.addEventListener('keydown', onKeyDown);

    return () => {
      document.removeEventListener('mousedown', onPointerDown);
      document.removeEventListener('keydown', onKeyDown);
    };
  }, [open]);

  const count = unread.data ?? 0;
  const items = notifications.data ?? [];

  return (
    <div className="relative" ref={containerRef}>
      <button
        type="button"
        onClick={() => setOpen((value) => !value)}
        aria-label={count > 0 ? `Notifications, ${count} unread` : 'Notifications'}
        aria-expanded={open}
        className={cx(
          'relative rounded-lg border p-2 transition-colors',
          open
            ? 'border-paper/30 bg-white/10 text-paper'
            : 'border-transparent text-paper/70 hover:bg-white/10 hover:text-paper',
        )}
      >
        <Bell className="h-[18px] w-[18px]" />

        {count > 0 && (
          <span
            className={cx(
              'absolute -right-1 -top-1 flex h-[18px] min-w-[18px] items-center',
              'justify-center rounded-full bg-acid px-1 font-mono text-[10px]',
              'font-medium text-ink ring-2 ring-ink',
            )}
          >
            {count > 9 ? '9+' : count}
          </span>
        )}
      </button>

      {open && (
        <div
          className={cx(
            'absolute right-0 z-50 mt-2 w-[min(22rem,calc(100vw-2rem))]',
            'animate-rise overflow-hidden rounded-card border border-ink/10',
            'bg-paper shadow-lift',
          )}
        >
          <div className="flex items-center justify-between border-b border-rule px-4 py-3">
            <div>
              <p className="ledger-index">Activity</p>
              <p className="font-display text-base font-semibold text-ink">
                Notifications
              </p>
            </div>

            {count > 0 && (
              <button
                type="button"
                onClick={() => markAll.mutate()}
                className="inline-flex items-center gap-1.5 text-xs text-body-muted transition-colors hover:text-ink"
              >
                <CheckCheck className="h-3.5 w-3.5" />
                Mark all read
              </button>
            )}
          </div>

          <div className="max-h-[26rem] overflow-y-auto">
            {notifications.isLoading && (
              <p className="px-4 py-8 text-center text-sm text-body-muted">
                Loading
              </p>
            )}

            {notifications.isSuccess && items.length === 0 && (
              <p className="px-4 py-10 text-center text-sm text-body-muted">
                Nothing yet. New assignments, submissions and grades land here.
              </p>
            )}

            <ul className="divide-y divide-rule">
              {items.slice(0, 8).map((item) => (
                <Row
                  key={item.notificationId}
                  item={item}
                  onRead={() => markRead.mutate(item.notificationId)}
                />
              ))}
            </ul>
          </div>

          <Link
            to={`${homeRoute}/notifications`}
            onClick={() => setOpen(false)}
            className="block border-t border-rule px-4 py-3 text-center text-sm text-ink transition-colors hover:bg-paper-warm"
          >
            View all activity
          </Link>
        </div>
      )}
    </div>
  );
}

function Row({ item, onRead }: { item: NotificationItem; onRead: () => void }) {
  return (
    <li
      className={cx(
        'border-l-2 px-4 py-3 transition-colors',
        typeAccent[item.notificationType] ?? 'border-l-rule',
        item.isRead ? 'bg-transparent' : 'bg-acid/[0.07]',
      )}
    >
      <div className="flex items-start justify-between gap-2">
        <p className="text-sm font-medium text-ink">{item.title}</p>

        {!item.isRead && (
          <button
            type="button"
            onClick={onRead}
            className="mt-0.5 shrink-0 rounded px-1.5 py-0.5 font-mono text-[10px] uppercase tracking-wide text-body-faint transition-colors hover:bg-paper-warm hover:text-ink"
          >
            Mark read
          </button>
        )}
      </div>

      <p className="mt-1 text-sm leading-relaxed text-body-muted">{item.message}</p>

      <p
        className="mt-1.5 font-mono text-[11px] text-body-faint"
        title={formatDateTime(item.createdAt)}
      >
        {timeAgo(item.createdAt)} · {formatDateTime(item.createdAt)}
      </p>
    </li>
  );
}

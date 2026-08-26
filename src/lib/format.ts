import type { Assignment } from './types';

const dateTime = new Intl.DateTimeFormat('en-GB', {
  day: '2-digit',
  month: 'short',
  year: 'numeric',
  hour: '2-digit',
  minute: '2-digit',
});

const dateOnly = new Intl.DateTimeFormat('en-GB', {
  day: '2-digit',
  month: 'short',
  year: 'numeric',
});

export const formatDateTime = (value?: string | null) =>
  value ? dateTime.format(new Date(value)) : '--';

export const formatDate = (value?: string | null) =>
  value ? dateOnly.format(new Date(value)) : '--';

/**
 * The API stores every timestamp as `timestamp without time zone` and Npgsql
 * rejects a DateTime whose Kind is Utc. `new Date().toISOString()` always ends
 * in Z, so payloads are built from the raw <input type="datetime-local"> value
 * instead, which has no zone designator.
 */
export const toLocalInputValue = (value?: string | null) => {
  if (!value) return '';

  const date = new Date(value);
  const pad = (part: number) => String(part).padStart(2, '0');

  return (
    `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}` +
    `T${pad(date.getHours())}:${pad(date.getMinutes())}`
  );
};

export const formatBytes = (bytes?: number | null) => {
  if (!bytes && bytes !== 0) return '--';
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(0)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
};

export type DeadlineState = 'open' | 'soon' | 'over' | 'idle';

export interface DeadlineInfo {
  state: DeadlineState;
  label: string;
}

/**
 * Turns a due date into the value the deadline rail is coloured by.
 * "soon" is anything inside 48 hours, which is when students actually act.
 */
export function deadlineInfo(assignment: Assignment): DeadlineInfo {
  if (assignment.assignmentStatus !== 'Published') {
    return { state: 'idle', label: assignment.assignmentStatus };
  }

  const now = Date.now();
  const due = new Date(assignment.dueDate).getTime();
  const diffHours = (due - now) / 36e5;

  if (diffHours < 0) {
    const lateDeadline = assignment.lateSubmissionDeadline
      ? new Date(assignment.lateSubmissionDeadline).getTime()
      : null;

    if (assignment.allowLateSubmission && lateDeadline && lateDeadline > now) {
      const lateHours = Math.round((lateDeadline - now) / 36e5);
      return {
        state: 'over',
        label:
          lateHours < 24
            ? `Late window, ${lateHours}h left`
            : `Late window, ${Math.round(lateHours / 24)}d left`,
      };
    }

    const overdueDays = Math.floor(-diffHours / 24);
    return {
      state: 'over',
      label: overdueDays < 1 ? 'Closed today' : `Closed ${overdueDays}d ago`,
    };
  }

  if (diffHours < 48) {
    return { state: 'soon', label: `Due in ${Math.max(1, Math.round(diffHours))}h` };
  }

  return { state: 'open', label: `Due in ${Math.round(diffHours / 24)}d` };
}

export const cx = (...classes: (string | false | null | undefined)[]) =>
  classes.filter(Boolean).join(' ');

/** "3 minutes ago", "2 days ago" - used in the notification feed. */
export function timeAgo(value: string): string {
  const then = new Date(value).getTime();
  const seconds = Math.round((Date.now() - then) / 1000);

  if (seconds < 60) return 'just now';

  const minutes = Math.round(seconds / 60);
  if (minutes < 60) return `${minutes} min ago`;

  const hours = Math.round(minutes / 60);
  if (hours < 24) return `${hours}h ago`;

  const days = Math.round(hours / 24);
  if (days < 30) return `${days}d ago`;

  return formatDate(value);
}

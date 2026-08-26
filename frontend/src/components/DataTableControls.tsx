import type { ReactNode } from 'react';
import { ArrowUpDown, ChevronLeft, ChevronRight, ChevronDown, ChevronUp, Search, X } from 'lucide-react';
import { cx } from '../lib/format';
import { Button } from './ui';

export function SearchInput({
  value,
  onChange,
  placeholder = 'Search...',
  className,
}: {
  value: string;
  onChange: (val: string) => void;
  placeholder?: string;
  className?: string;
}) {
  return (
    <div className={cx('relative min-w-[200px] flex-1 sm:max-w-xs', className)}>
      <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-body-muted pointer-events-none" />
      <input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
        className="w-full rounded-lg border border-rule bg-white pl-9 pr-8 py-1.5 text-sm text-ink placeholder:text-body-faint transition-colors focus:border-ink focus:outline-none focus:ring-4 focus:ring-acid/40"
      />
      {value && (
        <button
          type="button"
          onClick={() => onChange('')}
          className="absolute right-2.5 top-1/2 -translate-y-1/2 text-body-faint hover:text-ink rounded p-0.5"
          aria-label="Clear search"
        >
          <X className="h-3.5 w-3.5" />
        </button>
      )}
    </div>
  );
}

export function SortTh({
  label,
  sortKey,
  currentSortKey,
  currentSortDirection,
  onSort,
  className,
}: {
  label: ReactNode;
  sortKey: string;
  currentSortKey: string | null;
  currentSortDirection: 'asc' | 'desc';
  onSort: (key: string) => void;
  className?: string;
}) {
  const isActive = currentSortKey === sortKey;

  return (
    <th
      className={cx(
        'px-4 py-3 font-mono text-[11px] font-medium uppercase tracking-[0.14em] text-body-muted select-none cursor-pointer hover:text-ink transition-colors',
        className,
      )}
      onClick={() => onSort(sortKey)}
    >
      <div className="flex items-center gap-1.5">
        <span>{label}</span>
        {isActive ? (
          currentSortDirection === 'asc' ? (
            <ChevronUp className="h-3.5 w-3.5 text-ink" />
          ) : (
            <ChevronDown className="h-3.5 w-3.5 text-ink" />
          )
        ) : (
          <ArrowUpDown className="h-3 w-3 text-body-faint opacity-60 hover:opacity-100" />
        )}
      </div>
    </th>
  );
}

export function PaginationBar({
  currentPage,
  totalPages,
  totalItems,
  startIndex,
  endIndex,
  onPageChange,
  pageSize,
  onPageSizeChange,
}: {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  startIndex: number;
  endIndex: number;
  onPageChange: (page: number) => void;
  pageSize: number;
  onPageSizeChange?: (size: number) => void;
}) {
  if (totalItems === 0) return null;

  const pages: number[] = [];
  const maxButtons = 5;
  let startPage = Math.max(1, currentPage - Math.floor(maxButtons / 2));
  let endPage = startPage + maxButtons - 1;

  if (endPage > totalPages) {
    endPage = totalPages;
    startPage = Math.max(1, endPage - maxButtons + 1);
  }

  for (let i = startPage; i <= endPage; i++) {
    pages.push(i);
  }

  return (
    <div className="flex flex-wrap items-center justify-between gap-4 border-t border-rule px-4 py-3 text-xs">
      <div className="flex flex-wrap items-center gap-3 text-body-muted font-mono">
        <span>
          Showing <span className="font-semibold text-ink">{startIndex}</span>–
          <span className="font-semibold text-ink">{endIndex}</span> of{' '}
          <span className="font-semibold text-ink">{totalItems}</span> items
        </span>

        {onPageSizeChange && (
          <div className="flex items-center gap-1.5 border-l border-rule pl-3">
            <span className="text-body-muted font-mono">Per page:</span>
            <select
              value={pageSize}
              onChange={(e) => onPageSizeChange(Number(e.target.value))}
              className="h-7 min-w-[3.75rem] rounded-md border border-rule bg-white px-2 py-0.5 font-mono text-xs font-medium text-ink transition-colors focus:border-ink focus:outline-none focus:ring-2 focus:ring-acid/40 cursor-pointer"
            >
              <option value={5}>5</option>
              <option value={10}>10</option>
              <option value={25}>25</option>
              <option value={50}>50</option>
            </select>
          </div>
        )}
      </div>

      <div className="flex items-center gap-1">
        <Button
          variant="secondary"
          size="sm"
          disabled={currentPage <= 1}
          onClick={() => onPageChange(currentPage - 1)}
          className="px-2 py-1 h-7 text-xs"
        >
          <ChevronLeft className="h-3.5 w-3.5" />
          <span className="sr-only sm:not-sr-only">Prev</span>
        </Button>

        {pages.map((p) => (
          <button
            key={p}
            type="button"
            onClick={() => onPageChange(p)}
            className={cx(
              'h-7 w-7 rounded-md font-mono text-xs font-medium transition-colors',
              p === currentPage
                ? 'bg-ink text-paper font-bold'
                : 'border border-rule bg-white text-body-muted hover:bg-paper-warm hover:text-ink',
            )}
          >
            {p}
          </button>
        ))}

        <Button
          variant="secondary"
          size="sm"
          disabled={currentPage >= totalPages}
          onClick={() => onPageChange(currentPage + 1)}
          className="px-2 py-1 h-7 text-xs"
        >
          <span className="sr-only sm:not-sr-only">Next</span>
          <ChevronRight className="h-3.5 w-3.5" />
        </Button>
      </div>
    </div>
  );
}

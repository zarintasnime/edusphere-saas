import { useMemo, useState } from 'react';

export interface UseDataTableOptions<T> {
  data: T[];
  searchFields?: ((item: T) => string)[];
  initialSortField?: (item: T) => any;
  initialSortDirection?: 'asc' | 'desc';
  pageSize?: number;
}

export function useDataTable<T>({
  data,
  searchFields = [],
  initialSortField,
  initialSortDirection = 'asc',
  pageSize: initialPageSize = 10,
}: UseDataTableOptions<T>) {
  const [searchQuery, setSearchQuery] = useState('');
  const [sortKey, setSortKey] = useState<string | null>(null);
  const [sortGetter, setSortGetter] = useState<((item: T) => any) | null>(() => initialSortField ?? null);
  const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>(initialSortDirection);
  const [filterValue, setFilterValue] = useState<string>('all');
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);

  const handleSearchChange = (query: string) => {
    setSearchQuery(query);
    setCurrentPage(1);
  };

  const handleFilterChange = (filter: string) => {
    setFilterValue(filter);
    setCurrentPage(1);
  };

  const toggleSort = (key: string, getter: (item: T) => any) => {
    if (sortKey === key) {
      if (sortDirection === 'asc') {
        setSortDirection('desc');
      } else {
        setSortKey(null);
        setSortGetter(null);
        setSortDirection('asc');
      }
    } else {
      setSortKey(key);
      setSortGetter(() => getter);
      setSortDirection('asc');
    }
    setCurrentPage(1);
  };

  const filteredAndSortedItems = useMemo(() => {
    let result = [...data];

    // Search filtering
    if (searchQuery.trim()) {
      const q = searchQuery.toLowerCase().trim();
      result = result.filter((item) => {
        if (searchFields.length === 0) {
          // Fallback search across all string/number properties
          return Object.values(item as any).some((val) =>
            String(val ?? '').toLowerCase().includes(q),
          );
        }
        return searchFields.some((fieldFn) => fieldFn(item).toLowerCase().includes(q));
      });
    }

    // Sorting
    if (sortGetter) {
      result.sort((a, b) => {
        const valA = sortGetter(a);
        const valB = sortGetter(b);

        if (valA == null && valB == null) return 0;
        if (valA == null) return 1;
        if (valB == null) return -1;

        if (typeof valA === 'number' && typeof valB === 'number') {
          return sortDirection === 'asc' ? valA - valB : valB - valA;
        }

        const strA = String(valA).toLowerCase();
        const strB = String(valB).toLowerCase();
        const comparison = strA.localeCompare(strB);
        return sortDirection === 'asc' ? comparison : -comparison;
      });
    }

    return result;
  }, [data, searchQuery, searchFields, sortGetter, sortDirection]);

  const totalItems = filteredAndSortedItems.length;
  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));
  const safeCurrentPage = Math.min(currentPage, totalPages);

  const startIndex = totalItems === 0 ? 0 : (safeCurrentPage - 1) * pageSize + 1;
  const endIndex = Math.min(totalItems, safeCurrentPage * pageSize);

  const paginatedItems = useMemo(() => {
    const start = (safeCurrentPage - 1) * pageSize;
    return filteredAndSortedItems.slice(start, start + pageSize);
  }, [filteredAndSortedItems, safeCurrentPage, pageSize]);

  return {
    searchQuery,
    setSearchQuery: handleSearchChange,
    filterValue,
    setFilterValue: handleFilterChange,
    sortKey,
    sortDirection,
    toggleSort,
    currentPage: safeCurrentPage,
    setCurrentPage,
    pageSize,
    setPageSize: (size: number) => {
      setPageSize(size);
      setCurrentPage(1);
    },
    totalPages,
    totalItems,
    startIndex,
    endIndex,
    filteredItems: filteredAndSortedItems,
    paginatedItems,
  };
}

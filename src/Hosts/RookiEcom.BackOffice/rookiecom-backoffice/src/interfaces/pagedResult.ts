export interface PagedResult<T> {
  items: T[];
  pageData: PageData;
}

export interface PageData {
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}
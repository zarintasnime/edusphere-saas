import axios, { AxiosError } from 'axios';

export const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5258';

export const TOKEN_KEY = 'campusflow.token';

export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_KEY);

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

/**
 * The API always answers errors with { message, statusCode, errors? }
 * (see ExceptionHandlingMiddleware), so one place can turn any failure into a
 * sentence the UI can show.
 */
export interface ApiErrorBody {
  message?: string;
  statusCode?: number;
  errors?: Record<string, string[]>;
}

export function errorMessage(error: unknown): string {
  const axiosError = error as AxiosError<ApiErrorBody>;

  if (axiosError?.response) {
    const body = axiosError.response.data;

    if (body?.errors) {
      const first = Object.values(body.errors)[0];
      if (first?.length) return first[0];
    }

    if (body?.message) return body.message;

    if (axiosError.response.status === 403) {
      return 'Your role does not have access to this action.';
    }

    return `Request failed with status ${axiosError.response.status}.`;
  }

  if (axiosError?.request) {
    return `Cannot reach the API at ${API_BASE_URL}. Is it running?`;
  }

  return 'Something went wrong.';
}

/** Set by AuthContext so a 401 anywhere can clear the session once. */
let onUnauthorized: (() => void) | null = null;

export function setUnauthorizedHandler(handler: () => void) {
  onUnauthorized = handler;
}

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    // 401 means the token is missing or expired. 403 is a valid answer for a
    // signed-in user hitting the wrong role, so it must not log anyone out.
    if (error.response?.status === 401) {
      onUnauthorized?.();
    }

    return Promise.reject(error);
  },
);

/** Absolute URL for a file the API stored under wwwroot. */
export function fileUrl(filePath: string): string {
  if (/^https?:\/\//i.test(filePath)) return filePath;

  const normalized = filePath.replace(/\\/g, '/').replace(/^\.?\//, '');

  return `${API_BASE_URL}/${normalized}`;
}

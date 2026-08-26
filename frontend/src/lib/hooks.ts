import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';

import { api } from './api';
import { useAuth } from '../auth/AuthContext';
import type {
  AcademicYear,
  Assignment,
  AuditLogItem,
  Batch,
  Course,
  CourseSubject,
  Department,
  StudentEnrollment,
  NotificationItem,
  StudentProfile,
  Subject,
  TeacherProfile,
  TeacherSubject,
} from './types';

/**
 * The JWT carries the user id, not the teacher/student profile id, but almost
 * every teacher and student endpoint is keyed by the profile id. Both hooks
 * resolve it once and every page reuses the cached result.
 */
export function useTeacherProfile() {
  const { user } = useAuth();

  return useQuery({
    queryKey: ['teacher-profile', user?.userId],
    enabled: Boolean(user && user.role === 'Teacher'),
    queryFn: async () => {
      const { data } = await api.get<TeacherProfile>(
        `/api/Teacher/user/${user!.userId}`,
      );
      return data;
    },
  });
}

export function useStudentProfile() {
  const { user } = useAuth();

  return useQuery({
    queryKey: ['student-profile', user?.userId],
    enabled: Boolean(user && user.role === 'Student'),
    queryFn: async () => {
      const { data } = await api.get<StudentProfile>('/api/Student/my-profile');
      return data;
    },
  });
}

/** Enrolments belonging to the signed-in student, newest active first. */
export function useMyEnrollments(enabled = true) {
  return useQuery({
    queryKey: ['my-enrollments'],
    enabled,
    queryFn: async () => {
      const { data } = await api.get<StudentEnrollment[]>(
        '/api/StudentEnrollment/my',
      );
      return data;
    },
  });
}

export function useMyAssignments(enabled = true) {
  return useQuery({
    queryKey: ['assignments', 'mine'],
    enabled,
    queryFn: async () => {
      const { data } = await api.get<Assignment[]>('/api/Assignment/my');
      return data;
    },
  });
}

export function useTeacherSubjects(teacherId?: number) {
  return useQuery({
    queryKey: ['teacher-subjects', teacherId],
    enabled: Boolean(teacherId),
    queryFn: async () => {
      const { data } = await api.get<TeacherSubject[]>(
        `/api/TeacherSubject/teacher/${teacherId}`,
      );
      return data;
    },
  });
}

export function useAcademicYears(enabled = true) {
  return useQuery({
    queryKey: ['academic-years'],
    enabled,
    queryFn: async () => {
      const { data } = await api.get<AcademicYear[]>(
        '/api/Academic/academic-years',
      );
      return data;
    },
  });
}

/* --- admin reference data ------------------------------------------------- */

export const useDepartments = () =>
  useQuery({
    queryKey: ['departments'],
    queryFn: async () => {
      const { data } = await api.get<Department[]>('/api/Academic/departments');
      return data;
    },
  });

export const useCourses = () =>
  useQuery({
    queryKey: ['courses'],
    queryFn: async () => {
      const { data } = await api.get<Course[]>('/api/Academic/courses');
      return data;
    },
  });

export const useSubjects = () =>
  useQuery({
    queryKey: ['subjects'],
    queryFn: async () => {
      const { data } = await api.get<Subject[]>('/api/Academic/subjects');
      return data;
    },
  });

export const useBatches = () =>
  useQuery({
    queryKey: ['batches'],
    queryFn: async () => {
      const { data } = await api.get<Batch[]>('/api/Academic/batches');
      return data;
    },
  });

export const useCourseSubjects = () =>
  useQuery({
    queryKey: ['course-subjects'],
    queryFn: async () => {
      const { data } = await api.get<CourseSubject[]>('/api/CourseSubject');
      return data;
    },
  });

export const useTeachers = () =>
  useQuery({
    queryKey: ['teachers'],
    queryFn: async () => {
      const { data } = await api.get<TeacherProfile[]>('/api/Teacher');
      return data;
    },
  });

export const useStudents = () =>
  useQuery({
    queryKey: ['students'],
    queryFn: async () => {
      const { data } = await api.get<StudentProfile[]>('/api/Student');
      return data;
    },
  });


/* --- notifications ---------------------------------------------------------

   The bell polls the count on a short interval rather than opening a socket:
   SignalR would be the right answer for a real deployment, but it is a lot of
   moving parts for a demo, and a 30s poll is honest about what it is.        */

export function useUnreadCount(enabled = true) {
  return useQuery({
    queryKey: ['notifications', 'unread-count'],
    enabled,
    refetchInterval: 30_000,
    staleTime: 15_000,
    queryFn: async () => {
      const { data } = await api.get<{ unreadCount: number }>(
        '/api/Notification/my/unread-count',
      );
      return data.unreadCount;
    },
  });
}

export function useNotifications(enabled = true) {
  return useQuery({
    queryKey: ['notifications', 'list'],
    enabled,
    queryFn: async () => {
      const { data } = await api.get<NotificationItem[]>('/api/Notification/my');
      return data;
    },
  });
}

export function useMarkNotificationRead() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (notificationId: number) => {
      await api.patch(`/api/Notification/${notificationId}/read`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
    },
  });
}

export function useMarkAllNotificationsRead() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async () => {
      await api.patch('/api/Notification/my/read-all');
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
    },
  });
}

export const useAuditLogs = () =>
  useQuery({
    queryKey: ['audit-logs'],
    queryFn: async () => {
      const { data } = await api.get<AuditLogItem[]>('/api/AuditLog');
      return data;
    },
  });

import { Navigate, Route, Routes } from 'react-router-dom';

import { homeRouteFor, useAuth } from './auth/AuthContext';
import { ProtectedRoute } from './auth/ProtectedRoute';
import { AppShell, type NavItem } from './components/AppShell';

import LandingPage from './pages/LandingPage';
import LoginPage from './pages/LoginPage';
import NotificationsPage from './pages/NotificationsPage';

import AdminOverview from './pages/admin/AdminOverview';
import AcademicPage from './pages/admin/AcademicPage';
import PeoplePage from './pages/admin/PeoplePage';
import TeachingPage from './pages/admin/TeachingPage';

import TeacherOverview from './pages/teacher/TeacherOverview';
import TeacherAssignmentsPage from './pages/teacher/TeacherAssignmentsPage';
import AssignmentDetailPage from './pages/teacher/AssignmentDetailPage';

import StudentOverview from './pages/student/StudentOverview';
import StudentAssignmentsPage from './pages/student/StudentAssignmentsPage';
import StudentSubmissionsPage from './pages/student/StudentSubmissionsPage';

const adminNav: NavItem[] = [
  { to: '/admin', label: 'Overview', end: true },
  { to: '/admin/people', label: 'People' },
  { to: '/admin/academic', label: 'Academic structure' },
  { to: '/admin/teaching', label: 'Teaching & enrolment' },
  { to: '/admin/notifications', label: 'Activity' },
];

const teacherNav: NavItem[] = [
  { to: '/teacher', label: 'Overview', end: true },
  { to: '/teacher/assignments', label: 'Assignments' },
  { to: '/teacher/notifications', label: 'Activity' },
];

const studentNav: NavItem[] = [
  { to: '/student', label: 'Overview', end: true },
  { to: '/student/assignments', label: 'Assignments' },
  { to: '/student/submissions', label: 'My submissions' },
  { to: '/student/notifications', label: 'Activity' },
];

export default function App() {
  const { user } = useAuth();

  return (
    <Routes>
      <Route path="/" element={<LandingPage />} />
      <Route path="/login" element={<LoginPage />} />

      <Route
        path="/admin"
        element={
          <ProtectedRoute allow={['SuperAdmin', 'Admin']}>
            <AppShell nav={adminNav} homeRoute="/admin" />
          </ProtectedRoute>
        }
      >
        <Route index element={<AdminOverview />} />
        <Route path="people" element={<PeoplePage />} />
        <Route path="academic" element={<AcademicPage />} />
        <Route path="teaching" element={<TeachingPage />} />
        <Route path="notifications" element={<NotificationsPage />} />
      </Route>

      <Route
        path="/teacher"
        element={
          <ProtectedRoute allow={['Teacher']}>
            <AppShell nav={teacherNav} homeRoute="/teacher" />
          </ProtectedRoute>
        }
      >
        <Route index element={<TeacherOverview />} />
        <Route path="assignments" element={<TeacherAssignmentsPage />} />
        <Route path="assignments/:assignmentId" element={<AssignmentDetailPage />} />
        <Route path="notifications" element={<NotificationsPage />} />
      </Route>

      <Route
        path="/student"
        element={
          <ProtectedRoute allow={['Student']}>
            <AppShell nav={studentNav} homeRoute="/student" />
          </ProtectedRoute>
        }
      >
        <Route index element={<StudentOverview />} />
        <Route path="assignments" element={<StudentAssignmentsPage />} />
        <Route path="submissions" element={<StudentSubmissionsPage />} />
        <Route path="notifications" element={<NotificationsPage />} />
      </Route>

      <Route
        path="*"
        element={<Navigate to={user ? homeRouteFor(user.role) : '/'} replace />}
      />
    </Routes>
  );
}

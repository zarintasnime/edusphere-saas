/**
 * Mirrors the API DTOs. Enums arrive as strings because the API registers a
 * JsonStringEnumConverter, so these unions can be compared directly.
 */

export type RoleType = 'SuperAdmin' | 'Admin' | 'Teacher' | 'Student';

export type AssignmentStatus = 'Draft' | 'Published' | 'Closed' | 'Archived';

export type SubmissionStatus = 'Submitted' | 'UnderReview' | 'Reviewed' | 'Returned';

export interface AuthResponse {
  userId: number;
  institutionId: number | null;
  fullName: string;
  email: string;
  role: RoleType;
  token: string;
  expiresAt: string;
}

export interface UserResponse {
  userId: number;
  institutionId: number | null;
  roleId: number;
  role: RoleType;
  fullName: string;
  email: string;
  phoneNumber?: string | null;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string | null;
}

export interface TeacherProfile {
  teacherId: number;
  institutionId: number;
  institutionName: string;
  userId: number;
  teacherName: string;
  email: string;
  departmentId: number;
  departmentName: string;
  employeeCode: string;
  qualification?: string | null;
  joiningDate?: string | null;
  isActive: boolean;
  createdAt: string;
}

export interface StudentProfile {
  studentId: number;
  institutionId: number;
  institutionName: string;
  userId: number;
  studentName: string;
  email: string;
  studentCode: string;
  admissionDate?: string | null;
  isActive: boolean;
  createdAt: string;
}

export interface Department {
  departmentId: number;
  institutionId: number;
  departmentCode: string;
  departmentName: string;
  description?: string | null;
  isActive: boolean;
}

export interface Course {
  courseId: number;
  institutionId: number;
  departmentId: number;
  departmentName: string;
  courseCode: string;
  courseName: string;
  isActive: boolean;
}

export interface Subject {
  subjectId: number;
  institutionId: number;
  subjectCode: string;
  subjectName: string;
  isActive: boolean;
}

export interface Batch {
  batchId: number;
  institutionId: number;
  courseId: number;
  courseCode: string;
  courseName: string;
  batchCode: string;
  batchName: string;
  startYear: number;
  endYear?: number | null;
  isActive: boolean;
}

export interface AcademicYear {
  academicYearId: number;
  institutionId: number;
  batchId: number;
  batchCode: string;
  batchName: string;
  yearName: string;
  yearOrder: number;
  isActive: boolean;
}

export interface CourseSubject {
  courseSubjectId: number;
  institutionId: number;
  courseId: number;
  courseCode: string;
  courseName: string;
  subjectId: number;
  subjectCode: string;
  subjectName: string;
}

export interface TeacherSubject {
  teacherSubjectId: number;
  institutionId: number;
  teacherId: number;
  teacherName: string;
  courseSubjectId: number;
  courseId: number;
  courseName: string;
  subjectId: number;
  subjectName: string;
}

export interface StudentEnrollment {
  enrollmentId: number;
  institutionId: number;
  studentId: number;
  studentCode: string;
  studentName: string;
  academicYearId: number;
  academicYearName: string;
  rollNumber: string;
  enrollmentDate?: string | null;
  isActive: boolean;
}

export interface Assignment {
  assignmentId: number;
  institutionId: number;
  teacherId: number;
  teacherName: string;
  courseSubjectId: number;
  courseName: string;
  subjectName: string;
  teacherSubjectId: number;
  academicYearId: number;
  academicYearName: string;
  title: string;
  description?: string | null;
  totalMarks: number;
  dueDate: string;
  allowLateSubmission: boolean;
  lateSubmissionDeadline?: string | null;
  assignmentStatus: AssignmentStatus;
  isActive: boolean;
  createdAt: string;
}

export interface SubmissionAttachment {
  attachmentId: number;
  submissionId: number;
  fileName: string;
  filePath: string;
  fileType?: string | null;
  fileSize?: number | null;
  createdAt: string;
}

export interface Submission {
  submissionId: number;
  institutionId: number;
  assignmentId: number;
  assignmentTitle: string;
  studentId: number;
  studentCode: string;
  studentName: string;
  submissionVersion: number;
  submissionText?: string | null;
  submittedAt: string;
  isLateSubmission: boolean;
  isLatestSubmission: boolean;
  submissionStatus: SubmissionStatus;
  createdAt: string;
  attachments: SubmissionAttachment[];
}

export interface Assessment {
  assessmentId: number;
  submissionId: number;
  teacherId: number;
  teacherName: string;
  policyId?: number | null;
  policyName?: string | null;
  marksObtained: number;
  penaltyPercentageApplied: number;
  finalMarks: number;
  feedback?: string | null;
  reviewedAt: string;
}

export type NotificationType =
  | 'AssignmentPublished'
  | 'SubmissionReceived'
  | 'AssessmentPublished'
  | string;

export interface NotificationItem {
  notificationId: number;
  institutionId: number | null;
  userId: number;
  title: string;
  message: string;
  notificationType: NotificationType;
  channel: string;
  referenceId?: number | null;
  isRead: boolean;
  readAt?: string | null;
  createdAt: string;
}

export interface AuditLogItem {
  auditLogId: number;
  institutionId?: number | null;
  userId: number;
  userName: string;
  action: string;
  entityName: string;
  entityId: number;
  oldValues?: string | null;
  newValues?: string | null;
  createdAt: string;
}

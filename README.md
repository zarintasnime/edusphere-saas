# 🎓 CampusFlow (EduSphere)

> **Modern Academic Assignment & Submission Management System**  
> *A full-stack, enterprise-grade academic workflow platform built with ASP.NET Core 8 Web API and React 18 + TypeScript.*

[![React](https://img.shields.io/badge/Frontend-React_18-61DAFB?logo=react&logoColor=black)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/Language-TypeScript_5-3178C6?logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![Vite](https://img.shields.io/badge/Bundler-Vite_5-646CFF?logo=vite&logoColor=white)](https://vitejs.dev/)
[![Tailwind CSS](https://img.shields.io/badge/Styling-Tailwind_CSS_3-06B6D4?logo=tailwindcss&logoColor=white)](https://tailwindcss.com/)
[![.NET 8](https://img.shields.io/badge/Backend-.NET_8_Web_API-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL_16-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 📌 Overview

**CampusFlow** streamlines the entire academic lifecycle for higher education institutions through a clean, layered architecture and intuitive role-based user interfaces:

- **Administrators**: Establish academic structure (Departments, Courses, Subjects, Batches, Academic Years), assign staff, onboard students, and track live system audit logs.
- **Teachers**: Compose, schedule, publish, and evaluate assignments with configurable total marks, due dates, and late submission windows.
- **Students**: Monitor approaching deadlines via dynamic visual rails, submit work with notes and file attachments, and track graded feedback with late penalty breakdowns.

The application features an editorial dark ink design aesthetic, role-based JWT authentication, in-app notification dispatching, real-time activity tracking, and client-side avatar customization.

---

## 🔐 Demo Accounts

The database automatically migrates and seeds starter data on first application launch. All demo accounts share the standard password: `Demo@123`.

| Role | Email Address | Primary Access & Responsibilities |
| :--- | :--- | :--- |
| **Super Admin** | `superadmin@campusflow.dev` | Global platform administration, institution setup, system-wide oversight. |
| **Administrator** | `admin@campusflow.dev` | Academic structure setup, department management, teacher & student onboarding, audit logs. |
| **Teacher** | `teacher@campusflow.dev` | Subject management, assignment creation & publishing, student submission review & grading. |
| **Student** | `student@campusflow.dev` | Enrolled course assignment hub, homework & attachment submission, mark & feedback viewing. |

> 💡 *The automated database seeder populates starter institutions, departments, courses, subjects, academic years, active enrolments, teacher subject mappings, and starter assignments.*

---

## ✨ Key Features

### 🎓 Student Portal
- **Dynamic Deadline Rails:** Color-coded urgency indicators (green → amber → vermillion) that shift automatically as deadlines approach.
- **Flexible Submissions:** Submit assignments using rich text notes, multi-file attachments (up to 20 MB), or both.
- **Grade & Feedback Tracking:** Instant access to assigned scores, written teacher feedback, and late penalty calculations.

### 👩‍🏫 Teacher Workspace
- **Assignment Composer:** Draft, schedule, and publish assignments with total marks, due dates, and optional late submission grace periods.
- **Grading Queue:** Filter submissions by status (graded/ungraded), review student attachments, record final scores, and return commentary.
- **Scoped Subject Authorization:** Automatic authorization safeguards ensuring faculty can only manage work for their assigned courses.

### 🛡️ Admin & Operational Tools
- **Academic Hierarchy Management:** Setup Institution → Departments → Courses → Subjects → Batches → Academic Years.
- **User Onboarding:** Manage teacher and student accounts tied to specific departments and academic codes.
- **Audit Logging & Activity Feeds:** Live system event records tracking administrative actions and platform events.

### 🔔 Notification System
- **Event-Driven Alerts:** Automated notifications triggered by assignment publishing, student submissions (with late tags), and grade releases.
- **Header Bell Component:** Interactive popover with unread counter and one-click "Mark All Read".

---

## 🛠️ Tech Stack & Architecture

```
CampusFlow/
├── backend/                                            # ASP.NET Core 8 Web API
│   ├── AssignmentSubmissionManagementSystem.Domain/        # Core entities, value objects & enums
│   ├── AssignmentSubmissionManagementSystem.Application/   # DTOs, interfaces, FluentValidators & services
│   ├── AssignmentSubmissionManagementSystem.Infrastructure/# EF Core DbContext, PostgreSQL mappings & seeder
│   ├── AssignmentSubmissionManagementSystem.API/           # REST Controllers, JWT authentication & middleware
│   └── AssignmentSubmissionManagementSystem.Tests/         # xUnit unit test suite
└── frontend/                                           # React 18 + TypeScript Client
    └── src/
        ├── auth/                                       # AuthContext, JWT handling & RoleGuard
        ├── components/                                 # AppShell, UI components, Avatar & Notifications
        ├── lib/                                        # Axios API client, TanStack Query hooks & utilities
        └── pages/                                      # Admin, Teacher, and Student portal views
```

### Technical Highlights
- **Clean Architecture:** Strict inward dependency flow (`API` → `Infrastructure` / `Application` → `Domain`).
- **Strict Role-Based Security (RBAC):** Combined `[Authorize(Roles = ...)]` attributes with domain-level record ownership verification across all API endpoints.
- **JSON String Enum Handling:** `JsonStringEnumConverter` ensures API enums serialize cleanly as readable string literals (`"Teacher"`, `"Published"`).
- **Validation Pipeline:** Integrated `FluentValidation` with custom `ValidationFilter` returning structured `422 Unprocessable Entity` responses.

---

## 🚀 Local Setup & Execution

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 14+](https://www.postgresql.org/download/)

---

### 1️⃣ Database Configuration

1. Create a local PostgreSQL database:
   ```bash
   createdb campusflow
   ```

2. Navigate to the API directory:
   ```bash
   cd backend/AssignmentSubmissionManagementSystem.API
   ```

3. Set local secrets for your database connection string and JWT signing key (minimum 32 characters):
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=campusflow;Username=postgres;Password=yourpassword"
   dotnet user-secrets set "Jwt:Key" "super-secret-jwt-key-with-at-least-32-characters!"
   ```
   > 📝 *Alternatively, configure `ConnectionStrings__DefaultConnection` and `Jwt__Key` in environment variables or `appsettings.json`.*

---

### 2️⃣ Running the Backend (.NET Web API)

From the `backend` directory:

```bash
# Restore dependencies and build solution
dotnet build

# Launch API using http profile
dotnet run --project AssignmentSubmissionManagementSystem.API --launch-profile http
```

- **API Endpoint:** `http://localhost:5258`
- **Swagger Documentation:** `http://localhost:5258/swagger`
- **Health Check:** `http://localhost:5258/health`

---

### 3️⃣ Running the Frontend (React + Vite)

From the `frontend` directory:

```bash
# Copy environment variables template
cp .env.example .env

# Install dependencies
npm install

# Start Vite development server
npm run dev
```

- **Frontend Application:** `http://localhost:5173`

---

### 4️⃣ Executing Automated Tests

#### Backend Unit Tests (xUnit)
Runs tests for JWT token generation, password hashing, claims validation, and FluentValidation rules:
```bash
cd backend
dotnet test
```

#### Frontend Type Checking & Build
Validates TypeScript type safety and bundles production assets:
```bash
cd frontend
npm run build
```

---

## 📡 API Surface Summary

| Area | Controller | Key Endpoints | Description |
| :--- | :--- | :--- | :--- |
| **Authentication** | `AuthController` | `POST /api/Auth/login`<br>`GET /api/Auth/me` | JWT login authentication and session validation. |
| **Academic Setup** | `AcademicController`<br>`CourseSubjectController` | `/api/Academic/department`<br>`/api/Academic/course`<br>`/api/Academic/subject` | Department, course, subject, and batch configuration. |
| **User Accounts** | `TeacherController`<br>`StudentController`<br>`UserController` | `/api/Teacher`<br>`/api/Student`<br>`/api/User` | Staff and student account creation and profiling. |
| **Enrolment** | `TeacherSubjectController`<br>`StudentEnrollmentController` | `/api/TeacherSubject`<br>`/api/StudentEnrollment` | Subject allocation and student academic enrolments. |
| **Assignments** | `AssignmentController` | `/api/Assignment`<br>`/api/Assignment/my`<br>`/api/Assignment/student/{yearId}` | Assignment drafting, publishing, and filtering. |
| **Submissions** | `SubmissionController`<br>`AssessmentController` | `/api/Submission`<br>`/api/Assessment` | Student submission uploads and teacher evaluation. |
| **Notifications** | `NotificationController` | `/api/Notification/my`<br>`/my/unread-count`<br>`/my/read-all` | Activity alerts and unread status management. |

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for details.

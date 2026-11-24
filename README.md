# Laboratory Exercises 1–7 — `DB_Timon`

Everything below has been built and verified on this machine:

- **Database:** MariaDB 10.11 (XAMPP) is running; `DB_Timon` contains the Lab 1–6 schema including audit/session tables, 5 triggers, 3 procedures and role-based users.
- **SQL:** re-runnable scripts for all six SQL labs, plus extra seed data, in `sql/`.
- **Answers:** all lab questions answered in `answers/` (`Lab_Questions_Answers.md`, `Lab4_Questions_Answers.md`, `Lab5_Questions_Answers.md`, `Lab6_Questions_Answers.md`, `Lab7_Questions_Answers.md`).
- **VB.NET app:** complete Visual Basic Windows Forms project in `vbnet/AcademicSystem/`; builds with **0 warnings, 0 errors** and was runtime-tested for login, roles, MDI navigation, charts, real-time refresh, search, notifications, bulk entry, error dialog and exports.
- **Latest labs:** gradebook tables/views, role dashboards, indexed procedures, advanced reports, encrypted credentials, audit trails, validation triggers, session management, backup utilities and the Lab 7 advanced frontend/UX layer.

All required screenshots have been captured in dbGate / Visual Studio and inserted into the completed Lab 5, Lab 6 and Lab 7 PDFs listed near the end of this file.

---

## Demo logins

| Username | Password | Role | Opens |
|---|---|---|---|
| `admin` | `Admin@123` | Admin | Admin management form (Add Teacher, Lists, Reset Password, Gradebook, Reports) |
| `teacher1` | `Teacher@123` | Teacher | Teacher Dashboard (course overview, grade entry, grade calculation) |
| `student1` | `Student@123` | Student | Student Dashboard (personal grade summary and course averages) |

Additional accounts added for the class list:

| Username | Password | Role |
|---|---|---|
| `reyjay` | `reyjay123` | Teacher |
| `joseph` | `joseph123` | Teacher |
| `ben` | `ben123` | Teacher |
| `steven` | `steven123` | Student |
| `optimus` | `optimus123` | Student |
| `evo` | `evo123` | Student |
| `joe` | `joe123` | Student |

Passwords are stored only as salted PBKDF2-HMAC-SHA256 hashes (`pbkdf2_sha256$100000$<salt>$<hash>`), never as plain text.

---

## Folder map

```
database_timon/
├─ README.md                            ← this file
├─ sql/
│  ├─ 01_Lab1_DB_Timon_DDL.sql          ← CREATE DATABASE, students, courses + sample rows
│  ├─ 02_Lab2_Relationships.sql         ← instructors, Courses FK, enrollments junction
│  ├─ 03_Lab3_Users_Roles_Auth.sql      ← final 6-table schema, roles, demo users (standalone)
│  ├─ 04_Verification.sql               ← queries proving tables, FKs, keys and reports
│  ├─ 05_Additional_Teachers_Students.sql ← extra teachers, students, courses, enrollments
│  ├─ 06_Update_Usernames_Passwords.sql ← renames added accounts to first names / <name>123
│  ├─ 07_Safe_If_Exists_Setup.sql       ← checks existence and prints "already exists" (no drops)
│  ├─ 08_Lab4_Gradebook.sql             ← assignment_types, grades, chk_grade_range, 2 views
│  ├─ 09_Lab5_Performance_Procedures.sql← 3 indexes, 2 stored procedures, 2 reporting views
│  ├─ 10_Lab6_Security_Audit_Backup.sql  ← DB users, audit triggers, sessions, backup routines
│  └─ 00_ALL_SQL_Combined.sql           ← all scripts above in one file
├─ answers/
│  ├─ Lab_Questions_Answers.md          ← every question from Labs 1-3
│  ├─ Lab4_Questions_Answers.md         ← Lab 4 questions (composite PK, CHECK, views)
│  ├─ Lab5_Questions_Answers.md         ← Lab 5 questions (indexes, transactions, analytics)
│  └─ Lab6_Questions_Answers.md         ← Lab 6 questions (audit, triggers, performance)
├─ docs/
│  ├─ ER_Diagram.md                     ← Mermaid ER diagram (all 8 tables)
│  └─ ER_Diagram.html                   ← open in a browser to view/print the ER diagram
└─ vbnet/
   ├─ AcademicSystem.sln                ← open this in Visual Studio
   ├─ AcademicSystem.slnx               ← alternative solution format for newer SDKs
   └─ AcademicSystem/
      ├─ AcademicSystem.vbproj          ← .NET 8 Windows Forms, MySqlConnector 3.0.0
      ├─ Program.vb                     ← application entry point → LoginForm
      ├─ Db.vb                          ← connection string + query helpers
      ├─ PasswordHasher.vb              ← PBKDF2 hash / constant-time verify
      ├─ Session.vb                     ← logged-in user id, username, role
      ├─ LoginForm.vb                   ← login + role-based redirect
      ├─ AdminForm.vb                   ← teacher creation, lists, password reset,
      │                                   assignment types, all grades, enrollment, reports
      ├─ AdminForm.Designer.vb          ← includes the Gradebook tab
      ├─ TeacherDashboard.vb            ← Lab 4/5 teacher gradebook + grade calculation
      ├─ StudentDashboard.vb            ← Lab 4 student grades + course averages
      ├─ ReportsForm.vb                 ← Lab 5 transcript, analytics, CSV export
      ├─ TeacherForm.vb                 ← original Lab 3 teacher portal (kept for reference)
      └─ StudentForm.vb                 ← original Lab 3 student portal (kept for reference)
```

---

## Running the SQL in dbGate

The database is already built, so this is only needed if you want to watch it happen or rebuild it.

1. Open **dbGate** → connect to MySQL/MariaDB:
   - Host `localhost`, Port `3306`, User `root`, Password *(blank on XAMPP default)*.
2. Open a new SQL script (`File → New → SQL script`) and execute these in order (`Ctrl+Enter`):
   1. `sql/01_Lab1_DB_Timon_DDL.sql`
   2. `sql/02_Lab2_Relationships.sql`
   3. `sql/03_Lab3_Users_Roles_Auth.sql` — rebuilds the complete Lab 3 schema
   4. `sql/04_Verification.sql` — columns, foreign keys, unique keys and reports
   5. `sql/05_Additional_Teachers_Students.sql` — extra teachers/students and enrollments
   6. `sql/08_Lab4_Gradebook.sql` — gradebook tables, CHECK constraint and views
   7. `sql/09_Lab5_Performance_Procedures.sql` — indexes, procedures and reporting views

All scripts are safely re-runnable. Script `06` is only needed if you already ran an older version of `05` with dotted usernames. If you ever want to rebuild the final schema only, run `03_Lab3_Users_Roles_Auth.sql` → `05_Additional_Teachers_Students.sql` → `08_Lab4_Gradebook.sql` → `09_Lab5_Performance_Procedures.sql`.

> **"Already exists" version:** If you do **not** want any object dropped or overwritten, run `sql/07_Safe_If_Exists_Setup.sql`. It checks the database, all six base tables and every seed-data group, prints an `already exists` message for anything already present, and only creates/inserts what is missing. Every newer script follows the same pattern.

> Note: Windows/MariaDB stores table names in lowercase (`students`, `courses`, `assignment_types`, …). The application uses those lowercase names.

---

## Running the VB.NET application in Visual Studio

1. Open `vbnet/AcademicSystem.sln` in Visual Studio (found at `C:\Program Files\Microsoft Visual Studio\18\Community\`).
2. NuGet restores **MySqlConnector 3.0.0** automatically. Build with `Ctrl+Shift+B` (verified clean).
3. Press **F5**. The login form appears and redirects by role.
4. Try `admin` / `Admin@123` → **Gradebook** tab:
   - Manage assignment types (Add / Edit / Delete).
   - Enroll a student in a course using `sp_EnrollStudent` (the procedure's message is displayed), or use **Bulk Enroll**.
   - See all grades across the system in the read-only grid.
   - Click **Reports** to generate a transcript or course analytics and export it to CSV.
5. Log in as `teacher1` / `Teacher@123` → **Teacher Dashboard**:
   - Course overview from `teacher_course_overview`.
   - Choose course, student and assignment type, then **Save / Update Grade**.
   - **Calculate Final Grades** calls `sp_CalculateCourseGrade` and shows the weighted result.
6. Log in as `student1` / `Student@123` → **Student Dashboard**: personal grades from `student_grade_summary`, search box and per-course averages.
7. **Reset Password** tab on the Admin form generates a fresh salted hash for any existing user.

### Preview a form without running the app

In **Solution Explorer**, right-click any form file — `LoginForm.vb`, `AdminForm.vb`, `TeacherDashboard.vb`, `StudentDashboard.vb`, `ReportsForm.vb` — and choose **View Designer**. Database calls and the role checks are skipped at design time, so no login is needed.

If your XAMPP `root` account has a password, edit the connection string in `Db.vb`:

```vb
Private Const ConnectionString As String =
    "Server=localhost;Port=3306;Database=DB_Timon;Uid=root;Pwd=YOUR_PASSWORD;CharSet=utf8mb4;"
```

The project targets `net8.0-windows` and sets `RollForward=Major`, so it will also run on the installed .NET 10/11 Windows Desktop runtimes.

---

## Screenshot checklist (what you still need to capture)

These screenshots were captured in dbGate / Visual Studio and inserted into the completed Lab 5 PDF (see "Final Lab 5 deliverables" below).

**Lab 1**
- [ ] Object tree expanded: `Databases → DB_Timon → Tables → students` and `courses`, with column names, data types and the key icon on `student_id` / `course_id`.

**Lab 2**
- [ ] `instructors` schema: `instructor_id` (PK, AI), `first_name`, `last_name`, `department`.
- [ ] `courses` schema with the new `instructor_id` column and its **Foreign Keys** tab showing `instructor_id → instructors(instructor_id)`.
- [ ] `enrollments` schema with `enrollment_id` (PK), `student_id`, `course_id`, `enrollment_date` and both FK constraints.
- [ ] ER diagram with `Students`, `Courses`, `Instructors`, `Enrollments` and connecting lines.

**Lab 3**
- [ ] Object tree showing all six tables: `users`, `roles`, `instructors`, `students`, `courses`, `enrollments`.
- [ ] Foreign Keys tab for each table (six constraints in total — see the list in `docs/ER_Diagram.md`).
- [ ] ER diagram showing all six tables and all six relationships.
- [ ] VB.NET **Login form** screenshot.
- [ ] VB.NET **Admin form** with the Add Teacher fields.

**Lab 4**
- [ ] `assignment_types` schema: `type_id` (PK), `type_name` (UNIQUE), `default_weight`.
- [ ] `grades` schema showing the composite primary key `(student_id, course_id, type_id)` and `chk_grade_range`.
- [ ] **Foreign Keys** tab for `grades` showing its three relationships (students, courses, assignment_types).
- [ ] `student_grade_summary` and `teacher_course_overview` views in dbGate (with a sample result).
- [ ] Updated EER diagram including `assignment_types` and `grades`.
- [ ] VB.NET **Teacher Dashboard** with course overview and grade entry.
- [ ] VB.NET **Student Dashboard** with grade summary and course averages.
- [ ] VB.NET **Admin form → Gradebook** tab showing assignment-type management.

**Lab 5**
- [x] dbGate **Indexes** view showing `idx_grades_student_course`, `idx_enrollments_date`, `idx_courses_code_name`.
- [x] Execution plan for a grade lookup before and after the index.
- [x] Stored procedures `sp_EnrollStudent` and `sp_CalculateCourseGrade` open in dbGate, plus sample execution output.
- [x] `student_transcript` and `course_performance_analytics` views with sample results.
- [x] Updated EER diagram with index annotations.
- [x] VB.NET **Reports form** showing a transcript and course analytics.
- [x] VB.NET enrollment interface using the stored procedure (duplicate message captured; success path verified).
- [x] VB.NET Teacher Dashboard **Calculate Final Grades** result.
- [x] CSV export file opened in Notepad.

For any ER diagram screenshot: in dbGate right-click `DB_Timon` → **ER diagram / Show diagram** and drag the tables in from the tree. If your dbGate edition has no ER diagram view, open `docs/ER_Diagram.html` in a browser and screenshot that.

### Final Lab 5 deliverables

- **Completed PDF:** `output/results/Lab5_Completed.pdf` (generated from the original Lab 5 `.docx` template with all 82 original non-empty paragraphs preserved; header name `Winbert Joe M. Timon` and a written Conclusion were added).
- **Completed Word source:** `output/results/Lab5_Completed.docx`.
- **18 screenshots:** `output/results/` (numbered `01_` through `13_vbnet_teacher_grade_calc.png`, including the before/after `EXPLAIN` plans, stored-procedure outputs, EER diagram, reports, CSV export and grade calculation).
- **Exported sample CSV:** `output/results/course_analytics.csv`.
- **Answers:** `answers/Lab5_Questions_Answers.md`.

---

## Design notes

- `users.role_id` FK → `roles`; `instructors.user_id` and `students.user_id` are **UNIQUE FKs**, giving one-to-one profile links.
- `courses.instructor_id` is nullable with `ON DELETE SET NULL` — a course may be unassigned.
- `enrollments` has `UNIQUE (student_id, course_id)` so a student cannot enroll twice in the same course.
- Adding a teacher in the Admin form uses a **transaction**: the user account and instructor profile are both created, or neither is.
- `grades` uses a composite primary key `(student_id, course_id, type_id)` so one student has at most one grade per assignment type per course; `chk_grade_range` rejects anything outside 0–100.
- `teacher_course_overview` and `student_grade_summary` are views: the VB.NET forms query the views instead of repeating multi-table JOINs in code.
- `sp_EnrollStudent` wraps validation and insertion in a transaction with an error handler; `sp_CalculateCourseGrade` computes a weighted average from `assignment_types.default_weight`.
- Indexes target the columns used most often in lookups: grades by student/course, enrollments by date, and courses by code/name.

---

## Final Lab 6 deliverables

- **Completed PDF:** `output/results/Lab6_Completed.pdf` (built from the original Lab 6 `.docx` template with the required screenshots, short answers, header name and conclusion).
- **Completed Word source:** `output/results/Lab6_Completed.docx`.
- **Screenshots:** `output/results/lab6_*.png` — role grants, role connections, audit records, triggers, validation errors, EER diagram, audit viewer, sessions, backup utility and GitHub push.
- **Answers:** `answers/Lab6_Questions_Answers.md`.
- **SQL:** `sql/10_Lab6_Security_Audit_Backup.sql` (also appended to `sql/00_ALL_SQL_Combined.sql`).
- **VB.NET modules/forms:** `SecureConfig.vb`, `Db.vb`, `Session.vb`, `SessionMonitor.vb`, `InputValidator.vb`, `AuditLogger.vb`, `AuditLogForm.vb`, `BackupForm.vb`, `PdfExporter.vb`.

Run `sql/10_Lab6_Security_Audit_Backup.sql` after the earlier lab scripts. It is re-runnable and reports "already exists" without dropping data. Demo database roles are `dbt_admin`, `dbt_teacher` and `dbt_student`; their credentials are encrypted at rest by `SecureConfig.vb`.

---

## Final Lab 7 deliverables

- **Completed PDF:** `output/results/Lab7_Completed.pdf` (built from the original Lab 7 `.docx` template with full, uncropped screenshots, short answers, header name and conclusion).
- **Completed Word source:** `output/results/Lab7_Completed.docx`.
- **Screenshots:** `output/results/lab7_*.png` — full-screen captures of the MDI parent, charts/graphs, advanced DataGridView row colouring, notification center, advanced search, real-time refresh, custom error dialog, settings, bulk grade entry, code samples, splash screen and GitHub push.
- **Answers:** `answers/Lab7_Questions_Answers.md`.
- **VB.NET modules/forms:** `AppLogger.vb`, `AppSettings.vb`, `GlobalExceptionHandler.vb`, `RealTimeRefresher.vb`, `AdvancedUi.vb`, `AdvancedChartControl.vb`, `MainMdiForm.vb`, `NotificationCenterForm.vb`, `SearchAdvancedForm.vb`, `SettingsForm.vb`, `VisualizationForm.vb`, `BulkGradeEntryForm.vb`, `ErrorDialogForm.vb`, `CodeSamplesForm.vb`, `LogViewerForm.vb`, `SplashForm.vb`, `ExportHelper.vb`.
- **GitHub repository:** `https://github.com/winbert-code/db_timon_lab7_2411600248`.

Lab 7 keeps the existing Lab 1–6 database unchanged. Login now opens `MainMdiForm`; role dashboards, reports, audit and backup tools are MDI children. `Help > Simulate Error` demonstrates the global handler, `Help > Code Samples` shows the key implementation snippets, and `Settings` controls the background refresh interval and row colouring.

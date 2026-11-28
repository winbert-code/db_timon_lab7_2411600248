-- ============================================================
-- DB_Timon - Lab 7 Advanced VB.NET Frontend / Enhanced UX
-- ------------------------------------------------------------
-- Lab 7 does NOT create new tables. It reuses the DB_Timon
-- database built in Labs 1-6 (sql/00_ALL_SQL_Combined.sql).
--
-- This file contains:
--   1. The views Lab 7 forms read from (safe to re-run).
--   2. The exact SELECT queries used by the Lab 7 forms.
--   3. Example write operations used by Admin/Teacher forms.
--
-- Run section 1 first. Sections 2-9 are read-only and can be
-- re-run anytime. Section 10 is commented: uncomment only when
-- you want to demo the write operations.
-- ============================================================

USE db_timon;

-- Demo parameters used by the queries below.
-- Replace these with the logged-in user/student/course values.
SET @userId    = 2;      -- teacher1 (instructor_id 1)
SET @studentId = 1;      -- Juan Dela Cruz / student1
SET @courseId  = 1;      -- CS101
SET @q         = '%';    -- advanced-search LIKE parameter
SET @q1        = '%';    -- report/dashboard search parameter


-- ============================================================
-- 1. VIEWS REQUIRED BY LAB 7  (from Labs 4, 5 and 6)
--    CREATE OR REPLACE = re-runnable, no data is dropped.
-- ============================================================

-- Lab 4: student grade ledger and teacher course overview
CREATE OR REPLACE VIEW student_grade_summary AS
SELECT  s.student_id,
        CONCAT(s.first_name, ' ', s.last_name) AS student_name,
        c.course_code,
        c.course_name,
        at.type_name,
        g.grade,
        g.date_recorded
FROM    grades g
JOIN    students s          ON s.student_id = g.student_id
JOIN    courses c           ON c.course_id  = g.course_id
JOIN    assignment_types at ON at.type_id   = g.type_id;

CREATE OR REPLACE VIEW teacher_course_overview AS
SELECT  i.instructor_id,
        i.user_id,
        c.course_id,
        c.course_code,
        c.course_name,
        at.type_name,
        COUNT(g.grade)  AS submissions,
        AVG(g.grade)    AS average_grade
FROM    courses c
JOIN    instructors i        ON i.instructor_id = c.instructor_id
LEFT JOIN grades g           ON g.course_id     = c.course_id
LEFT JOIN assignment_types at ON at.type_id     = g.type_id
GROUP BY i.instructor_id, i.user_id, c.course_id,
         c.course_code, c.course_name, at.type_id, at.type_name;

-- Lab 5: reports and analytics
CREATE OR REPLACE VIEW student_transcript AS
SELECT  s.student_id,
        CONCAT(s.first_name, ' ', s.last_name)        AS student_name,
        c.course_code,
        c.course_name,
        at.type_name                                  AS assignment_type,
        at.default_weight,
        g.grade,
        e.enrollment_date,
        COALESCE(CONCAT(i.first_name, ' ', i.last_name), 'Unassigned') AS instructor_name
FROM    students s
JOIN    enrollments e       ON e.student_id = s.student_id
JOIN    courses c           ON c.course_id  = e.course_id
LEFT JOIN instructors i    ON i.instructor_id = c.instructor_id
LEFT JOIN grades g          ON g.student_id = s.student_id
                           AND g.course_id  = c.course_id
LEFT JOIN assignment_types at ON at.type_id = g.type_id;

CREATE OR REPLACE VIEW course_performance_analytics AS
SELECT  c.course_id,
        c.course_code,
        c.course_name,
        COALESCE(CONCAT(i.first_name, ' ', i.last_name), 'Unassigned') AS instructor_name,
        COALESCE(e.enrolled_students, 0) AS enrolled_students,
        COALESCE(g.grades_submitted, 0)  AS grades_submitted,
        g.average_grade,
        g.minimum_grade,
        g.maximum_grade
FROM    courses c
LEFT JOIN instructors i ON i.instructor_id = c.instructor_id
LEFT JOIN (
    SELECT course_id, COUNT(DISTINCT student_id) AS enrolled_students
    FROM enrollments
    GROUP BY course_id
) e ON e.course_id = c.course_id
LEFT JOIN (
    SELECT course_id,
           COUNT(grade) AS grades_submitted,
           ROUND(AVG(grade), 2) AS average_grade,
           MIN(grade) AS minimum_grade,
           MAX(grade) AS maximum_grade
    FROM grades
    GROUP BY course_id
) g ON g.course_id = c.course_id;

-- Lab 6: security/session and student-facing views
CREATE OR REPLACE VIEW v_active_sessions AS
SELECT s.session_id,
       s.user_id,
       s.username,
       s.login_time,
       s.last_activity,
       TIMESTAMPDIFF(MINUTE, s.last_activity, NOW()) AS idle_minutes,
       CASE WHEN TIMESTAMPDIFF(MINUTE, s.last_activity, NOW()) >= 30
            THEN 'TIMED_OUT' ELSE 'ACTIVE' END AS session_state
FROM   user_sessions s
WHERE  s.status = 'ACTIVE';

CREATE OR REPLACE VIEW student_own_profile AS
SELECT user_id, student_id, first_name, last_name
FROM   students;

CREATE OR REPLACE VIEW student_course_averages AS
SELECT g.student_id,
       c.course_code,
       c.course_name,
       ROUND(AVG(g.grade), 2) AS average_grade,
       COUNT(g.grade)        AS grades_recorded
FROM   grades g
JOIN   courses c ON c.course_id = g.course_id
GROUP BY g.student_id, c.course_code, c.course_name;


-- ============================================================
-- 2. TEACHER DASHBOARD  (TeacherDashboard.vb)
-- ============================================================

-- Course combo box
SELECT c.course_id AS Value,
       CONCAT(c.course_code, ' - ', c.course_name) AS Display
FROM   courses c
JOIN   instructors i ON i.instructor_id = c.instructor_id
WHERE  i.user_id = @userId
ORDER BY c.course_code;

-- Assignment-type combo box
SELECT type_id AS Value, type_name AS Display
FROM   assignment_types
ORDER BY type_id;

-- Student combo box (students enrolled in the selected course)
SELECT s.student_id AS Value,
       CONCAT(s.first_name, ' ', s.last_name) AS Display
FROM   enrollments e
JOIN   students s ON s.student_id = e.student_id
WHERE  e.course_id = @courseId
ORDER BY Display;

-- Advanced DataGridView overview + real-time refresh
SELECT course_code   AS 'Course',
       type_name     AS 'Assignment',
       submissions   AS 'Submissions',
       average_grade AS 'Average Grade'
FROM   teacher_course_overview
WHERE  user_id = @userId
ORDER BY course_code, type_name;

-- Same overview with Lab 7 advanced search applied
SELECT course_code   AS 'Course',
       course_name   AS 'Course Name',
       type_name     AS 'Assignment',
       submissions   AS 'Submissions',
       average_grade AS 'Average Grade'
FROM   teacher_course_overview
WHERE  user_id = @userId
  AND (course_code LIKE @q1 OR course_name LIKE @q1 OR type_name LIKE @q1)
ORDER BY course_code, type_name;


-- ============================================================
-- 3. STUDENT DASHBOARD  (StudentDashboard.vb)
-- ============================================================

SELECT student_id, first_name, last_name
FROM   student_own_profile
WHERE  user_id = @userId;

-- Real-time grade table with optional search
SELECT course_code AS 'Course Code',
       course_name AS 'Course',
       type_name   AS 'Assignment',
       grade       AS 'Grade',
       date_recorded AS 'Recorded'
FROM   student_grade_summary
WHERE  student_id = @studentId
  AND (course_code LIKE @q1 OR course_name LIKE @q1 OR type_name LIKE @q1)
ORDER BY course_code, type_name;

-- Student course averages
SELECT course_code   AS 'Course Code',
       course_name   AS 'Course',
       average_grade AS 'Average Grade',
       grades_recorded AS 'Grades Recorded'
FROM   student_course_averages
WHERE  student_id = @studentId
ORDER BY course_code;


-- ============================================================
-- 4. CHARTS / GRAPHS AND ADVANCED DATAGRIDVIEW
--    (VisualizationForm.vb)
-- ============================================================

-- Bar chart: average grade per course
SELECT course_code AS Label, average_grade AS Value
FROM   course_performance_analytics
ORDER BY course_code;

-- Pie chart: grade submissions per assignment type
SELECT at.type_name AS Label, COUNT(*) AS Value
FROM   grades g
JOIN   assignment_types at ON at.type_id = g.type_id
GROUP BY at.type_name
ORDER BY at.type_id;

-- Coloured DataGridView: course statistics
SELECT course_code   AS 'Course Code',
       course_name   AS 'Course',
       enrolled_students AS 'Enrolled',
       grades_submitted  AS 'Grades',
       average_grade AS 'Average Grade',
       minimum_grade AS 'Minimum',
       maximum_grade AS 'Maximum'
FROM   course_performance_analytics
ORDER BY course_code;


-- ============================================================
-- 5. ADVANCED SEARCH  (SearchAdvancedForm.vb)
-- ============================================================

-- Students
SELECT 'Student' AS Category,
       CONCAT(first_name, ' ', last_name) AS Name,
       CONCAT('Student ID ', student_id) AS Detail
FROM   students
WHERE  first_name LIKE @q OR last_name LIKE @q
ORDER BY Name;

-- Courses
SELECT 'Course' AS Category,
       course_code AS Name,
       course_name AS Detail
FROM   courses
WHERE  course_code LIKE @q OR course_name LIKE @q
ORDER BY Name;

-- Teachers
SELECT 'Teacher' AS Category,
       u.username AS Name,
       CONCAT(i.first_name, ' ', i.last_name, ' - ', i.department) AS Detail
FROM   users u
JOIN   instructors i ON i.user_id = u.user_id
WHERE  u.username LIKE @q OR i.first_name LIKE @q OR i.last_name LIKE @q
ORDER BY Name;

-- Grades
SELECT 'Grade' AS Category,
       CONCAT(s.first_name, ' ', s.last_name) AS Name,
       CONCAT(c.course_code, ' - ', at.type_name, ' = ', g.grade) AS Detail
FROM   grades g
JOIN   students s          ON s.student_id = g.student_id
JOIN   courses c           ON c.course_id  = g.course_id
JOIN   assignment_types at ON at.type_id   = g.type_id
WHERE  c.course_code LIKE @q OR s.first_name LIKE @q OR s.last_name LIKE @q
ORDER BY Name;

-- Audit trail
SELECT 'Audit' AS Category,
       app_user AS Name,
       CONCAT(action_type, ' on ', table_name, ' at ', event_time) AS Detail
FROM   audit_log
WHERE  app_user LIKE @q OR action_type LIKE @q OR table_name LIKE @q
ORDER BY event_time DESC
LIMIT 200;

-- All data (single UNION query used by the "All data" category)
SELECT 'Student' AS Category,
       CONCAT(first_name, ' ', last_name) AS Name,
       CONCAT('Student ID ', student_id) AS Detail
FROM   students
WHERE  first_name LIKE @q OR last_name LIKE @q
UNION ALL
SELECT 'Course', course_code, course_name
FROM   courses
WHERE  course_code LIKE @q OR course_name LIKE @q
UNION ALL
SELECT 'Teacher', u.username, CONCAT(i.first_name, ' ', i.last_name)
FROM   users u
JOIN   instructors i ON i.user_id = u.user_id
WHERE  u.username LIKE @q OR i.first_name LIKE @q OR i.last_name LIKE @q
UNION ALL
SELECT 'Grade', CONCAT(s.first_name, ' ', s.last_name),
       CONCAT(c.course_code, ' = ', g.grade)
FROM   grades g
JOIN   students s ON s.student_id = g.student_id
JOIN   courses c  ON c.course_id  = g.course_id
WHERE  c.course_code LIKE @q OR s.first_name LIKE @q
ORDER BY Category, Name;


-- ============================================================
-- 6. NOTIFICATION CENTER  (NotificationCenterForm.vb)
-- ============================================================

-- Low-average warnings
SELECT 'LOW AVERAGE' AS kind,
       CONCAT(course_code, ' average ', average_grade) AS detail,
       'Warning' AS severity,
       NOW() AS stamp
FROM   student_course_averages
WHERE  average_grade < 75
ORDER BY average_grade
LIMIT 8;

-- Recent audit notifications
SELECT 'AUDIT' AS kind,
       CONCAT(action_type, ' on ', table_name, ' by ', app_user) AS detail,
       'Info' AS severity,
       event_time AS stamp
FROM   audit_log
ORDER BY event_time DESC
LIMIT 6;

-- Active-session notification
SELECT 'SESSIONS' AS kind,
       CONCAT(COUNT(*), ' active session(s)') AS detail,
       'Info' AS severity,
       NOW() AS stamp
FROM   user_sessions
WHERE  status = 'ACTIVE';

-- Courses with the most grades submitted
SELECT 'COURSE' AS kind,
       CONCAT(course_code, ' - ', grades_submitted, ' grades') AS detail,
       'Info' AS severity,
       NOW() AS stamp
FROM   course_performance_analytics
ORDER BY grades_submitted DESC
LIMIT 5;


-- ============================================================
-- 7. REPORTS AND FORMATTED EXPORT  (ReportsForm.vb)
-- ============================================================

-- Autocomplete suggestions
SELECT CONCAT(first_name, ' ', last_name) AS name FROM students
UNION
SELECT course_code FROM courses
UNION
SELECT CONCAT(first_name, ' ', last_name) FROM instructors
ORDER BY name;

-- Student Transcript report
SELECT student_name AS 'Student',
       course_code  AS 'Course Code',
       course_name  AS 'Course',
       assignment_type AS 'Assignment',
       default_weight  AS 'Weight',
       grade        AS 'Grade',
       enrollment_date AS 'Enrolled On',
       instructor_name AS 'Instructor'
FROM   student_transcript
WHERE  student_name LIKE @q1 OR course_code LIKE @q1 OR course_name LIKE @q1
ORDER BY student_name, course_code, assignment_type;

-- Course Performance Analytics report
SELECT course_code     AS 'Course Code',
       course_name     AS 'Course Name',
       instructor_name AS 'Instructor',
       enrolled_students AS 'Enrolled Students',
       grades_submitted  AS 'Grades Submitted',
       average_grade   AS 'Average Grade',
       minimum_grade   AS 'Minimum Grade',
       maximum_grade   AS 'Maximum Grade'
FROM   course_performance_analytics
WHERE  course_code LIKE @q1 OR course_name LIKE @q1 OR instructor_name LIKE @q1
ORDER BY course_code;


-- ============================================================
-- 8. ADMIN MANAGEMENT  (AdminForm.vb)
-- ============================================================

-- Enrollment combo boxes
SELECT s.student_id AS Value, CONCAT(s.first_name, ' ', s.last_name) AS Display
FROM   students s
ORDER BY Display;

SELECT c.course_id AS Value, CONCAT(c.course_code, ' - ', c.course_name) AS Display
FROM   courses c
ORDER BY c.course_code;

SELECT i.instructor_id AS Value, CONCAT(i.first_name, ' ', i.last_name) AS Display
FROM   instructors i
ORDER BY Display;

-- Sessions / audit used by the status bar and audit viewer
SELECT session_id, user_id, username, login_time, last_activity, idle_minutes, session_state
FROM   v_active_sessions;

SELECT action_type, table_name, record_id, app_user, event_time, old_values, new_values
FROM   audit_log
ORDER BY event_time DESC
LIMIT 100;


-- ============================================================
-- 9. GRADE AND ENROLLMENT PROCEDURES USED BY THE FORMS
--    These are already defined in Lab 5 / Lab 6 scripts.
-- ============================================================

-- Enroll a student (transaction + duplicate handling inside the procedure)
-- CALL sp_EnrollStudent(@studentId, @courseId, @message);
-- SELECT @message AS enrollment_message;

-- Calculate weighted final grade (read-only output)
-- CALL sp_CalculateCourseGrade(@studentId, @finalGrade);
-- SELECT @finalGrade AS weighted_final_grade;


-- ============================================================
-- 10. WRITE EXAMPLES FROM THE VB.NET FORMS
--     COMMENTED OUT so this file is safe to re-run.
--     Uncomment only when you intentionally want to change data.
-- ============================================================

-- 10a. Teacher grade save / bulk grade entry (TeacherDashboard.vb,
--      BulkGradeEntryForm.vb). The trigger validates 0-100 and enrollment.
-- INSERT INTO grades (student_id, course_id, type_id, grade)
-- VALUES (@studentId, @courseId, 2, 88.00)
-- ON DUPLICATE KEY UPDATE grade = VALUES(grade),
--                        date_recorded = CURRENT_TIMESTAMP;

-- 10b. Add Student (AdminForm.vb) - login + profile in one transaction.
--      Password must be the salted PBKDF2 hash produced by PasswordHasher.vb.
-- START TRANSACTION;
-- INSERT INTO users (username, password_hash, email, role_id)
-- VALUES ('student2', '<PBKDF2_HASH>', 'student2@example.com', 3);
-- SET @newUserId = LAST_INSERT_ID();
-- INSERT INTO students (user_id, first_name, last_name, date_of_birth)
-- VALUES (@newUserId, 'Test', 'Student', '2005-01-01');
-- COMMIT;

-- 10c. Create Course (AdminForm.vb)
-- INSERT INTO courses (course_code, course_name, instructor_id)
-- VALUES ('CS601', 'Mobile Development', 1);


-- ============================================================
-- 11. QUICK VERIFICATION
-- ============================================================

SELECT 'course_performance_analytics' AS view_name, COUNT(*) AS rows_returned FROM course_performance_analytics
UNION ALL SELECT 'student_transcript', COUNT(*) FROM student_transcript
UNION ALL SELECT 'teacher_course_overview', COUNT(*) FROM teacher_course_overview
UNION ALL SELECT 'student_grade_summary', COUNT(*) FROM student_grade_summary
UNION ALL SELECT 'student_course_averages', COUNT(*) FROM student_course_averages
UNION ALL SELECT 'v_active_sessions', COUNT(*) FROM v_active_sessions;

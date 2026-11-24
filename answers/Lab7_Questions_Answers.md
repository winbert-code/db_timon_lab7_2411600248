# Laboratory Exercise 7 Questions and Answers

### 1. What are the advantages of using real-time data synchronization in the TeacherDashboard, and what potential challenges might arise from frequent database queries?

Real-time synchronization keeps the gradebook current without a manual reload: a teacher sees newly posted grades and the latest course averages while students and other teachers continue working. It also makes the dashboard feel responsive because the background task fetches data off the UI thread and only the finished table is applied to the grid. The main challenges are database load, stale or overlapping refreshes, and cross-thread UI errors. Frequent queries can slow the server when many users are connected, so the app uses the configurable minimum interval (3 seconds, default 10), skips a refresh while one is already running, stops the timer when the form closes, and marshals results back to the UI thread with `BeginInvoke`.

### 2. Explain how the global exception handler improves the user experience and application maintainability. Provide an example of an error scenario that would be handled gracefully.

`GlobalExceptionHandler` installs one place for UI-thread and background-thread errors. Instead of the default crash window, it writes the error to `App_Data/application.log`, mirrors it to `audit_log`, and shows a friendly `ErrorDialogForm` with a copyable technical detail box. Users know their work is safe, and developers get a consistent log format with source, exception type, message and stack trace. For example, if the report calculation receives an invalid grade (`InvalidOperationException`), the handler logs it, shows the custom dialog, and the application continues running so the user can correct the entry.

### 3. How does data visualization (charts and graphs) enhance the decision-making process for administrators and teachers compared to viewing raw data in tables?

Charts turn many rows into an immediate visual pattern. In the VisualizationForm, the bar chart compares average grades by course, the pie chart shows the share of submissions by assignment type, and the formatted grid keeps the exact numbers available. An administrator can spot a weak course or an unusual workload at a glance; a teacher can see which assignment types dominate the gradebook before planning. Tables are still needed for precise values and filtering, but charts reduce the time needed to notice trends, outliers and relative differences.

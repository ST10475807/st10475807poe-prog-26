using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace demo
{//start of namespace

    //class representing a single task/reminder item belonging to a user
    public class TaskItem
    {
        public int Id;
        public string Username;
        public string Description;
        public DateTime? ReminderDate;
        public bool IsCompleted;
        public DateTime CreatedDate;
    }


    //class to manage tasks and reminders using SQL Server.
    //Task Assistant Database Integration: every CRUD action (add, read,
    //mark as complete, delete) goes straight to the database, with
    //error handling around each call so the GUI never crashes if the
    //database is unreachable.
    public class task_manager
    {//start of class

        // ===========================================================
        // IMPORTANT: update YOUR_USERNAME and YOUR_PASSWORD below with
        // your actual SQL Server login credentials before running the
        // app. Run CreateDatabase.sql in SSMS once first to create the
        // ChatBotDB database and Tasks table.
        // ===========================================================

        private string connectionString =
    "Server=(localdb)\\MSSQLLocalDB;Database=ChatBotDB;Integrated Security=True;TrustServerCertificate=True;";

        //method to load all tasks belonging to a specific user
        public List<TaskItem> get_tasks(string username)
        {//start of method

            List<TaskItem> result = new List<TaskItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Id, Username, Description, ReminderDate, IsCompleted, CreatedDate " +
                                   "FROM Tasks WHERE Username = @username ORDER BY Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(read_task(reader));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                show_db_error("loading your tasks", ex);
            }
            catch (InvalidOperationException ex)
            {
                show_db_error("connecting to the database", ex);
            }

            return result;

        }//end of method


        //method to add a new task, with an optional reminder date
        public TaskItem add_task(string username, string description, DateTime? reminderDate)
        {//start of method

            TaskItem task = new TaskItem
            {
                Username = username,
                Description = description,
                ReminderDate = reminderDate,
                IsCompleted = false,
                CreatedDate = DateTime.Now
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Tasks (Username, Description, ReminderDate, IsCompleted, CreatedDate) " +
                                   "OUTPUT INSERTED.Id " +
                                   "VALUES (@username, @description, @reminderDate, @isCompleted, @createdDate)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@reminderDate", (object)task.ReminderDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@isCompleted", task.IsCompleted);
                        cmd.Parameters.AddWithValue("@createdDate", task.CreatedDate);

                        object newId = cmd.ExecuteScalar();

                        if (newId != null)
                            task.Id = Convert.ToInt32(newId);
                    }
                }
            }
            catch (SqlException ex)
            {
                show_db_error("adding the task", ex);
            }
            catch (InvalidOperationException ex)
            {
                show_db_error("connecting to the database", ex);
            }

            return task;

        }//end of method


        //method to set/update the reminder date for the most recently added task of a user
        //(used when the user replies "yes" to "would you like to set a reminder for this task?")
        public TaskItem set_reminder_for_last_task(string username, DateTime reminderDate)
        {//start of method

            TaskItem last = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT TOP 1 Id, Username, Description, ReminderDate, IsCompleted, CreatedDate " +
                                          "FROM Tasks WHERE Username = @username ORDER BY CreatedDate DESC";

                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                                last = read_task(reader);
                        }
                    }

                    if (last != null)
                    {
                        string updateQuery = "UPDATE Tasks SET ReminderDate = @reminderDate WHERE Id = @id";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@reminderDate", reminderDate);
                            updateCmd.Parameters.AddWithValue("@id", last.Id);
                            updateCmd.ExecuteNonQuery();
                        }

                        last.ReminderDate = reminderDate;
                    }
                }
            }
            catch (SqlException ex)
            {
                show_db_error("setting the reminder", ex);
            }
            catch (InvalidOperationException ex)
            {
                show_db_error("connecting to the database", ex);
            }

            return last;

        }//end of method


        //method to mark a task as completed
        public bool complete_task(string username, int taskId)
        {//start of method

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @id AND Username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        cmd.Parameters.AddWithValue("@username", username);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                show_db_error("completing the task", ex);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                show_db_error("connecting to the database", ex);
                return false;
            }

        }//end of method


        //method to delete a task
        public bool delete_task(string username, int taskId)
        {//start of method

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM Tasks WHERE Id = @id AND Username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        cmd.Parameters.AddWithValue("@username", username);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                show_db_error("deleting the task", ex);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                show_db_error("connecting to the database", ex);
                return false;
            }

        }//end of method


        //helper method to build a TaskItem from the current row of a SqlDataReader
        private TaskItem read_task(SqlDataReader reader)
        {
            return new TaskItem
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Description = reader.GetString(2),
                ReminderDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                IsCompleted = reader.GetBoolean(4),
                CreatedDate = reader.GetDateTime(5)
            };
        }


        //helper method to show a friendly message box instead of crashing
        //whenever a database operation fails
        private void show_db_error(string action, Exception ex)
        {
            MessageBox.Show(
                "Something went wrong while " + action + ".\n\n" + ex.Message,
                "Database Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

    }//end of class
}//end of namespace

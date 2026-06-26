using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Chatbot3
{//Start of namespace
    public class tasks
    {//start of class
     //global connection string , with variable declaration
       

        // Database connection string used throughout the application
        private readonly string connection =
            @"Server=(localdb)\MSSQLLocalDB;Database=pro_tasks;Trusted_Connection=True;";



        // Method responsible for creating the table if it does not already exist
        public void CreateTableIfNotExists()
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                try
                {
                    connect.Open();

                    // Verify the table existence before creating a new one
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='demo_tasks' AND xtype='U')
                        BEGIN
                            CREATE TABLE demo_tasks (
                                task_id INT IDENTITY(1,1) PRIMARY KEY,
                                task_name NVARCHAR(100) NOT NULL,
                                task_description NVARCHAR(255),
                                task_dueDate NVARCHAR(50),
                                task_status NVARCHAR(20)
                            )
                        END";

                    SqlCommand createCommand = new SqlCommand(createTableQuery, connect);
                    createCommand.ExecuteNonQuery();

                    connect.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error creating table: " + error.Message);
                }
            }
        }

        // Method used to verify database connectivity
        public void test_connection()
        {// Beginning of test_connection method

            /*
             * SqlConnection - Establishes communication with the database
             * SqlCommand - Executes SQL statements and queries
             * SqlDataReader - Reads records returned from queries
             *                 and makes them available to the application
             */

            // Create a database connection object
            SqlConnection connect = new SqlConnection(connection);

            // Handle any connection-related exceptions
            try
            {
                // Open and test the connection
                connect.Open();

                // Display successful connection message
                MessageBox.Show("connected..");

                // Close connection after testing
                connect.Close();

            }
            catch (Exception error)
            {
                // Display any generated error message
                MessageBox.Show(error.Message);
            }

        }// End of test_connection method



        // Method used to save a new task into the database
        public void insert_task(string name, string description, string dueDate, string status)
        {// Beginning of insert_task method

            // Create a database connection object
            // SqlConnection connects = new SqlConnection(connection);

            // Exception handling should be applied where required

            // Ensure the connection object is managed by a using statement
            using (SqlConnection connects = new SqlConnection(connection))
            {// Beginning of using block

                // Open database connection
                connects.Open();

                // SQL statement for inserting task details
                string query = $"insert into demo_tasks values('{name}','{description}','{dueDate}','{status}');";

                // Execute the insert query
                SqlCommand run_query = new SqlCommand(query, connects);

                // Run the command without returning records
                run_query.ExecuteNonQuery();

                connects.Close();

            }// End of using block

        }// End of insert_task method



        // Method used to retrieve and display all tasks
        public void load_tasks(ListView view_task)
        {// Beginning of load_tasks method

            /*
             * SqlConnection - Establishes communication with the database
             * SqlCommand - Executes SQL statements and queries
             * SqlDataReader - Reads records returned from queries
             *                 and makes them available to the application
             */

            // Create connection object
            SqlConnection connects = new SqlConnection(connection);

            // Open database connection
            connects.Open();

            // SQL statement for retrieving all tasks
            string query = $"select * from demo_tasks;";

            // Execute the retrieval query
            SqlCommand run_query = new SqlCommand(query, connects);

            // Read records returned by the query
            SqlDataReader data_collect = run_query.ExecuteReader();

            // Tracks whether any records were found
            bool data_found = false;

            // Loop through all available records
            while (data_collect.Read())
            {// Beginning of while loop

                // Indicate that at least one record exists
                data_found = true;

                // Retrieve values from each database column
                string task_id = data_collect["task_id"].ToString();
                string task_name = data_collect["task_name"].ToString();
                string task_description = data_collect["task_description"].ToString();
                string task_dueDate = data_collect["task_dueDate"].ToString();
                string task_status = data_collect["task_status"].ToString();

                // Display task information in the ListView control
                view_task.Items.Add(task_id + " " + task_name + " with " + task_description + " due on " + task_dueDate + " and is " + task_status);

            }// End of while loop

            // Display a message when no records are available
            if (!data_found)
            {// Beginning of if statement

                // Inform the user that no tasks were found
                view_task.Items.Add("No task found");

            }// End of if statement

            // Close database connection
            connects.Close();

        }// End of load_tasks method

        // Method used to mark a task as completed
        public void update_taskStatus(int id)
        {// Beginning of update_taskStatus method

            // Create database connection object
            SqlConnection connects = new SqlConnection(connection);

            // Open connection
            connects.Open();

            // SQL statement for updating task status
            string query = $"update demo_tasks set task_status='done' where task_id={id}";

            // Execute update command
            SqlCommand run_query = new SqlCommand(query, connects);
            run_query.ExecuteNonQuery();

            // Close connection after update operation
            connects.Close();

        }// End of update_taskStatus method



        // Method used to remove a task from the database
        public void delete_task(int id)
        {// Beginning of delete_task method

            // Create database connection object
            SqlConnection connects = new SqlConnection(connection);

            // Open connection
            connects.Open();

            // SQL statement for deleting a task
            string query = $"delete from demo_tasks where task_id={id}";

            // Execute delete command
            SqlCommand run_query = new SqlCommand(query, connects);

            // Run the deletion query
            run_query.ExecuteNonQuery();

            // Close connection after completion
            connects.Close();

        }//end of method

    }//end of class
}//end of namespace
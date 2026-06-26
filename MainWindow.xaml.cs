using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chatbot3
{//Start of namespace

    public partial class MainWindow : Window
    {//Start of class

  ArrayList reply = new ArrayList();
            ArrayList ignore = new ArrayList();
            user_name check_name = new user_name();

        // variables
        private List<string> activityLog = new List<string>();

        // variables
        string username = string.Empty;
        string pre_question = string.Empty;
        int counting = 0;


        //creating a class training with an object name train_ai
        training train_ai = new training();


        //for the tasks class with database to add task for reminder
        //create an instance for the class tasks
        //with an object name manage_tasks
        tasks manage_tasks = new tasks();
        //global variable to hold the task details
        string task_name, task_description, task_dueDate, task_status = string.Empty;






        // Counter for tracking how many times the user has interacted (used for reminders)
        int count_interest = 0;

        // Tracks the current quiz question index
        int currentQuestionIndex = 0;

        // Holds the user’s current quiz score
        int score = 0;

        // Stores the user’s currently selected answer
        string selectedAnswer = null;

        // Stores the list of answer buttons for the current quiz question
        List<Button> answerButtons;

        // Holds the entire list of quiz questions
        List<Question_in_quiz> questions;

        // Handles loading quiz questions from storage or file
        Quiz_Question_Load load_Quiz = new Quiz_Question_Load();



        public MainWindow()
        {//start of constructor
          

           

            InitializeComponent();
            new respond(reply, ignore) { };

            //train NLP on the run
            train_ai.Train();
            manage_tasks.CreateTableIfNotExists();
            //creating an instance for the class voice_greeting 
            //with an object name greet
            voice_greeting greet = new voice_greeting();

            //call the voice method
            greet.greet();

            // Dynamically add all buttons to the UI (custom logic)
            addAllButtons();

            // Load quiz questions automatically into the 'questions' list
            load_Quiz.autoLoadQuiz(ref questions);

            // Load questions into the UI for display or interaction
            loadQuestions();

        }







        // Switch to the Chats page
        private void chatting(object sender, RoutedEventArgs e)
        {
            // Show chats page and hide all others
            chat_page.Visibility = Visibility.Visible;
            HistoryPage.Visibility = Visibility.Hidden;
            reminderPage.Visibility = Visibility.Hidden;
            LogPage.Visibility = Visibility.Hidden;
            gamePage.Visibility = Visibility.Hidden;
        }

        // Switch to the Reminders page
        private void reminders(object sender, RoutedEventArgs e)
        {
            // Show reminders page and hide all others
            chat_page.Visibility = Visibility.Hidden;
            HistoryPage.Visibility = Visibility.Collapsed;
            reminderPage.Visibility = Visibility.Visible;
            LogPage.Visibility = Visibility.Hidden;
            gamePage.Visibility = Visibility.Hidden;



            //call the auto load task method, to show all the tasks
            autoLoad_task();
        }

        // Switch to the History page
        private void history(object sender, RoutedEventArgs e)
        {
            // Show history page and hide all others
            chat_page.Visibility = Visibility.Hidden;
            HistoryPage.Visibility = Visibility.Visible;
            reminderPage.Visibility = Visibility.Hidden;
            LogPage.Visibility = Visibility.Hidden;
            gamePage.Visibility = Visibility.Hidden;


        }

        // Switch to the Activity Log page

        private void activity(object sender, RoutedEventArgs e)
        {
            // Show log page and hide all others
            chat_page.Visibility = Visibility.Hidden;
            HistoryPage.Visibility = Visibility.Hidden;
            reminderPage.Visibility = Visibility.Hidden;
            LogPage.Visibility = Visibility.Visible;
            gamePage.Visibility = Visibility.Hidden;

            // Auto-display the activity log when navigating to the page
            ViewActivityLog_Click(sender, e);
        }

        // Switch to the Game page
        private void game(object sender, RoutedEventArgs e)
        {
            // Show game page and hide all others
            chat_page.Visibility = Visibility.Hidden;
            HistoryPage.Visibility = Visibility.Hidden;
            reminderPage.Visibility = Visibility.Hidden;
            LogPage.Visibility = Visibility.Hidden;
            gamePage.Visibility = Visibility.Visible;
        }

        // Exit the application
        private void exit(object sender, RoutedEventArgs e)
        {
            // Terminate the application immediately
            System.Environment.Exit(0);
        }






        //method to add all the buttons
        private void addAllButtons()
        {
            answerButtons = new List<Button>
    {
        optionButtonOne,
        optionButtonTwo,
        optionButtonThree,
        optionButtonFour
    };

            // Verify buttons are not null
            foreach (var btn in answerButtons)
            {
                if (btn == null)
                {
                    MessageBox.Show("One or more quiz buttons are missing from the XAML.");
                    return;
                }
            }
        }//end of code

        //reset buttons background 

        private void buttonReset()
        {
            if (answerButtons == null) return;

            //reseting the background color of the button when clicked
            foreach (Button clickButton in answerButtons)
            {
                if (clickButton != null)
                {
                    //resetting the background color of the button
                    clickButton.ClearValue(Button.BackgroundProperty);
                    clickButton.Background = System.Windows.Media.Brushes.Gray;
                }
            }
        }

        //creating a method to load the questions
        private void loadQuestions()
        {
            // Check if questions list is null or empty
            if (questions == null || questions.Count == 0)
            {
                MessageBox.Show("No quiz questions available. Please restart the app.");
                return;
            }

            //checking if the user completed the quiz
            if (currentQuestionIndex >= questions.Count)
            {
                //display message for when the quiz is completed
                MessageBox.Show($"Great job! You've completed the quiz with a score of {score}.\nThe game will now reset. Press OK to continue.");
                AddToActivityLog($"Quiz completed with score: {score}/50");

                // Reset the game
                currentQuestionIndex = 0;
                score = 0;
                selectedAnswer = null;
                loadQuestions();
                return;
            }

            // Check if answer buttons are initialized
            if (answerButtons == null || answerButtons.Count < 4)
            {
                MessageBox.Show("Quiz buttons not initialized properly.");
                return;
            }

            //get the question
            var foundQuestion = questions[currentQuestionIndex];
            question_asked.Text = foundQuestion.Text;

            //setting up the score
            score_count.Text = "Score: " + score;

            //resetting the selected answer
            selectedAnswer = null;

            //resetting the buttons
            buttonReset();

            //randomizing answers
            List<string> allAnswers = new List<string>(foundQuestion.wrongAnswer);
            allAnswers.Add(foundQuestion.correctAnswer);

            // Shuffle using Fisher-Yates algorithm (works with all .NET versions)
            Random rand = new Random();
            for (int i = allAnswers.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                // Swap
                string temp = allAnswers[i];
                allAnswers[i] = allAnswers[j];
                allAnswers[j] = temp;
            }

            //assigning answers to the buttons
            for (int count = 0; count < answerButtons.Count && count < allAnswers.Count; count++)
            {
                answerButtons[count].Content = allAnswers[count];
            }
        }//end of code



        //quiz game code


        private void optionSelected(object sender, RoutedEventArgs e)
        {
            // Reset all buttons to gray first
            buttonReset();

            // Highlight the selected button in light blue 
            Button clickButton = sender as Button;
            if (clickButton != null)
            {
                clickButton.Background = System.Windows.Media.Brushes.LightBlue;
                selectedAnswer = clickButton.Content?.ToString();
            }
        }

        private void answerButton(object sender, RoutedEventArgs e)
        {
            // Check if the user selected an option first
            if (string.IsNullOrEmpty(selectedAnswer))
            {
                MessageBox.Show("Please select an answer first!", "Quiz", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if questions list is valid
            if (questions == null || currentQuestionIndex >= questions.Count)
            {
                MessageBox.Show("Quiz data is not available.");
                return;
            }

            // Get the correct answer for the current question
            string correctAnswer = questions[currentQuestionIndex].correctAnswer;

            // Find the button that has the selected answer
            Button selectedButton = null;
            foreach (Button btn in answerButtons)
            {
                if (btn.Content?.ToString() == selectedAnswer)
                {
                    selectedButton = btn;
                    break;
                }
            }

            // Find the button that has the correct answer
            Button correctButton = null;
            foreach (Button btn in answerButtons)
            {
                if (btn.Content?.ToString() == correctAnswer)
                {
                    correctButton = btn;
                    break;
                }
            }

            // Show correct/wrong colors
            if (selectedButton != null)
            {
                if (selectedAnswer == correctAnswer)
                {
                    // Correct answer – turn green
                    selectedButton.Background = System.Windows.Media.Brushes.Green;
                    score += 2;
                }
                else
                {
                    // Wrong answer – turn red
                    selectedButton.Background = System.Windows.Media.Brushes.Red;

                    // Highlight the correct answer in green so user can learn
                    if (correctButton != null)
                    {
                        correctButton.Background = System.Windows.Media.Brushes.Green;
                    }
                }
            }

            // Update score display
            score_count.Text = "Score: " + score;

            // Wait a moment so user can see the feedback, then move to next question
            // using a timer to delay the next question
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                currentQuestionIndex++;
                loadQuestions();
            };
            timer.Start();
        }


        //proceed  event handler
        private void proceed(object sender, RoutedEventArgs e)
        {
            //Hide home page grid and set Username grid visible
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }










        //submit name  event handler
        private void submit_name(object sender, RoutedEventArgs e)
        {



            //check the user name from memory recall
            username = check_name.submit_name(usernames_input, chats);





            //Hide username page grid and set chats grid visible
            username_grid.Visibility = Visibility.Hidden;
            MainPage.Visibility = Visibility.Visible;
        }





        // Handles double-clicking on an item in the reminder list
        // Handles double-clicking on an item in the reminder list
        private void remind_append_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (view_tasks.SelectedItem == null) return;

            // Get the selected value
            string get_selected_value = view_tasks.SelectedItem.ToString();

            // Get the id using substring (assuming ID is the first character)
            string get_id = get_selected_value.Substring(0, 1);
            int id = int.Parse(get_id);

            // Check if the selected task ends with "done"
            if (get_selected_value.ToLower().EndsWith("done"))
            {
                // Delete the task
                manage_tasks.delete_task(id);
                AddToActivityLog($"Task {id} deleted.");
            }
            else
            {
                // Mark as done
                manage_tasks.update_taskStatus(id);
                AddToActivityLog($"Task {id} marked as done.");
            }

            // Refresh the task list
            autoLoad_task();
        }




        //method to auto load the Tasks of a user
        // Method to auto load the Tasks of a user
        private void autoLoad_task()
        {
            // Clear the ListView first
            view_tasks.Items.Clear();

            // Load tasks from database
            manage_tasks.load_tasks(view_tasks);
        }


        //send event handler
        private void send(object sender, RoutedEventArgs e)
        {
            // Get the question from the design 
            string rawQuestion = question.Text.ToString();

            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                error_method("ChatBot", "Please enter a question.");
                return;
            }

            // Remove special characters and clean the question
            string questions = RemoveSpecialCharacters(rawQuestion);

            // Show what the user typed 
            error_method(username, rawQuestion);


            //ai chats and auto_show_interest
            auto_show_interest();
            ai_check(questions);
        }

        //end for the username submit



        //start of ai_chat method
        private void ai_check(string questions)
        {


            // Check if user entered anything meaningful
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "Please enter a valid question.");
                question.Clear();
                return;
            }



            // Check if the question contains only special characters or empty after cleaning
            if (questions.Length == 0 || string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "I couldn't understand that.");
                question.Clear();
                return;
            }




            // Variables for processing
            string[] words = questions.ToLower().Split(new char[] { ' ', ',', '.', '?', '!', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
            bool found = false;
            string message = string.Empty;
            Random indexer = new Random();
            List<string> per_word = new List<string>();
            List<string> answers_found = new List<string>();





            if (questions.ToLower().StartsWith("add task"))
            {//start of if add task

                //add the task to the listview as part of the chats
                message += "Great your task is added, would you like a reminder?\n and also ";

                //then filter to get task name
                task_name = questions.Replace("add task", "");



            }//end of if checking task add or add task


            //check if the user want to set a reminder
            if (questions.ToLower().StartsWith("yes, remind me in") || questions.ToLower().StartsWith("yes remind me in"))
            {//start of the reminder if

                //replace the yes, remind me in
                string reminder = questions.Replace("yes, remind me in", "");

                //then get the number inside the reminder variable
                string days_number = Regex.Replace(reminder, @"[^0-9]", "");

                //cast the days_number to an int
                int days = int.Parse(days_number);

                //add the days the user chose to do the task to the current date
                DateTime user_reminder = DateTime.Now.AddDays(days);



                //format the date how it should be
                //Like  06-07-2026 or Jun 15 2026
                string format_date = user_reminder.ToString("MMMM dd yyyy");
                //assign the format date with task_dueDate and status
                task_dueDate = format_date;
                task_status = "pending";

                //call the insert method to store the task
                error_method("ChatBot: ", "good , i will remind you in " + days + " days, on the " + format_date + "\n, to view your task click on view tasks");
                manage_tasks.insert_task(task_name, task_description, task_dueDate, task_status);

                return;

            }//end of the reminder if


            // Process each word
            foreach (string word in words)
            {
                // Skip very short words or ignored words
                if (word.Length < 3 || ignore.Contains(word.ToLower()))
                    continue;

                per_word.Clear();





                //start of interests




                if (word.Contains("interested"))
                {
                    string store_interests = string.Empty;
                    bool found_interest = false;

                    HashSet<string> currentInterests = new HashSet<string>();

                    foreach (string interest in words)
                    {
                        // CLEAN INPUT
                        string clean = interest.ToLower().Trim();
                        clean = Regex.Replace(clean, @"[^a-zA-Z0-9\s]", "");

                        // FILTER NOISE WORDS
                        if (!ignore.Contains(clean) && clean != "interested" && clean != "and" && clean != "in" && clean.Length >= 3)
                        {
                            found_interest = true;
                            currentInterests.Add(clean);
                        }
                    }


                    // prepare interests
                    store_interests = string.Join(", ", currentInterests);

                    if (found_interest && !string.IsNullOrWhiteSpace(store_interests))
                    {
                        string filename = "interested_topic.txt";
                        bool userFound = false;

                        if (File.Exists(filename))
                        {
                            string[] lines = File.ReadAllLines(filename);

                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (lines[i].StartsWith(username))
                                {
                                    userFound = true;

                                    //get all the interests
                                    string existing = lines[i].Replace(username + " interested in:", "").ToLower();

                                    HashSet<string> existingSet = new HashSet<string>(existing.Split(',').Select(x => x.Trim()).Where(x => x != ""));

                                    // remove dumplicates
                                    foreach (string item in currentInterests)
                                    {
                                        existingSet.Add(item);
                                    }

                                    string finalList = string.Join(", ", existingSet);

                                    lines[i] = username + " interested in: " + finalList;
                                    File.WriteAllLines(filename, lines);

                                    message += "great, i added " + store_interests + " to your interests and ";
                                    break;
                                }
                            }
                        }

                        if (!userFound)
                        {
                            File.AppendAllText(
                                filename,
                                username + " interested in: " + store_interests + "\n"
                            );

                            message += "great, i will remember that you are interested in " + store_interests + " and ";
                        }
                    }
                    else
                    {
                        message += "Please specify what you're interested in (e.g., 'I am interested in cybersecurity')";
                    }
                }



                //end of interests




                // Search for matching answers
                bool wordFound = false;
                foreach (string answer in reply)
                {
                    if (answer.ToLower().Contains(word))
                    {
                        wordFound = true;
                        per_word.Add(answer);
                    }
                }

                if (wordFound && per_word.Count > 0)
                {
                    found = true;
                    int indexing = indexer.Next(0, per_word.Count);
                    answers_found.Add(per_word[indexing]);
                }
            }

            // Show responses or error message
            if (found && answers_found.Count > 0)
            {
                // Remove duplicate answers
                answers_found = answers_found.Distinct().ToList();

                foreach (string per_answer in answers_found)
                {
                    message += per_answer + "\n";
                    task_description += per_answer + " ";
                }
                //task_description = message.TrimEnd('\n');
                error_method("ChatBot", message.TrimEnd('\n'));


                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
            }
            else
            {


                string response = train_ai.CheckAndLearn(questions);


                // It will learn from unknown questions
                string unknownResponse = train_ai.CheckAndLearn(questions);
                //display error message for message not found


                error_method("ChatBot", unknownResponse);
            }

            // Clear the input box
            question.Clear();


        }

        //end of ai_chat method




        //method to remove special characters
        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            StringBuilder sanitized = new StringBuilder();

            foreach (char c in input)
            {
                // Keep letters, numbers, spaces, and basic punctuation
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '\'' || c == '-')
                {
                    sanitized.Append(c);
                }
                else
                {
                    // Replace other special characters with space
                    sanitized.Append(' ');
                }
            }

            // Clean up extra spaces and trim
            string result = sanitized.ToString();
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ").Trim();

            return result;
        }


        //end of method to remove special characters





        //method count to show interests randomly
        private void auto_show_interest()
        {
            //check if three times
            if (counting == 3)
            {
                //read the user's interests from file
                string filename = "interested_topic.txt";

                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);

                    //find the user's line
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(username))
                        {
                            //get the interests part
                            int colonIndex = line.IndexOf("interested in:");
                            if (colonIndex >= 0)
                            {
                                string interests = line.Substring(colonIndex + 14).Trim();

                                //show reminder of interests
                                error_method("ChatBot", "Just a reminder, you are interested in " + interests + " and ");
                                ai_check(interests);
                                break;
                            }
                        }
                    }
                }

                //reset counting
                counting = 0;
            }
            else
            {
                //incrementing
                counting += 1;
            }
        }
        //end of count interest method






        // Updated error method with better formatting
        private void error_method(string name, string message)
        {
            // Create a border for chats
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5)
            };

            // Set different background for user vs bot
            if (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat"))
            {// Light blue
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(173, 216, 230));
            }
            else
            {    // Light gray
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            }
            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2)
            };

            // Set color based on sender
            Brush nameColor = (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat")) ?
                              Brushes.DarkBlue : Brushes.DarkGreen;

            Brush messageColor = Brushes.Black;

            messageText.Inlines.Add(new Run
            {
                Text = name + ": ",
                Foreground = nameColor,
                FontWeight = FontWeights.Bold
            });

            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = messageColor
            });

            messageBorder.Child = messageText;
            chats.Items.Add(messageBorder);

            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }//end of error method


        private void AddToActivityLog(string entry)
        {
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            activityLog.Add($"[{timestamp}] {entry}");

            // Keep only the last 50 entries
            if (activityLog.Count > 50)
                activityLog.RemoveAt(0);
        }


        /// Handles the Check button click on the Activity Log page
        /// Displays the activity log in the log_append ListView

        private void ViewActivityLog_Click(object sender, RoutedEventArgs e)
        {
            log_append.Items.Clear();

            if (activityLog.Count == 0)
            {
                log_append.Items.Add("No activity recorded yet.");
            }
            else
            {
                // Get the last 10 entries 
                var recent = activityLog.Skip(Math.Max(0, activityLog.Count - 10)).Reverse().ToList();
                foreach (var entry in recent)
                {
                    log_append.Items.Add(entry);
                }
            }
        }


        /// Navigates to the Add Task page 

        private void addTaskPage(object sender, RoutedEventArgs e)
        {
            chat_page.Visibility = Visibility.Hidden;
            HistoryPage.Visibility = Visibility.Hidden;
            reminderPage.Visibility = Visibility.Visible;
            LogPage.Visibility = Visibility.Hidden;
            gamePage.Visibility = Visibility.Hidden;

            // Refresh the task list
            autoLoad_task();
        }


        /// Handles the Add Task button click
        /// Validates input, inserts task into database, and logs the activity

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string name = taskNameInput.Text.Trim();
            string description = taskDescriptionInput.Text.Trim();

            // Validate task name
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a task name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string dueDate = "No deadline";
            string status = "pending";

            // Check reminder selection
            if (reminderToday.IsChecked == true)
            {
                dueDate = DateTime.Now.ToString("MMMM dd yyyy");
                status = "pending";
            }
            else if (reminderCustom.IsChecked == true)
            {
                if (int.TryParse(reminderDaysInput.Text, out int days) && days > 0)
                {
                    dueDate = DateTime.Now.AddDays(days).ToString("MMMM dd yyyy");
                    status = "pending";
                }
                else
                {
                    MessageBox.Show("Please enter a valid number of days.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else // No reminder
            {
                dueDate = "No deadline";
                status = "pending";
            }

            // Insert into database
            manage_tasks.insert_task(name, description, dueDate, status);

            // Log the activity
            AddToActivityLog($"Task added: {name} (due: {dueDate})");

            // Clear the input fields
            ClearTaskFields_Click(sender, e);

            // Refresh the task list
            autoLoad_task();

            MessageBox.Show($"Task '{name}' added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        /// Clears all input fields on the Add Task page

        private void ClearTaskFields_Click(object sender, RoutedEventArgs e)
        {
            taskNameInput.Clear();
            taskDescriptionInput.Clear();
            reminderNo.IsChecked = true;
            reminderDaysInput.Text = "3";
            reminderDaysInput.IsEnabled = false;
        }



        //This radio button Disables the days input when "No" is selected

        private void reminderNo_Checked(object sender, RoutedEventArgs e)
        {
            if (reminderDaysInput != null)
                reminderDaysInput.IsEnabled = false;
        }


        //This radio button Disables the days input when "Today" is selected

        private void reminderToday_Checked(object sender, RoutedEventArgs e)
        {
            if (reminderDaysInput != null)
                reminderDaysInput.IsEnabled = false;
        }


        //This radio button enables the days input when "In (days)" is selected

        private void reminderCustom_Checked(object sender, RoutedEventArgs e)
        {
            if (reminderDaysInput != null)
            {
                reminderDaysInput.IsEnabled = true;
                reminderDaysInput.Focus();
            }
        }

    }//end of event handler

    
}//end of namespace

using demo;
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

namespace demo
{//start of namespace

    public partial class MainWindow : Window
    {//start of class


        //creating an instance for the class Array
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();
        user_name check_name = new user_name();

        //instances for the Part 3 features:
        //Task 2 - cybersecurity quiz mini-game
        //Task 3 - NLP simulation (keyword/phrase intent detection)
        //Task 4 - activity log
        //plus a simple task/reminder store the NLP commands act on
        quiz_manager quiz = new quiz_manager();
        nlp_processor nlp = new nlp_processor();
        activity_log activityLog = new activity_log();
        task_manager tasks = new task_manager();

        // variables
        string username = string.Empty;
        string pre_question = string.Empty;
        int counting = 0;

        //true while the bot is waiting for a yes/no/date reply to
        //"would you like to set a reminder for this task?"
        bool awaiting_reminder_response = false;



        public MainWindow()
        {
            InitializeComponent();

            new respond(reply, ignore) { };

            //creating an instance for the class voice_greeting 
            //with an object name greet
            voice_greeting greet = new voice_greeting();

            //call the voice method
            greet.greet();
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
            // Get username from textbox
            string enteredName = usernames_inputs.Text.Trim();

            // CHECK IF USERNAME IS EMPTY
            if (string.IsNullOrWhiteSpace(enteredName))
            {
                MessageBox.Show("Please enter a username.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // save/check username
            username = check_name.submit_name(usernames_inputs, chats);
            activityLog.log_action(username, "Session started");

            // Hide username page grid and show chat grid
            username_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;
        }








        //send event handler
        private void send(object sender, RoutedEventArgs e)
        {
            // Get the question from the design and sanitize it
            string rawQuestion = question.Text.ToString().Trim();

            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                error_method("ChatBot", "Please enter a question.");
                return;
            }

            // Show what the user typed
            error_method(username, rawQuestion);
            question.Clear();


            //--- Task 2: if a quiz is currently in progress, every message
            //typed by the user is treated as an answer to the current question
            if (quiz.quiz_active)
            {
                handle_quiz_answer(rawQuestion);
                return;
            }


            //--- Task 3: NLP simulation - try to detect a recognised intent
            //(tasks, reminders, quiz, activity log, help) from the raw,
            //un-sanitised text so phrases and dates are easier to match
            BotIntent intent = nlp.detect_intent(rawQuestion);


            //if we were waiting for a reply about setting a reminder, and the
            //user didn't immediately start a brand new command, treat this
            //message as the reminder response (yes / no / a date)
            if (awaiting_reminder_response && intent == BotIntent.None)
            {
                handle_reminder_response(rawQuestion);
                return;
            }

            awaiting_reminder_response = false;

            if (intent != BotIntent.None)
            {
                handle_intent(intent, rawQuestion);
                return;
            }


            //--- fall back to the original keyword/sentiment based chatbot logic
            string questions = RemoveSpecialCharacters(rawQuestion);

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
                                    activityLog.log_action(username, "Interest(s) added: " + store_interests);
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
                            activityLog.log_action(username, "Interest(s) added: " + store_interests);
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
                }

                error_method("ChatBot", message.TrimEnd('\n'));


                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
            }
            else
            {
                // when nothing is found
                string[] fallbackMessages = {
            "I'm sorry, I don't understand that. Could you rephrase your question?",
            "I didn't quite get that. Try asking about cyber security topics.",
            "Hmm, I'm not sure how to respond to that. Can you ask something else?",
            "I couldn't find an answer for that. Please ask about programming, security, or technology.",
            "My apologies, I don't have information on that topic yet."
        };

                Random random = new Random();
                string fallbackMessage = fallbackMessages[random.Next(fallbackMessages.Length)];
                error_method("ChatBot", fallbackMessage);
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






        //========================================================
        // PART 3 - TASK 3: NLP INTENT HANDLING
        //========================================================

        //method to route a detected intent to the correct handler method
        private void handle_intent(BotIntent intent, string rawQuestion)
        {//start of method

            switch (intent)
            {
                case BotIntent.AddTask:
                    handle_add_task(rawQuestion);
                    break;

                case BotIntent.SetReminder:
                    handle_set_reminder(rawQuestion);
                    break;

                case BotIntent.ShowTasks:
                    show_tasks();
                    break;

                case BotIntent.CompleteTask:
                    handle_complete_task(rawQuestion);
                    break;

                case BotIntent.DeleteTask:
                    handle_delete_task(rawQuestion);
                    break;

                case BotIntent.ShowActivityLog:
                    show_activity_log();
                    break;

                case BotIntent.ShowMoreLog:
                    show_full_log();
                    break;

                case BotIntent.StartQuiz:
                    start_quiz();
                    break;

                case BotIntent.Help:
                    show_help();
                    break;
            }

        }//end of method


        //method to handle a recognised "add a task" style request
        private void handle_add_task(string rawQuestion)
        {//start of method

            string description = nlp.extract_payload(rawQuestion, BotIntent.AddTask);

            if (string.IsNullOrWhiteSpace(description))
            {
                error_method("ChatBot", "Sure - what should the task be? For example: 'Add a task to enable 2FA'.");
                return;
            }

            DateTime? date = nlp.extract_date(rawQuestion);

            tasks.add_task(username, description, date);

            if (date.HasValue)
            {
                activityLog.log_action(username, "Task added: '" + description + "' (Reminder set for " + date.Value.ToString("dd MMM yyyy") + ")");
                error_method("ChatBot", "Task added: '" + description + "'. Reminder set for " + date.Value.ToString("dd MMM yyyy") + ".");
            }
            else
            {
                activityLog.log_action(username, "Task added: '" + description + "' (no reminder set)");
                error_method("ChatBot", "Task added: '" + description + "'. Would you like to set a reminder for this task? (yes/no, or give me a date)");
                awaiting_reminder_response = true;
            }

        }//end of method


        //method to handle a recognised "remind me to ..." style request
        private void handle_set_reminder(string rawQuestion)
        {//start of method

            string description = nlp.extract_payload(rawQuestion, BotIntent.SetReminder);

            if (string.IsNullOrWhiteSpace(description))
            {
                error_method("ChatBot", "What would you like to be reminded about?");
                return;
            }

            DateTime? date = nlp.extract_date(rawQuestion);

            //default to tomorrow if the user gave no specific date phrase
            if (!date.HasValue)
                date = DateTime.Now.Date.AddDays(1);

            tasks.add_task(username, description, date);
            activityLog.log_action(username, "Reminder set for '" + description + "' on " + date.Value.ToString("dd MMM yyyy"));

            error_method("ChatBot", "Reminder set for '" + description + "' on " + date.Value.ToString("dd MMM yyyy") + ".");

        }//end of method


        //method to handle the user's reply to "would you like to set a reminder for this task?"
        private void handle_reminder_response(string rawQuestion)
        {//start of method

            awaiting_reminder_response = false;

            string text = rawQuestion.ToLower().Trim();

            if (Regex.IsMatch(text, @"\bno\b") || Regex.IsMatch(text, @"\bnah\b"))
            {
                error_method("ChatBot", "No problem, the task has been saved without a reminder.");
                return;
            }

            DateTime? date = nlp.extract_date(rawQuestion);

            //if they said "yes"/"sure"/"ok" with no specific date, default to tomorrow
            if (!date.HasValue && (Regex.IsMatch(text, @"\byes\b") || Regex.IsMatch(text, @"\bsure\b") || Regex.IsMatch(text, @"\bok(ay)?\b")))
                date = DateTime.Now.Date.AddDays(1);

            if (!date.HasValue)
            {
                error_method("ChatBot", "Sorry, I couldn't understand that date. The task will be saved without a reminder for now - you can say 'show my tasks' anytime.");
                return;
            }

            TaskItem updated = tasks.set_reminder_for_last_task(username, date.Value);

            if (updated != null)
            {
                activityLog.log_action(username, "Reminder set for '" + updated.Description + "' on " + date.Value.ToString("dd MMM yyyy"));
                error_method("ChatBot", "Reminder set for '" + updated.Description + "' on " + date.Value.ToString("dd MMM yyyy") + ".");
            }

        }//end of method


        //method to display all of the current user's tasks
        private void show_tasks()
        {//start of method

            List<TaskItem> userTasks = tasks.get_tasks(username);

            if (userTasks.Count == 0)
            {
                error_method("ChatBot", "You don't have any tasks yet. Try saying 'Add a task to enable 2FA'.");
                return;
            }

            StringBuilder sb = new StringBuilder("Here are your tasks:\n");
            int i = 1;

            foreach (TaskItem t in userTasks)
            {
                string status = t.IsCompleted ? "[Done]" : "[Pending]";
                string reminder = t.ReminderDate.HasValue ? " - reminder: " + t.ReminderDate.Value.ToString("dd MMM yyyy") : "";

                sb.Append(i + ". " + status + " " + t.Description + reminder + " (id:" + t.Id + ")\n");
                i++;
            }

            error_method("ChatBot", sb.ToString().TrimEnd('\n'));

        }//end of method


        //method to handle "complete task <number>" style requests
        private void handle_complete_task(string rawQuestion)
        {//start of method

            Match match = Regex.Match(rawQuestion, @"\d+");

            if (!match.Success)
            {
                error_method("ChatBot", "Please tell me the task number to complete, e.g. 'complete task 2'. You can see task numbers with 'show my tasks'.");
                return;
            }

            int id = int.Parse(match.Value);
            bool success = tasks.complete_task(username, id);

            if (success)
            {
                activityLog.log_action(username, "Task #" + id + " marked as complete");
                error_method("ChatBot", "Nice work! Task #" + id + " has been marked as complete.");
            }
            else
            {
                error_method("ChatBot", "I couldn't find a task with id " + id + ". Try 'show my tasks' to see the list.");
            }

        }//end of method


        //method to handle "delete task <number>" style requests
        private void handle_delete_task(string rawQuestion)
        {//start of method

            Match match = Regex.Match(rawQuestion, @"\d+");

            if (!match.Success)
            {
                error_method("ChatBot", "Please tell me the task number to delete, e.g. 'delete task 2'. You can see task numbers with 'show my tasks'.");
                return;
            }

            int id = int.Parse(match.Value);
            bool success = tasks.delete_task(username, id);

            if (success)
            {
                activityLog.log_action(username, "Task #" + id + " deleted");
                error_method("ChatBot", "Done - task #" + id + " has been deleted.");
            }
            else
            {
                error_method("ChatBot", "I couldn't find a task with id " + id + ". Try 'show my tasks' to see the list.");
            }

        }//end of method


        //========================================================
        // PART 3 - TASK 4: ACTIVITY LOG
        //========================================================

        //method to display the user's most recent activity (5 entries)
        private void show_activity_log()
        {//start of method

            List<string> recent = activityLog.get_recent(username, 5);

            if (recent.Count == 0)
            {
                error_method("ChatBot", "There's no activity logged yet. Start chatting, adding tasks, or playing the quiz!");
                return;
            }

            StringBuilder sb = new StringBuilder("Here's a summary of recent actions:\n");

            for (int i = 0; i < recent.Count; i++)
                sb.Append((i + 1) + ". " + recent[i] + "\n");

            if (activityLog.count_for_user(username) > recent.Count)
                sb.Append("(say 'show more' to see your full history)");

            error_method("ChatBot", sb.ToString().TrimEnd('\n'));

        }//end of method


        //method to display the user's complete activity history
        private void show_full_log()
        {//start of method

            List<string> all = activityLog.get_all(username);

            if (all.Count == 0)
            {
                error_method("ChatBot", "There's no activity logged yet.");
                return;
            }

            StringBuilder sb = new StringBuilder("Your full activity history:\n");

            for (int i = 0; i < all.Count; i++)
                sb.Append((i + 1) + ". " + all[i] + "\n");

            error_method("ChatBot", sb.ToString().TrimEnd('\n'));

        }//end of method


        //========================================================
        // PART 3 - TASK 2: CYBERSECURITY QUIZ MINI-GAME
        //========================================================

        //method to begin the quiz
        private void start_quiz()
        {//start of method

            quiz.start_quiz();
            activityLog.log_action(username, "Quiz started");

            error_method("ChatBot", "Let's test your cybersecurity knowledge! I'll ask " + quiz.questions.Count +
                " questions, mixing multiple-choice and true/false. Type 'exit quiz' anytime to stop.");

            ask_next_question();

        }//end of method


        //method to display the next quiz question
        private void ask_next_question()
        {//start of method

            QuizQuestion current = quiz.get_current_question();

            if (current == null)
            {
                finish_quiz();
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Question " + (quiz.current_index + 1) + " of " + quiz.questions.Count + ": " + current.QuestionText + "\n");

            foreach (string option in current.Options)
                sb.Append(option + "\n");

            error_method("ChatBot", sb.ToString().TrimEnd('\n'));

        }//end of method


        //method to handle the user's answer while a quiz is active
        private void handle_quiz_answer(string rawQuestion)
        {//start of method

            string lower = rawQuestion.ToLower();

            if (lower.Contains("exit quiz") || lower.Contains("stop quiz") || lower.Contains("quit quiz"))
            {
                quiz.quiz_active = false;
                activityLog.log_action(username, "Quiz exited early - score: " + quiz.score + "/" + quiz.current_index);
                error_method("ChatBot", "No problem, exiting the quiz. Your score was " + quiz.score + "/" + quiz.current_index + ".");
                return;
            }

            bool correct;
            string explanation = quiz.submit_answer(rawQuestion, out correct);

            string feedback = (correct ? "Correct! " : "Not quite. ") + explanation;
            error_method("ChatBot", feedback);

            if (quiz.is_finished())
                finish_quiz();
            else
                ask_next_question();

        }//end of method


        //method to show the final score and feedback once the quiz ends
        private void finish_quiz()
        {//start of method

            string finalMessage = quiz.final_feedback();
            activityLog.log_action(username, "Quiz completed - " + finalMessage);
            error_method("ChatBot", finalMessage);

        }//end of method


        //method to show the user a short help/command summary
        private void show_help()
        {//start of method

            string helpText = "Here's what I can help you with:\n" +
                               "- Ask me about cybersecurity topics (phishing, passwords, firewalls, VPNs, etc.)\n" +
                               "- Say 'add a task to ...' to create a task\n" +
                               "- Say 'remind me to ... tomorrow' to set a reminder\n" +
                               "- Say 'show my tasks' to view your tasks\n" +
                               "- Say 'complete task <number>' to mark a task done\n" +
                               "- Say 'delete task <number>' to remove a task\n" +
                               "- Say 'start quiz' to play the cybersecurity quiz\n" +
                               "- Say 'show activity log' to see a summary of recent actions";

            error_method("ChatBot", helpText);

        }//end of method




        //button handler to start the cybersecurity quiz directly from the GUI
        private void quizButton_Click(object sender, RoutedEventArgs e)
        {
            if (!quiz.quiz_active)
                start_quiz();
            else
                error_method("ChatBot", "A quiz is already in progress! Type 'exit quiz' to stop it first.");
        }


        //button handler to show the activity log directly from the GUI
        private void logButton_Click(object sender, RoutedEventArgs e)
        {
            show_activity_log();
        }


        //button handler to show the help/command list directly from the GUI
        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            show_help();
        }




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
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(170, 248, 255));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(173, 216, 230));
            }
            else
            {    // Light gray
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(255, 245, 245));
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
                              Brushes.Red : Brushes.Violet;

            Brush messageColor = Brushes.Olive;

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

















    }//end of class
}//end of namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace demo
{//start of namespace

    //enum representing the different commands/intents the chatbot can recognise
    public enum BotIntent
    {
        None,
        AddTask,
        SetReminder,
        ShowTasks,
        CompleteTask,
        DeleteTask,
        ShowActivityLog,
        ShowMoreLog,
        StartQuiz,
        Help
    }


    //class that simulates basic Natural Language Processing by detecting
    //keywords/phrases within free-form user input, regardless of exact wording.
    //Task 3 of the POE: uses dictionaries of trigger phrases + string.Contains()
    //based matching so the chatbot recognises requests worded in different ways.
    public class nlp_processor
    {//start of class

        //dictionary mapping each intent to a list of trigger phrases that
        //should be recognised as meaning that intent
        private Dictionary<BotIntent, List<string>> triggers = new Dictionary<BotIntent, List<string>>();

        //the order intents are checked in matters - more specific, longer
        //phrases are checked before shorter/more generic ones
        private List<BotIntent> checkOrder = new List<BotIntent>
        {
            BotIntent.AddTask,
            BotIntent.SetReminder,
            BotIntent.DeleteTask,
            BotIntent.CompleteTask,
            BotIntent.ShowMoreLog,
            BotIntent.ShowActivityLog,
            BotIntent.ShowTasks,
            BotIntent.StartQuiz,
            BotIntent.Help
        };


        public nlp_processor()
        {
            build_triggers();
        }


        //method to populate the keyword/phrase dictionary for every intent
        private void build_triggers()
        {//start of method

            triggers[BotIntent.AddTask] = new List<string>
            {
                "add a task", "add task", "create a task", "new task",
                "add to do", "add to-do", "add an item"
            };

            triggers[BotIntent.SetReminder] = new List<string>
            {
                "remind me", "set a reminder", "set reminder",
                "create a reminder", "reminder to", "add a reminder"
            };

            triggers[BotIntent.ShowTasks] = new List<string>
            {
                "show my tasks", "show tasks", "view tasks", "list tasks",
                "what tasks", "see my tasks", "show to do", "show to-do"
            };

            triggers[BotIntent.CompleteTask] = new List<string>
            {
                "complete task", "mark task", "finish task",
                "done with task", "task done", "task complete", "mark as done"
            };

            triggers[BotIntent.DeleteTask] = new List<string>
            {
                "delete task", "remove task", "cancel task", "delete my task"
            };

            triggers[BotIntent.ShowActivityLog] = new List<string>
            {
                "activity log", "show log", "what have you done for me",
                "show summary", "recent actions", "show activity", "show history"
            };

            triggers[BotIntent.ShowMoreLog] = new List<string>
            {
                "show more", "full history", "see all activity", "see more", "see full log"
            };

            triggers[BotIntent.StartQuiz] = new List<string>
            {
                "start quiz", "play quiz", "take the quiz", "test me",
                "cyber quiz", "quiz me", "quiz"
            };

            triggers[BotIntent.Help] = new List<string>
            {
                "help", "what can you do", "commands", "show menu", "show help"
            };

        }//end of method


        //method to detect the user's intent from free-form text.
        //Uses simple string.Contains() based matching so slightly different
        //wording (e.g. "add a task to enable 2FA") is still recognised.
        public BotIntent detect_intent(string input)
        {//start of method

            if (string.IsNullOrWhiteSpace(input))
                return BotIntent.None;

            string text = input.ToLower().Trim();

            foreach (BotIntent intent in checkOrder)
            {
                foreach (string phrase in triggers[intent])
                {
                    if (text.Contains(phrase))
                        return intent;
                }
            }

            return BotIntent.None;

        }//end of method


        //method to strip the matched trigger phrase (and common connector
        //words like "to"/"for") out of the input, leaving just the
        //"payload" - i.e. the actual task/reminder description
        public string extract_payload(string input, BotIntent intent)
        {//start of method

            if (!triggers.ContainsKey(intent))
                return input.Trim();

            string lowerInput = input.ToLower();

            foreach (string phrase in triggers[intent])
            {
                int index = lowerInput.IndexOf(phrase);

                if (index >= 0)
                {
                    int afterPhrase = index + phrase.Length;
                    string remainder = afterPhrase < input.Length ? input.Substring(afterPhrase).Trim() : string.Empty;

                    string[] connectors = { "to ", "for ", "that ", "about ", "- ", ": " };

                    foreach (string c in connectors)
                    {
                        if (remainder.ToLower().StartsWith(c))
                        {
                            remainder = remainder.Substring(c.Length).Trim();
                            break;
                        }
                    }

                    //also trim off a trailing date phrase so it isn't part of the description
                    remainder = Regex.Replace(remainder, @"\b(tomorrow|today|next week|in \d+ days?)\b", "", RegexOptions.IgnoreCase).Trim();
                    remainder = remainder.TrimEnd('.', '!', '?', ' ');

                    return remainder;
                }
            }

            return input.Trim();

        }//end of method


        //method to detect a simple date phrase within text and convert it
        //into an actual DateTime. Returns null if no date phrase is found.
        public DateTime? extract_date(string input)
        {//start of method

            string text = input.ToLower();

            if (text.Contains("tomorrow"))
                return DateTime.Now.Date.AddDays(1);

            if (text.Contains("today"))
                return DateTime.Now.Date;

            if (text.Contains("next week"))
                return DateTime.Now.Date.AddDays(7);

            //matches phrases like "in 3 days" or "in 1 day"
            Match daysMatch = Regex.Match(text, @"in (\d+) days?");
            if (daysMatch.Success)
            {
                int days = int.Parse(daysMatch.Groups[1].Value);
                return DateTime.Now.Date.AddDays(days);
            }

            //try to find an explicit date written somewhere in the sentence,
            //e.g. "2026-07-01" or "01/07/2026"
            foreach (string word in input.Split(' ', ','))
            {
                DateTime parsed;
                if (word.Length >= 6 && DateTime.TryParse(word, out parsed))
                    return parsed;
            }

            return null;

        }//end of method

    }//end of class
}//end of namespace

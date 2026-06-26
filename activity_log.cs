using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace demo
{//start of namespace

    //class to record and retrieve a log of chatbot actions for each user.
    //Task 4 of the POE: tasks added, reminders set, quiz activity, and
    //NLP-recognised commands are all logged here with a timestamp.
    public class activity_log
    {//start of class

        private string filename = "activity_log.txt";
        private char separator = '|';


        //method to add a new entry to the activity log
        public void log_action(string username, string action)
        {
            string line = string.Join(separator.ToString(), DateTime.Now.ToString("o"), username, action);
            File.AppendAllText(filename, line + Environment.NewLine);
        }


        //method to get only the most recent entries for a user (newest first)
        public List<string> get_recent(string username, int count)
        {
            return get_all(username).Take(count).ToList();
        }


        //method to get the user's full, formatted history (newest first)
        public List<string> get_all(string username)
        {//start of method

            List<(DateTime date, string text)> entries = new List<(DateTime date, string text)>();

            if (!File.Exists(filename))
                return new List<string>();

            string[] lines = File.ReadAllLines(filename);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(separator);

                if (parts.Length < 3)
                    continue;

                if (parts[1].ToLower() != username.ToLower())
                    continue;

                DateTime date;
                if (!DateTime.TryParse(parts[0], out date))
                    continue;

                //re-join in case the action text itself contained the separator character
                string action = string.Join(separator.ToString(), parts.Skip(2));
                string formatted = action + " (" + date.ToString("dd MMM, HH:mm") + ")";

                entries.Add((date, formatted));
            }

            return entries.OrderByDescending(e => e.date)
                          .Select(e => e.text)
                          .ToList();

        }//end of method


        //method to count how many entries exist in total for a user
        public int count_for_user(string username)
        {
            return get_all(username).Count;
        }

    }//end of class
}//end of namespace

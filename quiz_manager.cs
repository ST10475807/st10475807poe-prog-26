using System;
using System.Collections.Generic;

namespace demo
{//start of namespace

    //class to represent a single quiz question (either multiple choice or true/false)
    public class QuizQuestion
    {
        public string QuestionText;
        public List<string> Options;   // e.g. ["A) ...","B) ...","C) ...","D) ..."] or ["True","False"]
        public int CorrectIndex;       // index into Options that holds the correct answer
        public string Explanation;     // brief explanation shown as feedback
        public bool IsTrueFalse;
    }


    //class to manage the Cybersecurity Mini-Game (Quiz)
    //Task 2 of the POE: prepares 10+ questions, mixes MCQ/True-False,
    //shows one question at a time, gives immediate feedback and tracks score
    public class quiz_manager
    {//start of class

        public List<QuizQuestion> questions = new List<QuizQuestion>();
        public int current_index = 0;
        public int score = 0;
        public bool quiz_active = false;

        public quiz_manager()
        {
            load_questions();
        }


        //method to load all the quiz questions into the list
        private void load_questions()
        {//start of method

            //1 - phishing (MCQ)
            questions.Add(new QuizQuestion
            {
                QuestionText = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectIndex = 2,
                Explanation = "Reporting phishing emails helps prevent scams and protects other users too.",
                IsTrueFalse = false
            });

            //2 - password safety (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: Using the same password for multiple accounts is a safe practice.",
                Options = new List<string> { "True", "False" },
                CorrectIndex = 1,
                Explanation = "Reusing passwords means one breached account can compromise all your other accounts.",
                IsTrueFalse = true
            });

            //3 - password safety (MCQ)
            questions.Add(new QuizQuestion
            {
                QuestionText = "Which of the following is the strongest password?",
                Options = new List<string> { "A) password123", "B) P@ssw0rd!", "C) Tr$8!qLv9#zR", "D) 123456" },
                CorrectIndex = 2,
                Explanation = "Long, random passwords mixing letters, numbers, and symbols are the hardest to crack.",
                IsTrueFalse = false
            });

            //4 - safe browsing (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: Public Wi-Fi networks are always safe for online banking.",
                Options = new List<string> { "True", "False" },
                CorrectIndex = 1,
                Explanation = "Public Wi-Fi can be intercepted by attackers. Use a VPN or mobile data for sensitive activity instead.",
                IsTrueFalse = true
            });

            //5 - social engineering (MCQ)
            questions.Add(new QuizQuestion
            {
                QuestionText = "What is 'social engineering' in cybersecurity?",
                Options = new List<string> { "A) A method of writing secure code", "B) Manipulating people into revealing confidential information", "C) A type of firewall", "D) A network protocol" },
                CorrectIndex = 1,
                Explanation = "Social engineering relies on tricking people rather than hacking systems directly.",
                IsTrueFalse = false
            });

            //6 - social engineering (MCQ)
            questions.Add(new QuizQuestion
            {
                QuestionText = "You receive a call from someone claiming to be from your bank's IT department asking for your PIN. What should you do?",
                Options = new List<string> { "A) Give them the PIN since they sound official", "B) Hang up and contact your bank directly using the official number", "C) Ask them to email you instead", "D) Give them half the PIN" },
                CorrectIndex = 1,
                Explanation = "Legitimate banks never ask for your PIN over the phone. Always verify using an official number.",
                IsTrueFalse = false
            });

            //7 - 2FA (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: Two-factor authentication (2FA) adds an extra layer of security beyond just a password.",
                Options = new List<string> { "True", "False" },
                CorrectIndex = 0,
                Explanation = "2FA requires a second form of verification, like an OTP, making accounts much harder to compromise.",
                IsTrueFalse = true
            });

            //8 - phishing (MCQ)
            questions.Add(new QuizQuestion
            {
                QuestionText = "Which of these is a common sign of a phishing email?",
                Options = new List<string> { "A) It's from a known colleague", "B) Urgent language demanding immediate action", "C) Properly spelled and formatted", "D) Sent during business hours" },
                CorrectIndex = 1,
                Explanation = "Phishing emails often create a false sense of urgency to pressure you into acting without thinking.",
                IsTrueFalse = false
            });

            //9 - safe browsing (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: It's safe to click links in text messages from unknown numbers offering prizes.",
                Options = new List<string> { "True", "False" },
                CorrectIndex = 1,
                Explanation = "This is a classic 'smishing' scam. Never click links from unknown senders, especially ones promising prizes.",
                IsTrueFalse = true
            });

            //10 - firewall (MCQ)
            questions.Add(new QuizQuestion
            {
                QuestionText = "What does a firewall primarily do?",
                Options = new List<string> { "A) Speeds up your internet", "B) Filters and monitors incoming/outgoing network traffic", "C) Stores your passwords", "D) Scans for viruses only" },
                CorrectIndex = 1,
                Explanation = "A firewall acts as a barrier, controlling traffic between trusted and untrusted networks.",
                IsTrueFalse = false
            });

            //11 - malware (MCQ)
            questions.Add(new QuizQuestion
            {
                QuestionText = "What is malware?",
                Options = new List<string> { "A) Software designed to damage or gain unauthorized access to a system", "B) A type of antivirus", "C) A secure password manager", "D) A firewall setting" },
                CorrectIndex = 0,
                Explanation = "Malware is any malicious software built to harm, exploit, or gain unauthorized access to devices or data.",
                IsTrueFalse = false
            });

            //12 - software updates (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: You should keep your software and operating system updated to patch security vulnerabilities.",
                Options = new List<string> { "True", "False" },
                CorrectIndex = 0,
                Explanation = "Updates often fix known security holes, so keeping software current reduces your risk of attack.",
                IsTrueFalse = true
            });

        }//end of method


        //method to start/restart the quiz
        public void start_quiz()
        {
            quiz_active = true;
            current_index = 0;
            score = 0;
        }


        //method to get the question currently being asked
        public QuizQuestion get_current_question()
        {
            if (current_index >= 0 && current_index < questions.Count)
                return questions[current_index];

            return null;
        }


        //method to mark the quiz as finished
        public bool is_finished()
        {
            return current_index >= questions.Count;
        }


        //method to submit the user's answer for the current question,
        //returns the explanation text and outputs whether it was correct.
        //Also advances on to the next question.
        public string submit_answer(string userAnswer, out bool correct)
        {//start of method

            QuizQuestion current = get_current_question();
            correct = false;

            if (current == null)
                return string.Empty;

            int chosenIndex = parse_answer(userAnswer, current);

            if (chosenIndex == current.CorrectIndex)
            {
                correct = true;
                score++;
            }

            current_index++;

            return current.Explanation;

        }//end of method


        //method to leniently turn free-form user text into an option index
        //accepts letters (a/b/c/d), the word true/false, or partial text matches
        private int parse_answer(string answer, QuizQuestion current)
        {//start of method

            string cleaned = answer == null ? string.Empty : answer.Trim().ToLower();

            if (current.IsTrueFalse)
            {
                if (cleaned.Contains("true") || cleaned == "t")
                    return 0;

                if (cleaned.Contains("false") || cleaned == "f")
                    return 1;

                return -1;
            }

            //accept a single starting letter such as "a", "b)", "c.", etc.
            if (cleaned.Length > 0)
            {
                char first = cleaned[0];

                if (first == 'a') return 0;
                if (first == 'b') return 1;
                if (first == 'c') return 2;
                if (first == 'd') return 3;
            }

            //fallback - try to match against the option text itself
            for (int i = 0; i < current.Options.Count; i++)
            {
                if (current.Options[i].ToLower().Contains(cleaned) && cleaned.Length > 2)
                    return i;
            }

            return -1;

        }//end of method


        //method to build the final score feedback message
        public string final_feedback()
        {//start of method

            quiz_active = false;

            double percent = questions.Count == 0 ? 0 : ((double)score / questions.Count) * 100;

            string message = "Quiz complete! Your final score is " + score + "/" + questions.Count + ". ";

            if (percent >= 80)
                message += "Great job! You're a cybersecurity pro!";
            else if (percent >= 50)
                message += "Good effort! Keep learning to stay safe online!";
            else
                message += "Keep learning to stay safe online - try the quiz again anytime!";

            return message;

        }//end of method

    }//end of class
}//end of namespace

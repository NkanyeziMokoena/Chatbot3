using System.Collections.Generic;

namespace Chatbot3
{//start of namespace
    public class Quiz_Question_Load
    {//start of class
        // Method responsible for loading all predefined quiz questions
        public void autoLoadQuiz(ref List<Question_in_quiz> questions)
        {
            // Create and populate the quiz question collection
            questions = new List<Question_in_quiz>()
        {
            new Question_in_quiz
            {
                Text = "What is phishing?",  // Stores the quiz question
                correctAnswer = "Tricking to steal data",  // Correct option for the question
                wrongAnswer = new List<string>{ "Data backup", "Safe login", "Password tips" }  // Incorrect answer options
            },//end of quiz question collection
            new Question_in_quiz
            {// start of quiz question collection
                Text = "What is password safety?",
                correctAnswer = "Unique & strong passwords",
                wrongAnswer = new List<string>{ "Share with friends", "Short passwords", "Common words" }
            },//end of quiz question collection
            new Question_in_quiz
            {// start of quiz question collection
                Text = "What is safe browsing?",
                correctAnswer = "Use trusted sites",
                wrongAnswer = new List<string>{ "Click all links", "Visit unknown pages", "Enable pop-ups" }
            },//end of quiz question collection
            new Question_in_quiz
            {// start of quiz question collection
                Text = "Phishing email sign?",
                correctAnswer = "Urgent or strange links",
                wrongAnswer = new List<string>{ "Good grammar", "Known sender", "Unsubscribe button" }
            },//end of quiz question collection 
            new Question_in_quiz
            {// start of quiz question collection
                Text = "Strong password?",
                correctAnswer = "P@55w0rD!#987",
                wrongAnswer = new List<string>{ "Password123", "qwerty2024", "123456789" }
            },//end of quiz question collection
            new Question_in_quiz
            {// start of quiz question collection
                Text = "When to update password?",
                correctAnswer = "Every 3–6 months",
                wrongAnswer = new List<string>{ "Yearly", "Never", "Only if hacked" }
            },//end of quiz question collection
            new Question_in_quiz
            {// start of quiz question collection
                Text = "Risk of reused passwords?",
                correctAnswer = "One hack = all at risk",
                wrongAnswer = new List<string>{ "Typing delay", "Site error", "No effect" }
            },//end of quiz question collection
            new Question_in_quiz
            {//start of quiz question collection
                Text = "Unsafe site sign?",
                correctAnswer = "Typos & pop-ups",
                wrongAnswer = new List<string>{ "HTTPS shown", "Fast load", "No ads" }
            },//    end of quiz question collection
            new Question_in_quiz
            {//start of quiz question collection
                Text = "Safe on public Wi-Fi?",
                correctAnswer = "Use VPN / avoid private info",
                wrongAnswer = new List<string>{ "Bank online", "File share", "Shop online" }
            },//end of quiz question collection
            new Question_in_quiz
            {//start of quiz question collection
                Text = "Flagged site action?",
                correctAnswer = "Leave right away",
                wrongAnswer = new List<string>{ "Ignore it", "Refresh page", "Click through" }
            },//end of quiz question collection
        };//end of quiz question collection
        }//end of autoLoadQuiz method
    }//end of class
}//end of namespace
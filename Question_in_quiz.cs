using System.Collections.Generic;

namespace Chatbot3
{
   public class Question_in_quiz
    {// start of class
        // Stores the question that will be displayed to the user
        public string Text { get; set; }

        // Holds the correct answer associated with the question
        public string correctAnswer { get; set; }

        // Contains the incorrect answer choices used in the quiz
        public List<string> wrongAnswer { get; set; }
    }//end of class
}//end of namespace
using System.Collections.Generic;

namespace Tests_SGMU.Core.Models
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect => UserAnswer == Options[CorrectAnswerIndex];
        public bool IsAnswered => !string.IsNullOrEmpty(UserAnswer);

        public Question()
        {
            Options = new List<string>();
        }
    }
}
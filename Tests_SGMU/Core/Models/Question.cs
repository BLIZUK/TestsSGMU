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

        public Question()
        {
            Options = new List<string>();
        }
    }

    public class Test
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public string FilePath { get; set; }

        public Test()
        {
            Questions = new List<Question>();
        }
    }
}
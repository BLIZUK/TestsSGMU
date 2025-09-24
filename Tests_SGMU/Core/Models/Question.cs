public class Question
{
    public string ?QuestionText { get; set; }
    public string[] ?Answers { get; set; } // Варианты ответов
    public int CorrectAnswerIndex { get; set; } // Индекс правильного ответа
}
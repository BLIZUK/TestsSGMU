using Tests_SGMU.Core.Interface;


namespace Tests_SGMU.Core.Models;
public class QuestionService : IQuestionService
{
    // Реализация загрузки из JSON файла
    public async Task<IEnumerable<Question>> LoadQuestionsAsync()
    {
        // Загрузка и десериализация вопросов из файла
        // Для примера вернем фиктивные данные
        return new List<Question>
        {
            new Question
            {
                QuestionText = "Пример вопроса?",
                Answers = new[] { "Ответ 1", "Ответ 2", "Ответ 3", "Ответ 4" },
                CorrectAnswerIndex = 0
            }
            // ... другие вопросы
        };
    }
}
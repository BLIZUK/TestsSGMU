using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tests_SGMU.Core.Interface;
using Tests_SGMU.Core.Models;


namespace Tests_SGMU.Core.Services
{
    public class QuestionsService : IQuestionsService
    {
        public List<Test> LoadAllTests()
        {
            var tests = new List<Test>();

            // Ищем все txt файлы в папке Data
            string dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");

            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            var txtFiles = Directory.GetFiles(dataFolder, "*.txt");

            foreach (var file in txtFiles)
            {
                try
                {
                    var test = ParseTestFromFile(file);
                    tests.Add(test);
                }
                catch (Exception ex)
                {
                    // Логируем ошибку, но продолжаем загрузку других файлов
                    System.Diagnostics.Debug.WriteLine($"Ошибка загрузки файла {file}: {ex.Message}");
                }
            }

            return tests;
        }

        public Test ParseTestFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            if (lines.Length < 4)
                throw new FormatException("Файл должен содержать как минимум 4 строки: название, вопрос, варианты ответов, правильный ответ");

            var test = new Test
            {
                Name = lines[0].Trim(),
                FilePath = filePath
            };

            int currentLine = 1; // Начинаем с вопроса (вторая строка)

            while (currentLine < lines.Length)
            {
                var question = new Question
                {
                    Text = lines[currentLine++].Trim()
                };

                // Читаем варианты ответов до тех пор, пока не встретим число
                while (currentLine < lines.Length && !int.TryParse(lines[currentLine], out _))
                {
                    question.Options.Add(lines[currentLine++].Trim());
                }

                if (question.Options.Count < 2)
                    throw new FormatException($"Вопрос должен содержать как минимум 2 варианта ответа: {question.Text}");

                // Читаем номер правильного ответа
                if (currentLine >= lines.Length || !int.TryParse(lines[currentLine], out int correctAnswer))
                    throw new FormatException($"Ожидался номер правильного ответа для вопроса: {question.Text}");

                // Проверяем, что номер правильного ответа в допустимом диапазоне
                if (correctAnswer < 1 || correctAnswer > question.Options.Count)
                    throw new FormatException($"Номер правильного ответа {correctAnswer} вне диапазона для вопроса: {question.Text}");

                question.CorrectAnswerIndex = correctAnswer - 1; // Конвертируем в 0-based индекс
                currentLine++;

                test.Questions.Add(question);

                // Если остались строки, предполагаем, что это следующий вопрос
                if (currentLine < lines.Length && string.IsNullOrWhiteSpace(lines[currentLine]))
                    currentLine++;
            }

            return test;
        }

        public List<Question> GetRandomQuestions(int count, List<Test> tests)
        {
            var allQuestions = tests.SelectMany(t => t.Questions).ToList();

            if (count >= allQuestions.Count)
                return allQuestions;

            var random = new Random();
            return allQuestions.OrderBy(x => random.Next()).Take(count).ToList();
        }
    }
}
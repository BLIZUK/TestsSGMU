using System.Collections.Generic;
using Tests_SGMU.Core.Models;


namespace Tests_SGMU.Core.Interface
{
    public interface IQuestionsService
    {
        List<Test> LoadAllTests();
        Test ParseTestFromFile(string filePath);
        List<Question> GetRandomQuestions(int count, List<Test> tests);
    }
}
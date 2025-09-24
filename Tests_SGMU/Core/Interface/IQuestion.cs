using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests_SGMU.Core.Interface
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> LoadQuestionsAsync();
    }
}

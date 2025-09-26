using System.Collections.Generic;
using System.IO;


namespace Tests_SGMU.Core.Models
{
    public class Test
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public string FilePath { get; set; }

        // Свойство для отображения короткого имени файла
        public string FileName => Path.GetFileName(FilePath);

        public Test()
        {
            Questions = new List<Question>();
        }
    }
}
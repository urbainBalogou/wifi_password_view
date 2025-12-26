using System.Collections.Generic;

namespace wifiCrack.Models
{
    public class EducationalContent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public EducationalCategory Category { get; set; }
        public List<string> KeyPoints { get; set; } = new();
        public string DetailedExplanation { get; set; }
    }

    public enum EducationalCategory
    {
        Protocols,
        Attacks,
        BestPractices,
        Tools,
        Legal
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
    }
}

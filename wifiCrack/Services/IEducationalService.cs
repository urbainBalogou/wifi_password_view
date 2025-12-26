using System.Collections.Generic;
using wifiCrack.Models;

namespace wifiCrack.Services
{
    public interface IEducationalService
    {
        List<EducationalContent> GetAllContent();
        EducationalContent GetContentByCategory(EducationalCategory category);
        List<QuizQuestion> GetQuizQuestions();
    }
}

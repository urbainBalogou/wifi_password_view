using System.Collections.ObjectModel;
using System.Linq;
using wifiCrack.Helpers;
using wifiCrack.Models;
using wifiCrack.Services;

namespace wifiCrack.ViewModels
{
    public class EducationalViewModel : ObservableObject
    {
        private readonly IEducationalService _educationalService;
        private EducationalContent _selectedContent;
        private ObservableCollection<QuizQuestion> _quizQuestions;
        private int _currentQuestionIndex;
        private int _score;

        public EducationalViewModel(IEducationalService educationalService)
        {
            _educationalService = educationalService;
            LoadContent();
        }

        public ObservableCollection<EducationalContent> AllContent { get; private set; }

        public EducationalContent SelectedContent
        {
            get => _selectedContent;
            set => SetProperty(ref _selectedContent, value);
        }

        public ObservableCollection<QuizQuestion> QuizQuestions
        {
            get => _quizQuestions;
            set => SetProperty(ref _quizQuestions, value);
        }

        public QuizQuestion CurrentQuestion =>
            QuizQuestions != null && CurrentQuestionIndex < QuizQuestions.Count
                ? QuizQuestions[CurrentQuestionIndex]
                : null;

        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex;
            set
            {
                SetProperty(ref _currentQuestionIndex, value);
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }

        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        private void LoadContent()
        {
            var content = _educationalService.GetAllContent();
            AllContent = new ObservableCollection<EducationalContent>(content);

            var questions = _educationalService.GetQuizQuestions();
            QuizQuestions = new ObservableCollection<QuizQuestion>(questions);

            CurrentQuestionIndex = 0;
            Score = 0;
        }

        public void AnswerQuestion(int selectedAnswerIndex)
        {
            if (CurrentQuestion != null && selectedAnswerIndex == CurrentQuestion.CorrectAnswerIndex)
            {
                Score++;
            }

            if (CurrentQuestionIndex < QuizQuestions.Count - 1)
            {
                CurrentQuestionIndex++;
            }
        }

        public void ResetQuiz()
        {
            CurrentQuestionIndex = 0;
            Score = 0;
        }
    }
}

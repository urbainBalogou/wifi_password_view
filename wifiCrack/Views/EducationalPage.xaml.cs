using Microsoft.Maui.Controls;
using wifiCrack.Models;
using wifiCrack.Services;
using wifiCrack.ViewModels;

namespace wifiCrack.Views
{
    public partial class EducationalPage : ContentPage
    {
        private readonly EducationalViewModel _viewModel;

        public EducationalPage()
        {
            InitializeComponent();

            var educationalService = new EducationalService();
            _viewModel = new EducationalViewModel(educationalService);

            BindingContext = _viewModel;
        }

        private void OnTutorialsClicked(object sender, System.EventArgs e)
        {
            TutorialsContent.IsVisible = true;
            QuizContent.IsVisible = false;

            TutorialsTab.BackgroundColor = (Color)Application.Current.Resources["Primary"];
            TutorialsTab.TextColor = Colors.White;

            QuizTab.BackgroundColor = (Color)Application.Current.Resources["Gray300"];
            QuizTab.TextColor = (Color)Application.Current.Resources["Gray900"];
        }

        private void OnQuizClicked(object sender, System.EventArgs e)
        {
            TutorialsContent.IsVisible = false;
            QuizContent.IsVisible = true;
            QuizResultFrame.IsVisible = false;

            QuizTab.BackgroundColor = (Color)Application.Current.Resources["Primary"];
            QuizTab.TextColor = Colors.White;

            TutorialsTab.BackgroundColor = (Color)Application.Current.Resources["Gray300"];
            TutorialsTab.TextColor = (Color)Application.Current.Resources["Gray900"];
        }

        private async void OnContentTapped(object sender, System.EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is EducationalContent content)
            {
                await Navigation.PushAsync(new ContentDetailPage(content));
            }
        }

        private async void OnReadMoreClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.BindingContext is EducationalContent content)
            {
                await Navigation.PushAsync(new ContentDetailPage(content));
            }
        }

        private async void OnAnswerClicked(object sender, System.EventArgs e)
        {
            if (sender is Button button && button.Text is string answer)
            {
                var question = _viewModel.CurrentQuestion;
                var selectedIndex = question.Options.IndexOf(answer);

                var isCorrect = selectedIndex == question.CorrectAnswerIndex;

                // Afficher le feedback
                var feedbackColor = isCorrect ? "Success" : "Danger";
                button.BackgroundColor = (Color)Application.Current.Resources[feedbackColor];

                await System.Threading.Tasks.Task.Delay(500);

                // Afficher l'explication
                await DisplayAlert(
                    isCorrect ? "✅ Correct !" : "❌ Incorrect",
                    question.Explanation,
                    "Continuer");

                // Mettre à jour le score et passer à la question suivante
                _viewModel.AnswerQuestion(selectedIndex);

                // Vérifier si c'était la dernière question
                if (_viewModel.CurrentQuestionIndex >= _viewModel.QuizQuestions.Count - 1)
                {
                    ShowQuizResult();
                }
            }
        }

        private void ShowQuizResult()
        {
            QuizResultFrame.IsVisible = true;

            var score = _viewModel.Score;
            var total = _viewModel.QuizQuestions.Count;
            var percentage = (double)score / total * 100;

            string message;
            if (percentage >= 80)
                message = $"Excellent ! Vous avez obtenu {score}/{total} ({percentage:F0}%)";
            else if (percentage >= 60)
                message = $"Bien ! Vous avez obtenu {score}/{total} ({percentage:F0}%)";
            else
                message = $"Vous avez obtenu {score}/{total} ({percentage:F0}%). Continuez à apprendre !";

            ResultLabel.Text = message;
        }

        private void OnRestartQuizClicked(object sender, System.EventArgs e)
        {
            _viewModel.ResetQuiz();
            QuizResultFrame.IsVisible = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XAML.Helpers;
using XAML.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace XAML.Views
{
    public sealed partial class QuestionView : UserControl
    {
        public event EventHandler GameEnded;

        public List<Category> Categories { get; set; }
        private int _numberOfQuestions;
        private List<Question> _questions;
        private List<Question> _completedQuestions;
        private List<Question> _inCorrectAnswers;
        private List<Question> _correctAnswers;

        public int CorrectAnswers
        {
            get
            {
                return _correctAnswers.Count();
            }
        }
        public int NumberOfQuestions
        {
            get
            {
                return _numberOfQuestions;
            }
        }
        public QuestionView()
        {
            this.InitializeComponent();
        }

        public void CreateGame(List<Question> questions)
        {
            _numberOfQuestions = questions.Count();
            _questions = questions;
            _inCorrectAnswers = new List<Question>();
            _correctAnswers = new List<Question>();
            _completedQuestions = new List<Question>();
        }

        public void StartGame()
        {
            GetNextQuestionAsync();

                QuestionViewHeader.Text = "The game has started!";
        }

        private async Task GetNextQuestionAsync()
        {
            if (_questions.Count == 0)
            {
                //var message = new MessageDialog($"Game over!\nCorrect answers: {_correctAnswers.Count()} of questions: {_numberOfQuestions}");
                //_ = await message.ShowAsync();

                GameEnded?.Invoke(this, EventArgs.Empty);

                return;
            }

            _completedQuestions.Add(_questions.First());
            UpdateProgress();
            var question = _questions.First();

            var answers = question.RandomAnswers;

            QTextBlock.Text = HttpUtility.HtmlDecode(question.TheQuestion);

            SetAnswerButton(RedButton, answers[0], question.CorrectAnswer);

            //Om det finns fler svar på en fråga
            if (question.IncorrectAnswers.Count == 3)
            {
                BlueButton.Visibility = Visibility.Visible;
                YellowButton.Visibility = Visibility.Visible;

                SetAnswerButton(GreenButton, answers[1], HttpUtility.HtmlDecode(question.CorrectAnswer));
                SetAnswerButton(YellowButton, answers[2], HttpUtility.HtmlDecode(question.CorrectAnswer));
                SetAnswerButton(BlueButton, answers[3], HttpUtility.HtmlDecode(question.CorrectAnswer));
            }

            //True or False frågor
            if (question.IncorrectAnswers.Count == 1)
            {
                BlueButton.Visibility = Visibility.Collapsed;
                YellowButton.Visibility = Visibility.Collapsed;

                SetAnswerButton(GreenButton, answers[1], question.CorrectAnswer);
            }
        }

        private void SetAnswerButton(Button button, string answer, string correctAnswer)
        {
            //Städar bort föregående klick event
            button.Click -= Button_Click_Correct;
            button.Click -= Button_Click_Incorrect;
            button.Content = answer;
            if (answer == correctAnswer)
                button.Click += Button_Click_Correct;

            else
                button.Click += Button_Click_Incorrect;
        }



        private void UpdateProgress()
        {
            score.Text = $"{_completedQuestions.Count()} / {_numberOfQuestions}";
        }

        private async void Button_Click_Correct(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            //if (button != null)
            //{
            //    //var message = new MessageDialog(" You click on the " + button.Content);
            //    var message = new MessageDialog("Correct answer!");
            //    await message.ShowAsync();
            //}
            _correctAnswers.Add(_questions.First());
            // tar bort en fråga ifrån listan
            if (_questions.Count > 0)
            {
                _questions.Remove(_questions.First());
            }
            //Hämtar en ny fråga om man klickat på rätt svar
            await GetNextQuestionAsync();
        }

        private async void Button_Click_Incorrect(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button != null)
            {
                //var message = new MessageDialog(" You click on the " + button.Content);
                //var message = new MessageDialog("Wrong answer!");
                //await message.ShowAsync();
                _inCorrectAnswers.Add(_questions.First());
                // tar bort en fråga ifrån listan
                if (_questions.Count > 0)
                {
                    _questions.Remove(_questions.First());
                }
                //Hämtar en ny fråga om man klickat på rätt svar
                await GetNextQuestionAsync();
            }

        }
    }
}

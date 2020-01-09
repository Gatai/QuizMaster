using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XAML.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using XAML.Models;
using System.Threading.Tasks;
using System.Web;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XAML
{
    public sealed partial class MainPage : Page
    {
        //synlig i denna klassen 
        private string _difficulty;
        private string _type;
        private List<Question> _questions = new List<Question>();
        public List<Category> Categories { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            //Hämta en fråga ifrån API
            //detta startat direkt när programmet är igång
            GetCategories();
        }

        private async void Button_Click_Correct(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button != null)
            {
                //var message = new MessageDialog(" You click on the " + button.Content);
                var message = new MessageDialog("Correct answer!");
                await message.ShowAsync();
            }
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
                var message = new MessageDialog("Wrong answer!");
                await message.ShowAsync();
            }
        }

        private async Task GetNextQuestionAsync()
        {
            if (_questions.Count == 0)
            {
                var message = new MessageDialog("No more questions!");
                _ = await message.ShowAsync();
                return;
            }

            var question = _questions.First();

            var answers = question.RandomAnswers;

            QTextBlock.Text = HttpUtility.HtmlDecode(question.TheQuestion);

            SetAnswerButton(RedButton, answers[0], question.CorrectAnswer);

            //Om det finns 3 fel svar då vissa blå och gul knapparna
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

        private void GetCategories()
        {
            var task = Task.Run(() => ApiHelper.GetCategories());
            task.Wait();
            Categories = task.Result;
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

        private void SetLevel(object sender, RoutedEventArgs e)
        {
            _difficulty = (string)((RadioButton)sender).CommandParameter;
        }

        private void SetType(object sender, RoutedEventArgs e)
        {
            _type = (string)((RadioButton)sender).CommandParameter;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            StartButton.IsEnabled = true;
        }

        private async void StartGameAsync(object sender, RoutedEventArgs e)
        {

            StartButton.IsEnabled = false;

            var selectedCategory = CategoryComboBox.SelectedItem as Category;
            _questions = await ApiHelper.GetQuestions(selectedCategory.Id, _difficulty, _type, Convert.ToInt32(amountBox.Text));
            await GetNextQuestionAsync();
        }
    }
}

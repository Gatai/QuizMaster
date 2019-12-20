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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XAML
{
    public sealed partial class MainPage : Page
    {
        //synlig i denna klassen 
        private string _difficulty;
        public List<Category> Categories { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            //Hämta en fråga ifrån API
            GetCategories();
            GetNextQuestion();

        }

        //private async void eventClick(object sender, RoutedEventArgs e)
        //{

        //}

        private async void Button_Click_Correct(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button != null)
            {
                //var message = new MessageDialog(" You click on the " + button.Content);
                var message = new MessageDialog("Correct answer!");
                await message.ShowAsync();
            }
            //Hämtar en ny fråga om man klickat på rätt svar
            GetNextQuestion();
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

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            var textBlock = checkbox.CommandParameter as TextBlock;

            textBlock.Text = "Checkbox checked";

            //if (checkbox.Name == "cb1") text1.Text = "Two-state CheckBox checked.";
            //else text2.Text = "Three-state CheckBox checked.";
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            var textBlock = checkbox.CommandParameter as TextBlock;

            textBlock.Text = "Unchecked";
            //if (checkbox.Name == "cb1") text1.Text = "Two-state CheckBox unchecked.";
            //else text2.Text = "Three-state CheckBox checked.";
        }

        //private async void GetAllFood_Click(object sender, RoutedEventArgs e)
        //{
        //    //GetNextQuestion();
        //}

        private async void GetNextQuestion()
        {
            //Hämta första frågan
            var questions = await ApiHelper.GetQuestions(9, _difficulty);
            var question = questions.First();

            var answers = question.RandomAnswers;

            QTextBlock.Text = question.TheQuestion;

            SetAnswerButton(RedButton, answers[0], question.CorrectAnswer);

            //Om det finns 3 fel svar då vissa blå och gul knapparna
            if (question.IncorrectAnswers.Count == 3)
            {
                BlueButton.Visibility = Visibility.Visible;
                YellowButton.Visibility = Visibility.Visible;

                SetAnswerButton(GreenButton, answers[1], question.CorrectAnswer);
                SetAnswerButton(YellowButton, answers[2], question.CorrectAnswer);
                SetAnswerButton(BlueButton, answers[3], question.CorrectAnswer);

            }

            //True or False frågor
            if (question.IncorrectAnswers.Count == 1)
            {
                BlueButton.Visibility = Visibility.Collapsed;
                YellowButton.Visibility = Visibility.Collapsed;

                SetAnswerButton(GreenButton, answers[1], question.CorrectAnswer);
            }
        }

        private  void GetCategories()
        {
            var task = Task.Run(() => ApiHelper.GetCategories());
            task.Wait();
            Categories = task.Result;            

            ////var category = categorys.First();

            //var comobox = categorys.Add as ComboBox;
            ////Vill foreach alla category som finns och add to listitembox som com
            //foreach (var item in categorys)
            //{
            //    //ListItemBox1.Content = item.CategoryName;
            //}
            //ListItemBox1.Content = category.CategoryName;
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
    }
}

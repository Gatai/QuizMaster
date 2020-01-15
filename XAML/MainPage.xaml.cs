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
using XAML.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XAML
{
    public sealed partial class MainPage : Page
    {   
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void StartGameView_GameStarted(object sender, EventArgs e)
        {
            var startGame = ((StartGameView)sender);

            var category = startGame.SelectedCategory().Id; var dif = startGame.Difficulty;var typ = startGame.Type;var count = startGame.NumberOfQuestions;

            var task = Task.Run(() => ApiHelper.GetQuestions(category,dif,typ,count));
            task.Wait();
            // var questions = await ApiHelper.GetQuestions(startGame.SelectedCategory().Id, startGame.Difficulty, startGame.Type, startGame.NumberOfQuestions);
            QuestionView.CreateGame(task.Result);
            QuestionView.StartGame();

            //var message = new MessageDialog("The user started the game, selected category " + startGame.SelectedCategory().CategoryName);
            //_ = message.ShowAsync();
            StartGameView.Visibility = Visibility.Collapsed;
            QuestionView.Visibility = Visibility.Visible;
        }

        private void QuestionView_GameEnded(object sender, EventArgs e)
        {
            GameOverView.Visibility = Visibility.Visible;
            QuestionView.Visibility = Visibility.Collapsed;

           var count = QuestionView.NumberOfQuestions;
            var correctAnswer = QuestionView.CorrectAnswers;
            GameOverView.GameEnd(correctAnswer, count);
        }
    }
}

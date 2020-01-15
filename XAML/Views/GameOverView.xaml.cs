using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace XAML.Views
{
    public sealed partial class GameOverView : UserControl
    {
        public event EventHandler NewGame;

        public GameOverView()
        {
            this.InitializeComponent();
        }

        public void GameEnd(int correctAnswers, int numberOfQuestions)
        {
            score.Text = $"Correct answers: {correctAnswers} of questions: {numberOfQuestions}";
            Text.Text = $"Game over!";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using XAML.Models;
using Newtonsoft.Json; 
using XAML.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XAML
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string _difficulty;
        public MainPage()
        {
            this.InitializeComponent();
            //Här kan något hämta ifrån API:et.
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
                var message = new MessageDialog("RÄTT");
                await message.ShowAsync();
            }
        }

        private async void Button_Click_Incorrect(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button != null)
            {
                //var message = new MessageDialog(" You click on the " + button.Content);
                var message = new MessageDialog("FEL");
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

        private async void GetAllFood_Click(object sender, RoutedEventArgs e)
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

            //om det finns 1 fel fråga då hide blå och gul
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

        private void SetLevel(object sender, RoutedEventArgs e)
        {            
            _difficulty = (string)((RadioButton)sender).CommandParameter;
        }
    }
}
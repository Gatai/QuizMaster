using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class StartGameView : UserControl
    {
        public event EventHandler GameStarted;

        public string Difficulty { get; set; }
        public string Type { get; set; }
        public int NumberOfQuestions
        {
            get 
            { 
                return Convert.ToInt32(amountBox.Text); 
            }
        }
        public List<Category> Categories { get; set; }

        public StartGameView()
        {
            this.InitializeComponent();
            GetCategories();
        }
        private void GetCategories()
        {
            var task = Task.Run(() => ApiHelper.GetCategories());
            task.Wait();
            Categories = task.Result;
        }
        public Category SelectedCategory()
        {
            return CategoryComboBox.SelectedItem as Category;
        }

        private void SetLevel(object sender, RoutedEventArgs e)
        {
            Difficulty = (string)((RadioButton)sender).CommandParameter;
        }

        private void SetType(object sender, RoutedEventArgs e)
        {
            Type = (string)((RadioButton)sender).CommandParameter;
        }

        private void CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            StartButton.IsEnabled = true;

        }

        private void StartGameClick(object sender, RoutedEventArgs e)
        {

            var selectedCategory = CategoryComboBox.SelectedItem as Category;

            GameStarted?.Invoke(this, EventArgs.Empty);

            //rootFrame.Navigate(typeof(StartGameView), e.Arguments);
            //await GetNextQuestionAsync();
        }
    }
}

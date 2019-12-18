using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAML.Extensions;

namespace XAML.Models
{
    public class QuestionRoot
    {
        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }

        [JsonProperty("results")]
        public Question[] Questions { get; set; }
    }

    public class Question
    {
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }
        [JsonProperty("question")]
        public string TheQuestion { get; set; }
        [JsonProperty("correct_answer")]
        public string CorrectAnswer { get; set; }
        [JsonProperty("incorrect_answers")]
        public List<string> IncorrectAnswers { get; set; }

        public List<string> RandomAnswers
        {
            get
            {
                var answers = new List<string>();
                answers.Add(CorrectAnswer);
                answers.AddRange(IncorrectAnswers);
                //anropar ListExtenstions metoden
                answers.Shuffle();
                return answers;
            }

        }
    }
}

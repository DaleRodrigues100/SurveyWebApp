using System.ComponentModel.DataAnnotations;

namespace XSLearning.Models
{
    public class Responses
    {

        [Key]
        public string Username { get; set; }
        [Key]
        public int SurveyId { get; set; }
        [Key]
        public int QuestionId { get; set; }
        [Key]
        public int OptionId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace XSLearning.Models
{
    public class Surveys
    {
       
        [Key]
        public int SurveyId { get; set; }
        [Key]
        public int QuestionId { get; set; }
        [Key]
        public int OptionId { get; set; }

        public string Question { get; set; }
        public string OptionList { get; set; }

        
        public string SurveyTitle { get; set; }
        public string Status { get; set; }
    }
}

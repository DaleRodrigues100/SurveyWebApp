using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XSLearning.Models
{
    public class Surveys
    {
        [Key, Column(Order = 0)]
        public int SurveyId { get; set; }
        
        [Key, Column(Order = 1)]
        public int QuestionId { get; set; }
        
        [Key, Column(Order = 2)]
        public int OptionId { get; set; }

        [Required]
        public string Question { get; set; }
        
        [Required]
        public string OptionList { get; set; }
        
        [Required]
        public string SurveyTitle { get; set; }
        
        [Required]
        public string Status { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ModifiedAt { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XSLearning.Models
{
    public class Responses
    {
        [Key, Column(Order = 0)]
        public string Username { get; set; }
        
        [Key, Column(Order = 1)]
        public int SurveyId { get; set; }
        
        [Key, Column(Order = 2)]
        public int QuestionId { get; set; }
        
        [Key, Column(Order = 3)]
        public int OptionId { get; set; }
        
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties for better EF relationships
        [ForeignKey("Username")]
        public virtual Users User { get; set; }
    }
}

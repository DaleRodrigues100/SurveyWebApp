using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace XSLearning.DTOs
{
    public class SurveyDto
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string Status { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
    }

    public class OptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
    }

    public class CreateSurveyDto
    {
        [Required]
        public string SurveyTitle { get; set; }
        
        [Required]
        public List<CreateQuestionDto> Questions { get; set; } = new List<CreateQuestionDto>();
    }

    public class CreateQuestionDto
    {
        [Required]
        public string Question { get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "At least two options are required")]
        public List<string> Options { get; set; } = new List<string>();
    }
}

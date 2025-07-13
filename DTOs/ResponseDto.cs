using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace XSLearning.DTOs
{
    public class ResponseDto
    {
        public string Username { get; set; }
        public int SurveyId { get; set; }
        public List<QuestionResponseDto> QuestionResponses { get; set; } = new List<QuestionResponseDto>();
    }

    public class QuestionResponseDto
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
    }

    public class SubmitResponseDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public int SurveyId { get; set; }
        
        [Required]
        public List<QuestionResponseDto> Answers { get; set; } = new List<QuestionResponseDto>();
    }
    
    public class SurveyResultDto
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public List<QuestionResultDto> QuestionResults { get; set; } = new List<QuestionResultDto>();
    }
    
    public class QuestionResultDto
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<OptionResultDto> OptionResults { get; set; } = new List<OptionResultDto>();
    }
    
    public class OptionResultDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
}

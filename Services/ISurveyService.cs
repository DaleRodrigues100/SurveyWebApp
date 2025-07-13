using System.Collections.Generic;
using System.Threading.Tasks;
using XSLearning.DTOs;

namespace XSLearning.Services
{
    public interface ISurveyService
    {
        Task<IEnumerable<SurveyDto>> GetAllSurveysAsync();
        Task<SurveyDto> GetSurveyByIdAsync(int surveyId);
        Task<SurveyDto> CreateSurveyAsync(CreateSurveyDto createSurveyDto, bool publish);
        Task<bool> DeleteSurveyAsync(int surveyId);
        Task<bool> UpdateSurveyStatusAsync(int surveyId, string status);
        Task<SurveyResultDto> GetSurveyResultsAsync(int surveyId);
    }
}

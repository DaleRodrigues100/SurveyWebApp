using System.Collections.Generic;
using System.Threading.Tasks;
using XSLearning.DTOs;

namespace XSLearning.Services
{
    public interface IResponseService
    {
        Task<bool> SubmitResponseAsync(SubmitResponseDto responseDto);
        Task<bool> HasUserRespondedAsync(string username, int surveyId);
        Task<IEnumerable<ResponseDto>> GetResponsesBySurveyAsync(int surveyId);
        Task<IEnumerable<OptionResultDto>> GetQuestionResultsAsync(int surveyId, int questionId);
    }
}

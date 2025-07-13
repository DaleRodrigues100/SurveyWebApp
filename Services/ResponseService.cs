using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XSLearning.DTOs;
using XSLearning.Models;
using XSLearning.Repositories;

namespace XSLearning.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResponseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SubmitResponseAsync(SubmitResponseDto responseDto)
        {
            try
            {
                // Check if survey exists
                if (!await _unitOfWork.Surveys.SurveyExistsAsync(responseDto.SurveyId))
                    return false;

                // Check if user exists
                if (!await _unitOfWork.Users.UserExistsAsync(responseDto.Username))
                    return false;

                // Check if user has already responded to this survey
                if (await _unitOfWork.Responses.UserHasRespondedAsync(responseDto.Username, responseDto.SurveyId))
                    return false;

                var responseEntities = new List<Responses>();

                foreach (var answer in responseDto.Answers)
                {
                    var response = new Responses
                    {
                        Username = responseDto.Username,
                        SurveyId = responseDto.SurveyId,
                        QuestionId = answer.QuestionId,
                        OptionId = answer.OptionId,
                        SubmittedAt = DateTime.UtcNow
                    };

                    responseEntities.Add(response);
                }

                await _unitOfWork.Responses.AddRangeAsync(responseEntities);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> HasUserRespondedAsync(string username, int surveyId)
        {
            return await _unitOfWork.Responses.UserHasRespondedAsync(username, surveyId);
        }

        public async Task<IEnumerable<ResponseDto>> GetResponsesBySurveyAsync(int surveyId)
        {
            var responses = await _unitOfWork.Responses.GetResponsesBySurveyIdAsync(surveyId);
            
            return responses
                .GroupBy(r => r.Username)
                .Select(g => new ResponseDto
                {
                    Username = g.Key,
                    SurveyId = surveyId,
                    QuestionResponses = g.Select(r => new QuestionResponseDto
                    {
                        QuestionId = r.QuestionId,
                        OptionId = r.OptionId
                    }).ToList()
                });
        }

        public async Task<IEnumerable<OptionResultDto>> GetQuestionResultsAsync(int surveyId, int questionId)
        {
            var responses = await _unitOfWork.Responses.GetResponsesForQuestionAsync(surveyId, questionId);
            var survey = await _unitOfWork.Surveys.GetSurveysByIdAsync(surveyId);
            
            var options = survey
                .Where(s => s.QuestionId == questionId)
                .OrderBy(s => s.OptionId)
                .Select(s => new 
                { 
                    OptionId = s.OptionId,
                    OptionText = s.OptionList
                })
                .ToList();
                
            var totalResponses = responses.Select(r => r.Username).Distinct().Count();
            var results = options.Select(o => 
            {
                var optionCount = responses.Count(r => r.OptionId == o.OptionId);
                var percentage = totalResponses > 0 ? (double)optionCount / totalResponses * 100 : 0;
                
                return new OptionResultDto
                {
                    OptionId = o.OptionId,
                    OptionText = o.OptionText,
                    Count = optionCount,
                    Percentage = Math.Round(percentage, 2)
                };
            });
            
            return results;
        }
    }
}

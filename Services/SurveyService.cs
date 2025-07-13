using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XSLearning.DTOs;
using XSLearning.Models;
using XSLearning.Repositories;

namespace XSLearning.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SurveyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SurveyDto>> GetAllSurveysAsync()
        {
            var surveys = await _unitOfWork.Surveys.GetAllAsync();
            var distinctSurveys = surveys
                .GroupBy(s => s.SurveyId)
                .Select(g => g.First())
                .ToList();

            var surveyDtos = new List<SurveyDto>();
            
            foreach (var survey in distinctSurveys)
            {
                var surveyDto = await GetSurveyByIdAsync(survey.SurveyId);
                surveyDtos.Add(surveyDto);
            }

            return surveyDtos;
        }

        public async Task<SurveyDto> GetSurveyByIdAsync(int surveyId)
        {
            var surveyItems = (await _unitOfWork.Surveys.GetSurveysByIdAsync(surveyId)).ToList();
            
            if (!surveyItems.Any())
                return null;

            var surveyDto = new SurveyDto
            {
                SurveyId = surveyId,
                SurveyTitle = surveyItems.First().SurveyTitle,
                Status = surveyItems.First().Status
            };

            var questionGroups = surveyItems
                .GroupBy(s => s.QuestionId)
                .OrderBy(g => g.Key);

            foreach (var questionGroup in questionGroups)
            {
                var firstQuestion = questionGroup.First();
                var questionDto = new QuestionDto
                {
                    QuestionId = firstQuestion.QuestionId,
                    Question = firstQuestion.Question,
                    Options = questionGroup
                        .OrderBy(q => q.OptionId)
                        .Select(q => new OptionDto
                        {
                            OptionId = q.OptionId,
                            OptionText = q.OptionList
                        }).ToList()
                };
                
                surveyDto.Questions.Add(questionDto);
            }

            return surveyDto;
        }

        public async Task<SurveyDto> CreateSurveyAsync(CreateSurveyDto createSurveyDto, bool publish)
        {
            var nextSurveyId = await _unitOfWork.Surveys.GetNextSurveyIdAsync();
            var status = publish ? "Completed" : "In Progress";
            var surveyEntities = new List<Surveys>();

            for (int questionIndex = 0; questionIndex < createSurveyDto.Questions.Count; questionIndex++)
            {
                var question = createSurveyDto.Questions[questionIndex];
                var questionId = questionIndex + 1;

                for (int optionIndex = 0; optionIndex < question.Options.Count; optionIndex++)
                {
                    var optionText = question.Options[optionIndex];
                    var optionId = optionIndex + 1;

                    var surveyEntity = new Surveys
                    {
                        SurveyId = nextSurveyId,
                        QuestionId = questionId,
                        OptionId = optionId,
                        Question = question.Question,
                        OptionList = optionText,
                        SurveyTitle = createSurveyDto.SurveyTitle,
                        Status = status,
                        CreatedAt = DateTime.UtcNow
                    };

                    surveyEntities.Add(surveyEntity);
                }
            }

            await _unitOfWork.Surveys.AddRangeAsync(surveyEntities);
            await _unitOfWork.CompleteAsync();

            return await GetSurveyByIdAsync(nextSurveyId);
        }

        public async Task<bool> DeleteSurveyAsync(int surveyId)
        {
            var surveyItems = await _unitOfWork.Surveys.GetSurveysByIdAsync(surveyId);
            if (!surveyItems.Any())
                return false;

            _unitOfWork.Surveys.RemoveRange(surveyItems);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateSurveyStatusAsync(int surveyId, string status)
        {
            var surveyItems = (await _unitOfWork.Surveys.GetSurveysByIdAsync(surveyId)).ToList();
            if (!surveyItems.Any())
                return false;

            foreach (var item in surveyItems)
            {
                item.Status = status;
                item.ModifiedAt = DateTime.UtcNow;
                _unitOfWork.Surveys.Update(item);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<SurveyResultDto> GetSurveyResultsAsync(int surveyId)
        {
            var survey = await GetSurveyByIdAsync(surveyId);
            if (survey == null)
                return null;

            var responses = await _unitOfWork.Responses.GetResponsesBySurveyIdAsync(surveyId);
            var surveyItems = await _unitOfWork.Surveys.GetSurveysByIdAsync(surveyId);

            var result = new SurveyResultDto
            {
                SurveyId = survey.SurveyId,
                SurveyTitle = survey.SurveyTitle,
                QuestionResults = new List<QuestionResultDto>()
            };

            foreach (var question in survey.Questions)
            {
                var questionResult = new QuestionResultDto
                {
                    QuestionId = question.QuestionId,
                    Question = question.Question,
                    OptionResults = new List<OptionResultDto>()
                };

                var questionResponses = responses.Where(r => r.QuestionId == question.QuestionId).ToList();
                var totalResponses = questionResponses.Select(r => r.Username).Distinct().Count();

                foreach (var option in question.Options)
                {
                    var optionResponses = questionResponses.Count(r => r.OptionId == option.OptionId);
                    var percentage = totalResponses > 0 ? (double)optionResponses / totalResponses * 100 : 0;
                    
                    var optionResult = new OptionResultDto
                    {
                        OptionId = option.OptionId,
                        OptionText = option.OptionText,
                        Count = optionResponses,
                        Percentage = Math.Round(percentage, 2)
                    };

                    questionResult.OptionResults.Add(optionResult);
                }

                result.QuestionResults.Add(questionResult);
            }

            return result;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XSLearning.DTOs;
using XSLearning.Services;

namespace XSLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IResponseService _responseService;

        public UserController(ISurveyService surveyService, IResponseService responseService)
        {
            _surveyService = surveyService;
            _responseService = responseService;
        }

        /// <summary>
        /// Gets all available surveys for users to participate in
        /// </summary>
        /// <returns>List of surveys with their status</returns>
        [HttpGet("surveys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SurveyDto>>> GetAvailableSurveys()
        {
            var surveys = await _surveyService.GetAllSurveysAsync();
            return Ok(surveys);
        }

        /// <summary>
        /// Gets a specific survey by ID
        /// </summary>
        /// <param name="surveyId">Survey ID to retrieve</param>
        /// <returns>Survey details with questions and options</returns>
        [HttpGet("surveys/{surveyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SurveyDto>> GetSurvey(int surveyId)
        {
            var survey = await _surveyService.GetSurveyByIdAsync(surveyId);
            if (survey == null)
                return NotFound(new { message = $"Survey with ID {surveyId} not found" });

            return Ok(survey);
        }

        /// <summary>
        /// Submits a user's response to a survey
        /// </summary>
        /// <param name="responseDto">User's survey responses</param>
        /// <returns>Status of the submission</returns>
        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SubmitResponse(SubmitResponseDto responseDto)
        {
            var result = await _responseService.SubmitResponseAsync(responseDto);
            if (!result)
                return BadRequest(new { message = "Failed to submit response. Survey may not exist, or you may have already responded." });

            return Ok(new { message = "Survey response submitted successfully" });
        }

        /// <summary>
        /// Checks if a user has already responded to a specific survey
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <param name="surveyId">Survey ID to check</param>
        /// <returns>Boolean indicating if user has already responded</returns>
        [HttpGet("{username}/{surveyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckResponded(string username, int surveyId)
        {
            var hasResponded = await _responseService.HasUserRespondedAsync(username, surveyId);
            return Ok(hasResponded);
        }
    }
}

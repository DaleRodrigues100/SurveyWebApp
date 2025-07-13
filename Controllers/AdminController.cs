using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XSLearning.DTOs;
using XSLearning.Services;

namespace XSLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISurveyService _surveyService;
        private readonly IResponseService _responseService;

        public AdminController(IUserService userService, ISurveyService surveyService, IResponseService responseService)
        {
            _userService = userService;
            _surveyService = surveyService;
            _responseService = responseService;
        }
        //USERS **************************************

        /// <summary>
        /// Returns list of all users for admin management
        /// </summary>
        /// <returns>List of user information</returns>
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="username">Username to retrieve</param>
        /// <returns>User information</returns>
        [HttpGet("users/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetUserInfo(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound(new { message = $"User '{username}' not found" });

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user account
        /// </summary>
        /// <param name="userDto">User registration details</param>
        /// <returns>Created user information</returns>
        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDto>> CreateUser(RegisterUserDto userDto)
        {
            var user = await _userService.RegisterAsync(userDto);
            if (user == null)
                return BadRequest(new { message = "Username already exists" });

            return CreatedAtAction(nameof(GetUserInfo), new { username = user.Username }, user);
        }

        /// <summary>
        /// Updates user account information
        /// </summary>
        /// <param name="username">Username to update</param>
        /// <param name="userDto">Updated user information</param>
        /// <returns>Updated user information</returns>
        [HttpPut("users/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(string username, RegisterUserDto userDto)
        {
            var user = await _userService.UpdateUserAsync(username, userDto);
            if (user == null)
                return NotFound(new { message = $"User '{username}' not found" });

            return Ok(user);
        }

        /// <summary>
        /// Deletes a user account
        /// </summary>
        /// <param name="username">Username to delete</param>
        /// <returns>Status of deletion</returns>
        [HttpDelete("users/{username}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var result = await _userService.DeleteUserAsync(username);
            if (!result)
                return NotFound(new { message = $"User '{username}' not found" });

            return NoContent();
        }

        // SURVEYS *******************************

        /// <summary>
        /// Gets all surveys for admin management
        /// </summary>
        /// <returns>List of all surveys</returns>
        [HttpGet("surveys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SurveyDto>>> GetSurveys()
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
        /// Creates a new survey in draft mode (In Progress)
        /// </summary>
        /// <param name="surveyDto">Survey creation details</param>
        /// <returns>Created survey information</returns>
        [HttpPost("surveys/draft")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SurveyDto>> CreateSurveyDraft(CreateSurveyDto surveyDto)
        {
            var survey = await _surveyService.CreateSurveyAsync(surveyDto, false);
            return CreatedAtAction(nameof(GetSurvey), new { surveyId = survey.SurveyId }, survey);
        }

        /// <summary>
        /// Creates and publishes a survey (Completed status)
        /// </summary>
        /// <param name="surveyDto">Survey creation details</param>
        /// <returns>Created survey information</returns>
        [HttpPost("surveys/publish")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SurveyDto>> PublishSurvey(CreateSurveyDto surveyDto)
        {
            var survey = await _surveyService.CreateSurveyAsync(surveyDto, true);
            return CreatedAtAction(nameof(GetSurvey), new { surveyId = survey.SurveyId }, survey);
        }

        /// <summary>
        /// Updates an existing survey's status
        /// </summary>
        /// <param name="surveyId">Survey ID to update</param>
        /// <param name="status">New status ("In Progress" or "Completed")</param>
        /// <returns>Status of the operation</returns>
        [HttpPut("surveys/{surveyId}/status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateSurveyStatus(int surveyId, string status)
        {
            if (status != "In Progress" && status != "Completed")
                return BadRequest(new { message = "Status must be 'In Progress' or 'Completed'" });

            var result = await _surveyService.UpdateSurveyStatusAsync(surveyId, status);
            if (!result)
                return NotFound(new { message = $"Survey with ID {surveyId} not found" });

            return Ok(new { message = $"Survey status updated to {status}" });
        }

        /// <summary>
        /// Deletes a survey and all its questions and options
        /// </summary>
        /// <param name="surveyId">Survey ID to delete</param>
        /// <returns>Status of the deletion</returns>
        [HttpDelete("surveys/{surveyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSurvey(int surveyId)
        {
            var result = await _surveyService.DeleteSurveyAsync(surveyId);
            if (!result)
                return NotFound(new { message = $"Survey with ID {surveyId} not found" });

            return NoContent();
        }

        /// <summary>
        /// Gets survey results for a specific survey
        /// </summary>
        /// <param name="surveyId">Survey ID to get results for</param>
        /// <returns>Detailed survey results with statistics</returns>
        [HttpGet("surveys/{surveyId}/results")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SurveyResultDto>> GetSurveyResults(int surveyId)
        {
            var results = await _surveyService.GetSurveyResultsAsync(surveyId);
            if (results == null)
                return NotFound(new { message = $"Survey with ID {surveyId} not found" });

            return Ok(results);
        }

        /// <summary>
        /// Gets results for a specific question in a survey
        /// </summary>
        /// <param name="surveyId">Survey ID</param>
        /// <param name="questionId">Question ID within the survey</param>
        /// <returns>Question results with option statistics</returns>
        [HttpGet("surveys/{surveyId}/questions/{questionId}/results")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OptionResultDto>>> GetQuestionResults(int surveyId, int questionId)
        {
            var results = await _responseService.GetQuestionResultsAsync(surveyId, questionId);
            return Ok(results);
        }

        /// <summary>
        /// Gets all responses for a specific survey
        /// </summary>
        /// <param name="surveyId">Survey ID to get responses for</param>
        /// <returns>All user responses for the survey</returns>
        [HttpGet("surveys/{surveyId}/responses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResponseDto>>> GetSurveyResponses(int surveyId)
        {
            var responses = await _responseService.GetResponsesBySurveyAsync(surveyId);
            return Ok(responses);
        }
    }
}

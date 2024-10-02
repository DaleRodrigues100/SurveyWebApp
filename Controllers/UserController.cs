using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XSLearning.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace XSLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    
    {
        private readonly DataContext dc;
        private readonly SqlConnection connection;
        public UserController(DataContext dc, IConfiguration configuration)
        {
            this.dc = dc;
            
            this.connection = new SqlConnection(configuration.GetConnectionString("DevConnection"));
        }

      
        // For list of surveys tables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Surveys>>> GetSurveysnStatus()
        {
            var surveys = await this.dc.Surveys.ToListAsync();
            var status = await this.dc.Responses.ToListAsync();
            List<object> arr = new List<object>();
            arr.Add(surveys);
            arr.Add(status);

            return Ok(arr);
        }

        // Get the survey the user selected


        // User submit survey
   
        [HttpPost]
        public async Task<ActionResult<Surveys>> SubmitResponse(Responses response)
        {
            this.dc.Responses.Add(response);
            await this.dc.SaveChangesAsync();
            return Ok(response);

        }


        [HttpGet("{username}/{surveyId}")]
        public ActionResult<bool> CheckResponded(string username, int surveyId)
        {
            this.connection.Open();
            
            string q = "SELECT * FROM Responses WHERE SurveyId LIKE @Number AND Username Like @String";
            SqlCommand com = new SqlCommand(q, this.connection);

            com.Parameters.AddWithValue("@Number", surveyId);
            com.Parameters.AddWithValue("@String", username);

            SqlDataReader reader = com.ExecuteReader();
            List<Responses> list = new List<Responses>();

            return reader.Read();
        }





    }

   
        

        
    }


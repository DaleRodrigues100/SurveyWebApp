using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using XSLearning.Models;

namespace XSLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly DataContext dc;
        private readonly SqlConnection connection;

        public AdminController(DataContext dc, IConfiguration configuration)
        {
            
            this.dc = dc;
            this.connection = new SqlConnection(configuration.GetConnectionString("DevConnection"));
            
        }
        //USERS **************************************

        //Returns list of users. Use this method for populating users table(Test Takers)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await this.dc.Users.ToListAsync();
        }


        // Pass the username of the user and get there username and password
        [HttpGet("{name}")]
        public async Task<ActionResult<Users>> GetUserInfo(string name)
        {
            var user = await this.dc.Users.FindAsync(name);
            if (user != null)
            {

                return Ok(user);

            }
            return BadRequest();
           
            
        }

        // Add user to the table (Add New Survey Taker)
        [HttpPost]
        public async Task<ActionResult<Users>> PostPaymentDetail(Users user)
        {
            this.dc.Users.Add(user);
            
            await this.dc.SaveChangesAsync();

            return Ok(user);
        }

        // Edit user method
        [HttpPut("edit")]
        public async Task<ActionResult> EditUser(Users user)
        {
            this.dc.Users.Update(user);
            await this.dc.SaveChangesAsync();
            return Ok(user);
        }

        // Delete user method
        [HttpDelete("{user}")]
        public async Task<ActionResult> DeleteUser(string user)
        {
            var userfound = await this.dc.Users.FindAsync(user);
            if (userfound != null)
            {
                this.dc.Users.Remove(userfound);
                await this.dc.SaveChangesAsync();
                return Ok();
            }
            return NotFound();

        }

        // SURVEYS *******************************
        [HttpGet("survey")]
        public async Task<ActionResult<IEnumerable<Surveys>>> GetSurveys()
        {
            return await this.dc.Surveys.ToListAsync();
        }

        
        [HttpGet("survey/{surveyId}")]
        public async Task<ActionResult<Surveys>> getSurveyWId(int surveyId)
        {

            this.connection.Open();
            string q = "SELECT * FROM Surveys WHERE SurveyId LIKE @Number";
            SqlCommand com = new SqlCommand(q, this.connection);
            com.CommandTimeout = 0;
            com.Parameters.AddWithValue("@Number", surveyId);
            
            SqlDataReader reader = com.ExecuteReader();
            List<Surveys> list = new List<Surveys>();

            while (reader.Read())
            {
                Surveys s = new Surveys();
                s.SurveyId = int.Parse( reader["SurveyId"].ToString());
                s.Question = reader["Question"].ToString();
                s.QuestionId = (int)reader["QuestionId"];
                s.OptionList = reader["OptionList"].ToString();
                s.SurveyTitle = reader["SurveyTitle"].ToString();
                s.SurveyId = (int)reader["SurveyId"];
                s.OptionId = (int)reader["OptionId"];

                list.Add(s);
    
            }

            reader.Close();

            this.connection.Close();
            return Ok(list);
        }

        [HttpGet("results/{surveyId}")]
        public async Task<ActionResult<IEnumerable<Responses>>> GetResult(int surveyId)

        {
            this.connection.Open();
            

            string q = "SELECT * FROM Responses WHERE SurveyId LIKE @Number";
            SqlCommand com = new SqlCommand(q, this.connection);

            com.Parameters.AddWithValue("@Number", surveyId);
            com.CommandTimeout = 0;

            SqlDataReader reader = com.ExecuteReader();
            List<Responses> list = new List<Responses>();

            while (reader.Read())
            {
                Responses r = new Responses();
                r.SurveyId = (int)reader["SurveyId"];
                r.OptionId = (int)reader["OptionId"];
                r.QuestionId = (int)reader["QuestionId"];
                r.Username = (string)reader["Username"];

                list.Add(r);
            }

            reader.Close();

            this.connection.Close();
            return Ok(list);
        }

        // [HttpGet("results/charts/{surveyId}/{questionId}")]
        // public async Task<ActionResult<IEnumerable<Surveys>>> GetOptionsByQuestion(int surveyId, int questionId){

        //     this.connection.Open();

        //     string q = "SELECT OptionList, OptionId FROM Surveys WHERE SurveyId LIKE @SurveyId AND QuestionId LIKE @QuestionId";
        //     SqlCommand com = new SqlCommand(q, this.connection);
        //     com.Parameters.AddWithValue("@SurveyId", surveyId);
        //     com.Parameters.AddWithValue("@QuestionId", questionId);

        //     SqlDataReader reader = com.ExecuteReader();
        //     List<Surveys> list = new List<Surveys>();

        //     while (reader.Read())
        //     {
        //         Surveys r = new Surveys();
        //         r.OptionList = (string)reader["OptionList"];
        //         r.OptionId = (int)reader["OptionId"];
        //         list.Add(r);
        //     }
        //     return Ok(list);


        // }

        [HttpGet("results/charts/{surveyId}/{questionId}")]
        public async Task<ActionResult<IEnumerable<Responses>>> GetResultByQuestion(int surveyId, int questionId){

            this.connection.Open();

            string q = "SELECT COUNT ( OptionId ) AS 'numberUsers',QuestionId, OptionId FROM Responses WHERE SurveyId = @SurveyId AND QuestionId = @QuestionId GROUP BY QuestionId,OptionId";
            SqlCommand com = new SqlCommand(q, this.connection);
            com.CommandTimeout = 0;
            com.Parameters.AddWithValue("@SurveyId", surveyId);
            com.Parameters.AddWithValue("@QuestionId", questionId);

            SqlDataReader reader = com.ExecuteReader();
            List<Surveys> list = new List<Surveys>();

            while (reader.Read())
            {
                Surveys r = new Surveys();
                r.SurveyId = (int)reader["numberUsers"];
                r.OptionId = (int)reader["OptionId"];
                r.QuestionId = (int)reader["QuestionId"];

                list.Add(r);
            }

            reader.Close();


            this.connection.Close();
            return Ok(list);


        }


        [HttpGet("results/{surveyId}/{optionId}/{questionId}")]
        public async Task<ActionResult<IEnumerable<Surveys>>> GetResult(int surveyId, int optionId, int questionId)

        {
            this.connection.Open();


            string q = "SELECT * FROM Surveys WHERE SurveyId LIKE @SurveyId AND OptionId LIKE @OptionId AND QuestionId LIKE @QuestionId";
            SqlCommand com = new SqlCommand(q, this.connection);

            com.Parameters.AddWithValue("@SurveyId", surveyId);
            com.Parameters.AddWithValue("@OptionId", optionId);
            com.Parameters.AddWithValue("@QuestionId", questionId);
            com.CommandTimeout = 0;

            SqlDataReader reader = com.ExecuteReader();
            List<Surveys> list = new List<Surveys>();

            while (reader.Read())
            {
                Surveys r = new Surveys();
                r.SurveyId = (int)reader["SurveyId"];
                r.SurveyTitle = (string)reader["SurveyTitle"];
                r.OptionId = (int)reader["OptionId"];
                r.OptionList = (string)reader["OptionList"];
                r.QuestionId = (int)reader["QuestionId"];
                r.Question = (string)reader["Question"];

                list.Add(r);
            }

            reader.Close();

            this.connection.Close();
            return Ok(list);
        }


        [HttpPost("survey/editPublish/{surid}")]
        public async Task<ActionResult<Surveys>> SurveyPublish(Object[] survey, string surId)
        {
            
            
            //var hi2 = CreateSurveyPublish(survey);
            var hi = DeleteSurvey(int.Parse(surId));
            //var exists = await this.dc.Surveys.FindAsync(survey.SurveyId);
            //exists.Status = "Complete";
            //this.dc.Surveys.Update(survey);
            return Ok();
            
        }

        [HttpPost("survey/createSurveySave")]
        public async Task<ActionResult> CreateSurveySave( Object[] survey)
        {
            Console.WriteLine(survey);
            int number = 0;

           

            SqlCommand command = new SqlCommand("SELECT  ISNULL(MAX(SurveyId),0) AS surveyNumber FROM Surveys", this.connection);
            command.CommandTimeout = 0;




            Surveys surveyObj = new Surveys();
            this.connection.Open();
            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                if (reader["surveyNumber"] != null)
                {

                    number = int.Parse(reader["surveyNumber"].ToString());

                    surveyObj.SurveyId = number + 1;

                }
                else
                {

                    surveyObj.SurveyId = 1;
                }
            }
            reader.Close();


            for (int i = 0; i < survey.Length; i++)
            {
                JObject json = JObject.Parse(survey[i].ToString());

                surveyObj.SurveyTitle = json.GetValue("surveyTitle").ToString();
                surveyObj.Status = "In Progress";
                surveyObj.QuestionId = i + 1;
                surveyObj.Question = json.GetValue("question").ToString();

                var options = json.GetValue("options");
                for (int j = 0; j < options.Count(); j++)
                {
                    surveyObj.OptionId = j + 1;
                    surveyObj.OptionList = options[j].ToString();


                    this.dc.Surveys.Add(surveyObj);
                    await this.dc.SaveChangesAsync();

                  
                }


            }

         

            this.connection.Close();


            return Ok(survey);
        }
        [HttpPost("survey/createSurveyPublish")]
        public async Task<ActionResult> CreateSurveyPublish(Object[] survey){

            Console.WriteLine(survey);
            int number = 0;
            
           

            SqlCommand command = new SqlCommand("SELECT  ISNULL(MAX(SurveyId),0) AS surveyNumber FROM Surveys", this.connection);
            command.CommandTimeout = 0;



            Surveys surveyObj = new Surveys();
            this.connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                if (reader["surveyNumber"] != null)
                {

                    number = int.Parse(reader["surveyNumber"].ToString());
                   


                    surveyObj.SurveyId = number + 1;

                }
                else
                {

                    surveyObj.SurveyId = 1;
                }
            }
            reader.Close();



            for (int i = 0; i < survey.Length; i++)
            {
                JObject json = JObject.Parse(survey[i].ToString());
                
                surveyObj.SurveyTitle = json.GetValue("surveyTitle").ToString();
                surveyObj.Status = "Completed";
                surveyObj.QuestionId = i + 1;
                surveyObj.Question = json.GetValue("question").ToString();

                var options = json.GetValue("options");
                for (int j = 0; j < options.Count(); j ++)
                {
                    surveyObj.OptionId = j + 1;
                    surveyObj.OptionList = options[j].ToString();

                    //SqlCommand c3 = new SqlCommand("SET IDENTITY_INSERT dbo.Surveys ON", this.connection);

                    //SqlCommand c2 = new SqlCommand("INSERT INTO dbo.Surveys (SurveyId, QuestionId, OptionId, Question, OptionList, SurveyTitle, Status) VALUES(2, 3, 4, '??', 'DSD','ffdfdfd', 'Completed')",this.connection);

                    //SqlCommand c2 = new SqlCommand("SET IDENTITY_INSERT Surveys ON", this.connection);
                    //c3.ExecuteNonQuery();
                  

                    this.dc.Surveys.Add(surveyObj);
                    await this.dc.SaveChangesAsync();

                    //SqlCommand c3 = new SqlCommand("SET IDENTITY_INSERT Surveys OFF", this.connection);


                    //c3.ExecuteNonQueryAsync();
                }
                
            

            }

            //Surveys s = new Surveys();
            //s.SurveyId = 4;
            //s.QuestionId = 2;
            //s.OptionId = 3;
            //s.Question = "hi";
            //s.Option = "dog";
            //s.SurveyTitle = "helloo";
            //s.Status = "In progress";

            //this.dc.Surveys.Add(s);

           // await this.dc.SaveChangesAsync();






            this.connection.Close();


            //var exists = await this.dc.Surveys.FindAsync(survey.SurveyId);
            //exists.Status = "Complete";
            //this.dc.Surveys.Add(survey);
            return Ok(survey);
        }


        [HttpDelete("survey/{surveyId}")]
        public async Task<ActionResult<Surveys>> DeleteSurvey(int surveyId)
        {
            
            this.connection.Open();
            Console.WriteLine(surveyId);
            string q = "DELETE FROM Surveys WHERE SurveyId LIKE @Number";
            SqlCommand com = new SqlCommand(q, this.connection);
            com.CommandTimeout = 0;

            com.Parameters.Add(new SqlParameter("@Number", surveyId));
            int result = com.ExecuteNonQuery();
            
            if (result <= 0)
            {
                this.connection.Close();
                return BadRequest();
            }
            else
            {
                this.connection.Close();
                return Ok();
            }

            
        }    






    }
}

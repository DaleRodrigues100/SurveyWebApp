using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XSLearning.Models;

namespace XSLearning.Repositories
{
    public class ResponseRepository : Repository<Responses>, IResponseRepository
    {
        public ResponseRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Responses>> GetResponsesBySurveyIdAsync(int surveyId)
        {
            return await _context.Responses
                .Where(r => r.SurveyId == surveyId)
                .OrderBy(r => r.QuestionId)
                .ThenBy(r => r.OptionId)
                .ToListAsync();
        }

        public async Task<bool> UserHasRespondedAsync(string username, int surveyId)
        {
            return await _context.Responses
                .AnyAsync(r => r.Username == username && r.SurveyId == surveyId);
        }

        public async Task<IEnumerable<Responses>> GetResponsesForQuestionAsync(int surveyId, int questionId)
        {
            return await _context.Responses
                .Where(r => r.SurveyId == surveyId && r.QuestionId == questionId)
                .ToListAsync();
        }
    }
}

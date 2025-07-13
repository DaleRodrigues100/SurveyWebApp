using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XSLearning.Models;

namespace XSLearning.Repositories
{
    public class SurveyRepository : Repository<Surveys>, ISurveyRepository
    {
        public SurveyRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Surveys>> GetSurveysByIdAsync(int surveyId)
        {
            return await _context.Surveys
                .Where(s => s.SurveyId == surveyId)
                .OrderBy(s => s.QuestionId)
                .ThenBy(s => s.OptionId)
                .ToListAsync();
        }

        public async Task<int> GetNextSurveyIdAsync()
        {
            var maxId = await _context.Surveys
                .Select(s => (int?)s.SurveyId)
                .MaxAsync() ?? 0;
            
            return maxId + 1;
        }

        public async Task<bool> SurveyExistsAsync(int surveyId)
        {
            return await _context.Surveys.AnyAsync(s => s.SurveyId == surveyId);
        }
    }
}

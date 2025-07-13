using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace XSLearning.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(params object[] id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }

    public interface IUserRepository : IRepository<XSLearning.Models.Users>
    {
        Task<Models.Users> GetUserByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
    }

    public interface ISurveyRepository : IRepository<XSLearning.Models.Surveys>
    {
        Task<IEnumerable<Models.Surveys>> GetSurveysByIdAsync(int surveyId);
        Task<int> GetNextSurveyIdAsync();
        Task<bool> SurveyExistsAsync(int surveyId);
    }

    public interface IResponseRepository : IRepository<XSLearning.Models.Responses>
    {
        Task<IEnumerable<Models.Responses>> GetResponsesBySurveyIdAsync(int surveyId);
        Task<bool> UserHasRespondedAsync(string username, int surveyId);
        Task<IEnumerable<Models.Responses>> GetResponsesForQuestionAsync(int surveyId, int questionId);
    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ISurveyRepository Surveys { get; }
        IResponseRepository Responses { get; }
        Task<int> CompleteAsync();
    }
}

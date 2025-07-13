using System;
using System.Threading.Tasks;
using XSLearning.Models;

namespace XSLearning.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IUserRepository _userRepository;
        private ISurveyRepository _surveyRepository;
        private IResponseRepository _responseRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);

        public ISurveyRepository Surveys => _surveyRepository ??= new SurveyRepository(_context);

        public IResponseRepository Responses => _responseRepository ??= new ResponseRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

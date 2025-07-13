using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using XSLearning.Models;

namespace XSLearning.Repositories
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
    }
}

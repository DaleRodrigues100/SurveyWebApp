using Microsoft.EntityFrameworkCore;

namespace XSLearning.Models
{
    public class DataContext: DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Surveys>().HasKey(table => new {
                table.SurveyId,
                table.QuestionId,
                table.OptionId
            });

            builder.Entity<Responses>().HasKey(table => new {
                table.Username,
                table.SurveyId,
                table.QuestionId,
                table.OptionId
            });
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Surveys> Surveys { get; set; }
        public DbSet<Responses> Responses { get; set; }
    }

    
}

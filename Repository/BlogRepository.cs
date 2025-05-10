using MusicLove.Models;

namespace MusicLove.Data.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly AppDbContext dbContext;
        public BlogRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(Blog blog)
        {
            dbContext.Blogs.Update(blog);
        }
    }
}
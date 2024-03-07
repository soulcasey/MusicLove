using MusicLove.Models;

namespace MusicLove.Data.Repository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        public void Update(Blog blog);
        public void Save();
    }
}
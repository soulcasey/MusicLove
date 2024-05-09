using MusicLove.Models;

namespace MusicLove.Data.Repository
{
    public interface IImageRepository : IRepository<Image>
    {
        public void Update(Image image);
        public void Save();
    }
}
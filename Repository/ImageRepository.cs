using MusicLove.Models;

namespace MusicLove.Data.Repository
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly AppDbContext dbContext;
        public ImageRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(Image image)
        {
            dbContext.Images.Update(image);
        }
    }
}
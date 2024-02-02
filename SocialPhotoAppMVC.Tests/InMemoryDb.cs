using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialPhotoAppMVC.Tests
{
    public class InMemoryDb
    {
        public static async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            var dbContext = new ApplicationDbContext(options);

            dbContext.Database.EnsureCreated();

            for (int i = 1; i < 4; i++)
            {
                dbContext.Users.Add(
                new AppUser
                {
                    Id = $"{i}",
                    NickName = $"Nick{i}"
                });

                dbContext.Albums.Add(
                new Album
                {
                    Title = $"TestAlbumName{i}",
                    Description = $"TestAlbumDescription{i}",
                    User = dbContext.AppUsers.Find($"{i}"),
                });

                dbContext.Photos.Add(
                new Photo
                {
                    Title = $"TestPhotoName{i}",
                    Description = $"TestPhotoDescription{i}",
                    User = dbContext.AppUsers.Find($"{i}"),
                    Category = Enums.Category.Animals
                });

            }
            await dbContext.SaveChangesAsync();

            return dbContext;
        }
    }
}

using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess
{
    public class ImageManager : IImageManager
    {
        private readonly ApplicationDbContext db;

        public ImageManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        public async Task Add(ImageInfo info)
        {
            db.Entry(info).State = EntityState.Added;
            await db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var img = await Get(id);
            db.Entry(img).State = EntityState.Deleted;
            await db.SaveChangesAsync();
        }

        public async Task<ImageInfo> Get(string id)
        {
            return await db.Images.FindAsync(id);
        }
    }
}

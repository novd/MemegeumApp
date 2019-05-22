using System;
using memegeumApp.Models;
using Microsoft.EntityFrameworkCore;

namespace memegeumApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
            //todo db logic with sqlite
        }

        public DbSet<Meme> Memes { get; set; }
    }
}

using System;
using Microsoft.EntityFrameworkCore;

namespace memegeumApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
            //todo db logic with sqlite
        }
    }
}

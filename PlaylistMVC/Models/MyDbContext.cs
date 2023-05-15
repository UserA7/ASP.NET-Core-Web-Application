using Microsoft.EntityFrameworkCore;

namespace PlaylistMVC.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        { }
        public DbSet<SongInfo> SongInfo { get; set; }
    }
}

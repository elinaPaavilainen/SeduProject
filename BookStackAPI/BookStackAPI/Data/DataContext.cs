using BookStackAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStackAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
        }
        public DbSet<Books> Books { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Deleted_books> Deleted_books { get; set; }   
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { // ID to autoincrement primary key
            modelBuilder.Entity<Books>()
                .HasKey(b => b.Id); 
            modelBuilder.Entity<Books>()
                .Property(b => b.Id).ValueGeneratedOnAdd(); 
            modelBuilder.Entity<Users>()
                .HasKey(u => u.Id); 
            modelBuilder.Entity<Users>()
                .Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Deleted_books>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Deleted_books>()
                .Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}

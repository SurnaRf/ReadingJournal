using BusinessLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public class ReadingJournalDbContext : IdentityDbContext<User>
	{
		public ReadingJournalDbContext()
		{

		}

		public ReadingJournalDbContext(DbContextOptions contextOptions) : base(contextOptions)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=DESKTOP-F3IKLD2;Database=ReadingJournalDb;Trusted_Connection=True;");
				optionsBuilder.EnableSensitiveDataLogging();

            }

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Edition>().Property(e => e.CoverType).HasConversion<string>();
			modelBuilder.Entity<Shelf>().Property(sh => sh.ShelfPurpose).HasConversion<string>();
            modelBuilder.Entity<Shelf>()
           .HasOne(sh => sh.User)
           .WithMany(u => u.Shelves)
           .HasForeignKey(s => s.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany()
                .HasForeignKey(fr => fr.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.Property(e => e.RequestId).ValueGeneratedOnAdd();
            });


            base.OnModelCreating(modelBuilder);
		}

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Edition> Editions { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Shelf> Shelves { get; set; }

		public DbSet<FriendRequest> FriendRequests { get; set; }
    }
}

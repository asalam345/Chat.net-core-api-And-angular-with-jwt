using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models
{
	public partial class SignalRChatContext : DbContext
	{
		public SignalRChatContext()
		{
		}

		public SignalRChatContext(DbContextOptions<SignalRChatContext> options)
			: base(options)
		{
		}

		public virtual DbSet<UserChat> UserChat { get; set; }
		public virtual DbSet<User> UserLogin { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
				optionsBuilder.UseSqlServer("Server=.;Database=Chat;user id=sa;password=sa123");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserChat>(entity =>
			{
				entity.HasKey(e => e.ChatId);

				entity.HasIndex(e => new { e.SenderId, e.ReciverId })
					.HasName("NonClusteredIndex-20200419-114105");

				entity.Property(e => e.ChatId).ValueGeneratedNever();

				entity.Property(e => e.date).HasColumnType("datetime");

				entity.Property(e => e.Message).HasMaxLength(1000);

				entity.Property(e => e.ReciverId).HasMaxLength(50);

				entity.Property(e => e.SenderId).HasMaxLength(50);
			});

			modelBuilder.Entity<User>(entity =>
			{
				//entity.HasNoKey();

				entity.Property(e => e.UserId).HasColumnName("UserID");

				entity.Property(e => e.FirstName).HasMaxLength(50);

				entity.Property(e => e.LastName).HasMaxLength(50);
				entity.Property(e => e.Email).HasMaxLength(50);
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}

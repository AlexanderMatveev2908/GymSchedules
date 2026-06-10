using Server.ModelsNS.UserNS;
using Microsoft.EntityFrameworkCore;
using Server.ModelsNS.RefreshTokensNS;
using Server.ModelsNS.ThumbNS;

namespace Server.ConfigNS.SqlNS;

public class SqlDbCtx : DbContext
{
  public SqlDbCtx(DbContextOptions<SqlDbCtx> options) : base(options)
  {
  }

  public DbSet<User> User => Set<User>();
  public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>();
  public DbSet<Thumbnail> Thumbnail => Set<Thumbnail>();

  protected override void OnModelCreating(
    ModelBuilder modelBuilder
)
  {
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
  }
}
using Server.ModelsNS.UsersNS;
using Microsoft.EntityFrameworkCore;
using Server.ModelsNS.RefreshTokensNS;

namespace Server.ConfigNS.SqlNS;

public class SqlDbCtx : DbContext
{
  public SqlDbCtx(DbContextOptions<SqlDbCtx> options) : base(options)
  {
  }

  public DbSet<Users> Users => Set<Users>();
  public DbSet<RefreshTokens> RefreshTokens => Set<RefreshTokens>();

  protected override void OnModelCreating(
    ModelBuilder modelBuilder
)
  {
    modelBuilder.Entity<Users>()
        .HasIndex(u => u.Email)
        .IsUnique();
  }
}
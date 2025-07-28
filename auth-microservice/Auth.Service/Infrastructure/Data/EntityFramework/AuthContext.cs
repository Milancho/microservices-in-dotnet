using Auth.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Service.Infrastructure.Data.EntityFramework;

public class AuthContext : DbContext, IAuthStore
{
    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    public async Task<User?> VerifyUserLogin(string username, string password) => await Users.FirstOrDefaultAsync(u => 
        u.Username == username &&
        u.Password == password);
}

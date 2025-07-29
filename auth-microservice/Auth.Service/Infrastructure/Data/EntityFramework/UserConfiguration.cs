using Auth.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Service.Infrastructure.Data.EntityFramework;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired();

        builder.Property(u => u.Password)
            .IsRequired();

        builder.HasData(
            new User
            {
                Id = new Guid("E197A36F-4E70-4BE8-B7B6-68F79C9E50C9"),
                Username = "microservices@code-maze.com",
                Password = "oKNrqkO7iC#G"
            });
    }
}
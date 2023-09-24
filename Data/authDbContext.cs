using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Data;

public class authDbContext : IdentityDbContext
{
    public authDbContext(DbContextOptions<authDbContext> options): base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        var readerRoleId = "7157b81e-fe20-4835-b45e-ce86d11c265c";
        var writerRoleId = "7ea6461e-fddb-4503-913c-2ff916ec59fd";
        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper()
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            },
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}
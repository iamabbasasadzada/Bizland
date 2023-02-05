using BizLand.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizLand.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Features> Features { get; set; }

    }
}

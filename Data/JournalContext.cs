using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Journal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Journal.Data
{
    public class JournalContext : IdentityDbContext<IdentityUser>
    {
        public JournalContext (DbContextOptions<JournalContext> options)
            : base(options)
        {
        }

        public DbSet<Journal.Models.Note> Note { get; set; } = default!;
        public DbSet<Journal.Models.ImageAsset> ImageAsset { get; set; } = default!;
    }
}


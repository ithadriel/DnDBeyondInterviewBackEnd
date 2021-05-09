using ddb_back_end_developer_challenge.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Persistence
{
    public class CharacterDbContext : DbContext
    {
        public CharacterDbContext(DbContextOptions<CharacterDbContext> options) : base(options)
        {

        }

        public DbSet<DomainCharacter> DomainCharacters { get; set; }
    }
}

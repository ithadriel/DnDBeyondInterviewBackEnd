using ddb_back_end_developer_challenge.Adapters.Rest.Models;
using ddb_back_end_developer_challenge.Adapters.Utilities;
using ddb_back_end_developer_challenge.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Persistence
{
    public class CharacterDbContext : DbContext
    {
        public CharacterDbContext(DbContextOptions<CharacterDbContext> options) : base(options)
        {

        }

        public CharacterDbContext()
        {
            // this constructor used for Moq testing
        }

        public virtual DbSet<DomainCharacter> DomainCharacters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DomainCharacter>()
                .Property(c => c.Resistances)
                .HasConversion(
                    resistances => string.Join(",", resistances.ToArray()),
                    resistances => resistances.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList<string>(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList())
                );
            modelBuilder.Entity<DomainCharacter>()
                .Property(c => c.Vulnerabilities)
                .HasConversion(
                    vulnerabilities => string.Join(",", vulnerabilities.ToArray()),
                    vulnerabilities => vulnerabilities.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList<string>(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList())
                );
            modelBuilder.Entity<DomainCharacter>()
                .Property(c => c.Immunities)
                .HasConversion(
                    immunities => string.Join(",", immunities.ToArray()),
                    immunities => immunities.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList<string>(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList())
                );
        }
    }
}

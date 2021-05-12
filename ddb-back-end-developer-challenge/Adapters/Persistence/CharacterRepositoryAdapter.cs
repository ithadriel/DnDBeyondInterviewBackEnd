using ddb_back_end_developer_challenge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Persistence
{
    public class CharacterRepositoryAdapter : ICharacterRepositoryAdapter
    {
        private CharacterDbContext dbContext;
        public CharacterRepositoryAdapter(CharacterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<DomainCharacter> GetAllCharacters()
        {
            return dbContext.DomainCharacters.ToList();
        }

        public DomainCharacter GetCharacter(long id)
        {
            return dbContext.Find<DomainCharacter>(id);
        }

        public DomainCharacter SaveCharacter(DomainCharacter character)
        {
            dbContext.Add(character);
            dbContext.SaveChanges();
            return character;
        }

        public DomainCharacter UpdateCharacter(DomainCharacter characterToUpdate)
        {
            dbContext.Update(characterToUpdate);
            dbContext.SaveChanges();
            return characterToUpdate;
        }
    }
}

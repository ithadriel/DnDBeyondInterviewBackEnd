using ddb_back_end_developer_challenge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Persistence
{
    public class CharacterRepositoryAdapter : ICharacterRepositoryAdapter
    {
        public CharacterRepositoryAdapter()
        {
        }

        public DomainCharacter GetCharacter(long id)
        {
            throw new NotImplementedException();
        }

        public void SaveeCharacter(DomainCharacter character)
        {
            throw new NotImplementedException();
        }

        public void UpdateCharacter(DomainCharacter expectedSavedCharacter)
        {
            throw new NotImplementedException();
        }
    }
}

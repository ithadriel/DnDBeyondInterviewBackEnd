using ddb_back_end_developer_challenge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Persistence
{
    public interface ICharacterRepositoryAdapter
    {
        DomainCharacter GetCharacter(long id);
        void UpdateCharacter(DomainCharacter character);
        void SaveeCharacter(DomainCharacter character);
    }
}

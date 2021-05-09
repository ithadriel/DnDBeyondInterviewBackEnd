using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Core.Services
{
    public class CharacterService
    {
        private ICharacterRepositoryAdapter repositoryAdapter;
        public CharacterService(ICharacterRepositoryAdapter repoAdapter)
        {
            this.repositoryAdapter = repoAdapter;
        }

        public int HealCharacter(int characterId, int healAmount)
        {
            DomainCharacter character = repositoryAdapter.GetCharacter(characterId);
            int updatedHp = Math.Min(Math.Max(character.CurrentHitpoints, 0) + healAmount, character.MaxHitpoints);

            character.CurrentHitpoints = updatedHp;
            repositoryAdapter.UpdateCharacter(character);

            return updatedHp;
        }
    }
}

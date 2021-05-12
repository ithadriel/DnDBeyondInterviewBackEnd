using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Core.Services
{
    public class CharacterService : ICharacterService
    {
        private ICharacterRepositoryAdapter repositoryAdapter;
        public CharacterService(ICharacterRepositoryAdapter repoAdapter)
        {
            this.repositoryAdapter = repoAdapter;
        }

        public DomainCharacter HealCharacter(long characterId, int healAmount)
        {
            DomainCharacter character = repositoryAdapter.GetCharacter(characterId);
            int updatedHp = Math.Min(Math.Max(character.CurrentHitpoints, 0) + healAmount, character.MaxHitpoints);

            character.CurrentHitpoints = updatedHp;
            repositoryAdapter.UpdateCharacter(character);

            return character;
        }

        public DomainCharacter DamageCharacter(long characterId, int damageAmount, string damageType)
        {
            DomainCharacter character = repositoryAdapter.GetCharacter(characterId);
            int adjustedDamageAmount = damageAmount;
            Console.WriteLine(damageType);
            if(character.Resistances != null && character.Resistances.Contains(damageType, StringComparer.OrdinalIgnoreCase))
            {
                adjustedDamageAmount /= 2;
            }
            if(character.Vulnerabilities != null && character.Vulnerabilities.Contains(damageType, StringComparer.OrdinalIgnoreCase))
            {
                adjustedDamageAmount *= 2;
            }
            if(character.Immunities != null && character.Immunities.Contains(damageType, StringComparer.OrdinalIgnoreCase))
            {
                adjustedDamageAmount = 0;
            }

            if(adjustedDamageAmount > character.TemporaryHitpoints)
            {
                character.CurrentHitpoints = character.CurrentHitpoints - (adjustedDamageAmount - character.TemporaryHitpoints);
                character.TemporaryHitpoints = 0;
            } else
            {
                character.TemporaryHitpoints -= adjustedDamageAmount;
            }

            repositoryAdapter.UpdateCharacter(character);

            return character;
        }

        public DomainCharacter AddTemporaryHitpoints(long characterId, int addedTempHp)
        {
            DomainCharacter character = repositoryAdapter.GetCharacter(characterId);

            character.TemporaryHitpoints = Math.Max(character.TemporaryHitpoints, addedTempHp);

            repositoryAdapter.UpdateCharacter(character);

            return character;
        }
    }
}

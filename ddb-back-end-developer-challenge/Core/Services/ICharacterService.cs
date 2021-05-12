using ddb_back_end_developer_challenge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Core.Services
{
    public interface ICharacterService
    {
        public DomainCharacter HealCharacter(long characterId, int healAmount);
        public DomainCharacter DamageCharacter(long characterId, int damageAmount, string damageType);
        public DomainCharacter AddTemporaryHitpoints(long characterId, int addedTempHp);
    }
}

using ddb_back_end_developer_challenge.Adapters.Rest.Models;
using ddb_back_end_developer_challenge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Utilities
{
    public class CharacterConverter
    {
        public static DomainCharacter ToDomain(Character character)
        {
            if(character == null)
            {
                return null;
            }

            DomainCharacter domainCharacter = new DomainCharacter();
            int totalHp = 0;
            if(character.Classes != null)
            {
                foreach (RpgClass rpgClass in character.Classes)
                {
                    totalHp += rpgClass.ClassLevel * ((rpgClass.HitDiceValue / 2) + 1);
                    totalHp += rpgClass.ClassLevel * CalculateConMod(character.Items, character.Stats.Constitution);
                }
            }

            List<string> immunities = new List<string>();
            List<string> resistances = new List<string>();
            List<string> vulnerabilities = new List<string>();

            if(character.Defenses != null)
            {
                foreach(CharacterDefense defense in character.Defenses)
                {
                    if(defense.Defense.Equals("immunity", StringComparison.OrdinalIgnoreCase))
                    {
                        immunities.Add(defense.Type);
                    } else if(defense.Defense.Equals("vulnerable", StringComparison.OrdinalIgnoreCase))
                    {
                        vulnerabilities.Add(defense.Type);
                    } else if(defense.Defense.Equals("resistance", StringComparison.OrdinalIgnoreCase))
                    {
                        resistances.Add(defense.Type);
                    }
                }
            }

            domainCharacter.MaxHitpoints = totalHp;
            domainCharacter.CurrentHitpoints = totalHp;
            domainCharacter.Immunities = immunities;
            domainCharacter.Vulnerabilities = vulnerabilities;
            domainCharacter.Resistances = resistances;
            domainCharacter.Name = character.Name;
            return domainCharacter;
        }

        private static int CalculateConMod(List<Item> items, int constitutionScore)
        {
            int totalConstitutionScore = constitutionScore;
            if(items != null)
            {
                foreach (Item item in items)
                {
                    Modifier itemMod = item.Modifier;
                    if (itemMod.AffectedObject.Equals("stats", StringComparison.OrdinalIgnoreCase) && itemMod.AffectedValue.Equals("constitution", StringComparison.OrdinalIgnoreCase))
                    {
                        totalConstitutionScore += itemMod.Value;
                    }
                }
            }
            int conMod = (totalConstitutionScore - 10) / 2;
            if (totalConstitutionScore < 10)
                --conMod;
            return conMod;
        }
    }
}

using ddb_back_end_developer_challenge.Adapters.Rest.Models;
using ddb_back_end_developer_challenge.Adapters.Utilities;
using ddb_back_end_developer_challenge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BackEndChallengeTests.Adapters.Utilities
{
    public class CharacterConverterTest
    {
        [Fact]
        public void ToDomain_ShouldReturnNull_WhenNullCharacterPassed()
        {
            Assert.Null(CharacterConverter.ToDomain(null));
        }

        [Fact]
        public void ToDomain_ShouldTranslateCharacterName()
        {
            Character namedCharacter = new Character { Name = "bob" };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(namedCharacter);

            Assert.Equal("bob", actualCharacter.Name);
        }

        [Theory]
        [InlineData(2, 4, 6)]
        [InlineData(5, 8, 25)]
        [InlineData(10, 12, 70)]
        public void ToDomain_ShouldCalculateHpCorrectly_NoItems_SingleClass_NoConMod(int classLevel, int hitDiceValue, int expectedHp)
        {
            RpgClass onlyClass = new RpgClass { ClassLevel = classLevel, HitDiceValue = hitDiceValue, Name = "singleClass" };
            List<RpgClass> singletonClass = new List<RpgClass> { onlyClass };
            Stats noConMod = new Stats { Constitution = 10 };
            Character noItemCharacter = new Character
            {
                Classes = singletonClass,
                Stats = noConMod,
            };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(noItemCharacter);

            Assert.Equal(expectedHp, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedHp, actualCharacter.CurrentHitpoints);
        }

        [Theory]
        [InlineData(14, 2, 4, 10)]
        [InlineData(15, 2, 4, 10)]
        [InlineData(20, 5, 8, 50)]
        [InlineData(9, 1, 20, 10)]
        [InlineData(7, 3, 12, 15)]
        public void ToDomain_ShouldCalculateHpCorrectly_NoItems_SingleClass_WithConMod(int constitutionScore, int classLevel, int hitDiceValue, int expectedHp)
        {
            RpgClass onlyClass = new RpgClass { ClassLevel = classLevel, HitDiceValue = hitDiceValue, Name = "singleClass" };
            List<RpgClass> singletonClass = new List<RpgClass> { onlyClass };
            Stats conMod = new Stats { Constitution = constitutionScore };
            Character noItemCharacter = new Character
            {
                Classes = singletonClass,
                Stats = conMod,
            };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(noItemCharacter);

            Assert.Equal(expectedHp, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedHp, actualCharacter.CurrentHitpoints);
        }

        [Fact]
        public void ToDomain_ShouldCalculateHpCorrectly_NoItems_MultipleClasses_WithConMod()
        {
            RpgClass firstClass = new RpgClass { ClassLevel = 2, HitDiceValue = 10, Name = "firstClass" };
            RpgClass secondClass = new RpgClass { ClassLevel = 1, HitDiceValue = 6, Name = "secondClass" };
            RpgClass thirdClass = new RpgClass { ClassLevel = 5, HitDiceValue = 12, Name = "thirdClass" };
            List<RpgClass> singletonClass = new List<RpgClass> { firstClass, secondClass, thirdClass };
            Stats conMod = new Stats { Constitution = 14 };
            Character noItemCharacter = new Character
            {
                Classes = singletonClass,
                Stats = conMod,
            };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(noItemCharacter);

            Assert.Equal(67, actualCharacter.MaxHitpoints);
            Assert.Equal(67, actualCharacter.CurrentHitpoints);
        }

        [Fact]
        public void ToDomain_ShouldCalculateHpCorrectly_WithItemsThatModifyCon()
        {
            RpgClass firstClass = new RpgClass { ClassLevel = 2, HitDiceValue = 10, Name = "firstClass" };
            RpgClass secondClass = new RpgClass { ClassLevel = 1, HitDiceValue = 6, Name = "secondClass" };
            RpgClass thirdClass = new RpgClass { ClassLevel = 5, HitDiceValue = 12, Name = "thirdClass" };
            List<RpgClass> singletonClass = new List<RpgClass> { firstClass, secondClass, thirdClass };
            Modifier conModification = new Modifier
            {
                AffectedObject = "stats",
                AffectedValue = "constitution",
                Value = 2
            };
            Item item = new Item { Name = "Ioun Stone of Fortitude", Modifier = conModification };
            Stats conMod = new Stats { Constitution = 14 };
            Character characterWithConItem = new Character
            {
                Classes = singletonClass,
                Stats = conMod,
                Items = new List<Item> { item, item }
            };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(characterWithConItem);

            Assert.Equal(83, actualCharacter.MaxHitpoints);
            Assert.Equal(83, actualCharacter.CurrentHitpoints);
        }

        [Fact]
        public void ToDomain_ShouldTranslateDefenseImmunities()
        {
            CharacterDefense fireImmunity = new CharacterDefense { Defense = "immunity", Type = "fire" };
            CharacterDefense slashingImmunity = new CharacterDefense { Defense = "immunity", Type = "slashing" };
            Character characterWithImmunities = new Character { Defenses = new List<CharacterDefense> { fireImmunity, slashingImmunity } };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(characterWithImmunities);

            Assert.Equal(new List<string> { "fire", "slashing" }, actualCharacter.Immunities);
        }
        
        [Fact]
        public void ToDomain_ShouldTranslateDefenseVulnerabilities()
        {
            CharacterDefense fireImmunity = new CharacterDefense { Defense = "vulnerable", Type = "bludgeoning" };
            CharacterDefense slashingImmunity = new CharacterDefense { Defense = "vulnerable", Type = "slashing" };
            Character characterWithImmunities = new Character { Defenses = new List<CharacterDefense> { fireImmunity, slashingImmunity } };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(characterWithImmunities);

            Assert.Equal(new List<string> { "bludgeoning", "slashing" }, actualCharacter.Vulnerabilities);
        }
          
        [Fact]
        public void ToDomain_ShouldTranslateDefenseResistances()
        {
            CharacterDefense fireImmunity = new CharacterDefense { Defense = "resistance", Type = "radiant" };
            CharacterDefense slashingImmunity = new CharacterDefense { Defense = "resistance", Type = "force" };
            Character characterWithImmunities = new Character { Defenses = new List<CharacterDefense> { fireImmunity, slashingImmunity } };

            DomainCharacter actualCharacter = CharacterConverter.ToDomain(characterWithImmunities);

            Assert.Equal(new List<string> { "radiant", "force" }, actualCharacter.Resistances);
        }


    }
}

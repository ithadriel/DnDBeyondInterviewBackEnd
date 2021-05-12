using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Core.Models;
using ddb_back_end_developer_challenge.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BackEndChallengeTests.Core.Services
{
    public class CharacterServiceTest
    {
        private CharacterService characterService;
        private Mock<ICharacterRepositoryAdapter> mockDbAdapter;

        public CharacterServiceTest()
        {
            mockDbAdapter = new Mock<ICharacterRepositoryAdapter>();
            characterService = new CharacterService(mockDbAdapter.Object);
        }

        [Theory]
        [InlineData(5, 10, 15, 15)]
        [InlineData(5, 15, 15, 15)]
        [InlineData(2, 1, 15, 3)]
        [InlineData(5, -20, 15, 5)]
        public void HealCharacter_ShouldAddHealAmountToCurrentHitpoints_Update_AndReturnCharacter(int healAmount, int currentHp, int maxHp, int expectedHp)
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = currentHp,
                MaxHitpoints = maxHp,
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);
            DomainCharacter expectedCharacter = new DomainCharacter
            {
                CurrentHitpoints = expectedHp,
                MaxHitpoints = maxHp,
            };

            DomainCharacter actualCharacter = characterService.HealCharacter(0, healAmount);

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedHp)));
            Assert.Equal(expectedCharacter.MaxHitpoints, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedCharacter.CurrentHitpoints, actualCharacter.CurrentHitpoints);
        }

        [Theory]
        [InlineData(5, 10, 5)]
        [InlineData(5, 15, 10)]
        [InlineData(2, 1, -1)]
        [InlineData(5, -20, -25)]
        public void DamageCharacter_ShouldReduceHp_Update_AndReturnCharacter_NoDefense(int damageAmount, int currentHp, int expectedHp)
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = currentHp,
                MaxHitpoints = 100,
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);
            DomainCharacter expectedCharacter = new DomainCharacter
            {
                CurrentHitpoints = expectedHp,
                MaxHitpoints = 100,
            };

            DomainCharacter actualCharacter = characterService.DamageCharacter(0, damageAmount, "slashing");

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedHp)));
            Assert.Equal(expectedCharacter.MaxHitpoints, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedCharacter.CurrentHitpoints, actualCharacter.CurrentHitpoints);
        }
        
        [Theory]
        [InlineData(5, 10, 5, 10, 0)]
        [InlineData(5, 15, 10, 15, 5)]
        [InlineData(2, 1, 10, 1, 8)]
        [InlineData(5, 20, 2, 17, 0)]
        public void DamageCharacter_ShouldReduceHp_Update_AndReturnCharacter_NoDefense_TempHp(int damageAmount, int currentHp, int currentTempHp, int expectedHp, int expectedTempHp)
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = currentHp,
                MaxHitpoints = 100,
                TemporaryHitpoints = currentTempHp
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);
            DomainCharacter expectedCharacter = new DomainCharacter
            {
                CurrentHitpoints = expectedHp,
                MaxHitpoints = 100,
                TemporaryHitpoints = expectedTempHp,
            };

            DomainCharacter actualCharacter = characterService.DamageCharacter(0, damageAmount, "slashing");

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedHp)));
            Assert.Equal(expectedCharacter.MaxHitpoints, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedCharacter.CurrentHitpoints, actualCharacter.CurrentHitpoints);
            Assert.Equal(expectedCharacter.TemporaryHitpoints, actualCharacter.TemporaryHitpoints);
        }

        [Fact]
        public void DamageCharacter_ShouldReduceHp_Update_AndReturnCharacter_WithMatchingResistanceDefense()
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = 10,
                MaxHitpoints = 100,
                Resistances = new List<string> { "slashing" }
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);
            DomainCharacter expectedCharacter = new DomainCharacter
            {
                CurrentHitpoints = 5,
                MaxHitpoints = 100,
                Resistances = new List<string> { "slashing" }
            };

            DomainCharacter actualCharacter = characterService.DamageCharacter(0, 10, "slashing");

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedCharacter.CurrentHitpoints)));
            Assert.Equal(expectedCharacter.MaxHitpoints, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedCharacter.CurrentHitpoints, actualCharacter.CurrentHitpoints);
        }
        
        [Fact]
        public void DamageCharacter_ShouldReduceHp_Update_AndReturnCharacter_WithMatchingVulnerabilityDefense()
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = 10,
                MaxHitpoints = 100,
                Vulnerabilities = new List<string> { "slashing" }
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);
            DomainCharacter expectedCharacter = new DomainCharacter
            {
                CurrentHitpoints = -10,
                MaxHitpoints = 100,
                Vulnerabilities = new List<string> { "slashing" }
            };

            DomainCharacter actualCharacter = characterService.DamageCharacter(0, 10, "slashing");

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedCharacter.CurrentHitpoints)));
            Assert.Equal(expectedCharacter.MaxHitpoints, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedCharacter.CurrentHitpoints, actualCharacter.CurrentHitpoints);
        }

        [Fact]
        public void DamageCharacter_ShouldReduceHp_Update_AndReturnCharacter_WithMatchingImmunityDefense()
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = 10,
                MaxHitpoints = 100,
                Immunities = new List<string> { "slashing" }
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);
            DomainCharacter expectedCharacter = new DomainCharacter
            {
                CurrentHitpoints = 10,
                MaxHitpoints = 100,
                Immunities = new List<string> { "slashing" }
            };

            DomainCharacter actualCharacter = characterService.DamageCharacter(0, 10, "slashing");

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedCharacter.CurrentHitpoints)));
            Assert.Equal(expectedCharacter.MaxHitpoints, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedCharacter.CurrentHitpoints, actualCharacter.CurrentHitpoints);
        }

        [Fact]
        public void DamageCharacter_ShouldReduceHp_Update_AndReturnCharacter_NonMatchingDefenses()
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = 10,
                MaxHitpoints = 100,
                Immunities = new List<string> { "slashing" },
                Vulnerabilities = new List<string> { "bludgeoning" },
                Resistances = new List<string> { "piercing" },
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);
            DomainCharacter expectedCharacter = new DomainCharacter
            {
                CurrentHitpoints = 0,
                MaxHitpoints = 100,
                Immunities = new List<string> { "slashing" },
                Vulnerabilities = new List<string> { "bludgeoning" },
                Resistances = new List<string> { "piercing" },
            };

            DomainCharacter actualCharacter = characterService.DamageCharacter(0, 10, "fire");

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedCharacter.CurrentHitpoints)));
            Assert.Equal(expectedCharacter.MaxHitpoints, actualCharacter.MaxHitpoints);
            Assert.Equal(expectedCharacter.CurrentHitpoints, actualCharacter.CurrentHitpoints);
        }
        
        [Theory]
        [InlineData(10, 0, 10)]
        [InlineData(5, 0, 5)]
        [InlineData(3, 5, 5)]
        [InlineData(8, 5, 8)]
        public void AddTemporaryHp_ShouldAddTemporaryHpAccordingToRules_Update_AndReturnCharacter(int addedTempHp, int currentTempHp, int expectedTempHp)
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                TemporaryHitpoints = currentTempHp,
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);

            DomainCharacter actualCharacter = characterService.AddTemporaryHitpoints(0, addedTempHp);

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.TemporaryHitpoints == expectedTempHp)));
            Assert.Equal(expectedTempHp, actualCharacter.TemporaryHitpoints);
        }

    }
}

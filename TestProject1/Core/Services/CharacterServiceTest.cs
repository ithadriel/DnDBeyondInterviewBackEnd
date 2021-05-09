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
        public void HealCharacter_ShouldAddHealAmountToCurrentHitpoints_Update_AndReturnCurrentHp(int healAmount, int currentHp, int maxHp, int expectedHp)
        {
            DomainCharacter retrievedCharacter = new DomainCharacter
            {
                CurrentHitpoints = currentHp,
                MaxHitpoints = maxHp,
            };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.IsAny<long>())).Returns(retrievedCharacter);


            int actualHp = characterService.HealCharacter(0, healAmount);

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.CurrentHitpoints == expectedHp)));
            Assert.Equal(expectedHp, actualHp);
        }
    }
}

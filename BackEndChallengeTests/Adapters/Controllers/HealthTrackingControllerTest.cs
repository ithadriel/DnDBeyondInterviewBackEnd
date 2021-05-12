using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Adapters.Rest.Controllers;
using ddb_back_end_developer_challenge.Core.Models;
using ddb_back_end_developer_challenge.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BackEndChallengeTests.Adapters.Controllers
{
    public class HealthTrackingControllerTest
    {
        private Mock<ICharacterService> mockCharacterService;
        private HealthTrackingController healthTrackingController;
        
        public HealthTrackingControllerTest()
        {
            mockCharacterService = new Mock<ICharacterService>();
            healthTrackingController = new HealthTrackingController(mockCharacterService.Object);
        }

        [Fact]
        public void DamageCharacter_shouldCallServiceAndReturnResult()
        {
            DomainCharacter damagedCharacter = new DomainCharacter { Name = "bob" };
            mockCharacterService.Setup(service => service.DamageCharacter(3, 3, "fire")).Returns(damagedCharacter);

            DomainCharacter actualCharacter = healthTrackingController.DamageCharacter(3, 3, "fire").Value;

            Assert.Equal(damagedCharacter, actualCharacter);
        }

        [Fact]
        public void HealCharacter_shouldCallServiceAndReturnResult()
        {
            DomainCharacter healedCharacter = new DomainCharacter { Name = "bob" };
            mockCharacterService.Setup(service => service.HealCharacter(3, 3)).Returns(healedCharacter);

            DomainCharacter actualCharacter = healthTrackingController.HealCharacter(3, 3).Value;

            Assert.Equal(healedCharacter, actualCharacter);
        }

        [Fact]
        public void AddTempHp_shouldCallServiceAndReturnResult()
        {
            DomainCharacter tempHpCharacter = new DomainCharacter { Name = "bob" };
            mockCharacterService.Setup(service => service.AddTemporaryHitpoints(3, 3)).Returns(tempHpCharacter);

            DomainCharacter actualCharacter = healthTrackingController.AddTemporaryHitpoints(3, 3).Value;

            Assert.Equal(tempHpCharacter, actualCharacter);
        }
    }
}

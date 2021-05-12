using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Adapters.Rest.Models;
using ddb_back_end_developer_challenge.Controllers;
using ddb_back_end_developer_challenge.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BackEndChallengeTests.Adapters.Controllers
{
    public class DomainCharacterControllerTest
    {
        private Mock<ICharacterRepositoryAdapter> mockDbAdapter;
        private DomainCharacterController domainCharacterController;

        public DomainCharacterControllerTest()
        {
            mockDbAdapter = new Mock<ICharacterRepositoryAdapter>();
            domainCharacterController = new DomainCharacterController(mockDbAdapter.Object);
        }

        [Fact]
        public void Index_ShouldReturnAllCharacters()
        {
            List<DomainCharacter> retrievedDomainCharacters = new List<DomainCharacter> {
                new DomainCharacter { Name = "bob" },
                new DomainCharacter { Name = "sue" }
            };
            mockDbAdapter.Setup(adapter => adapter.GetAllCharacters()).Returns(retrievedDomainCharacters);

            ActionResult<List<DomainCharacter>> actualDomainCharacters = domainCharacterController.Index();

            Assert.NotNull(actualDomainCharacters.Value);
            Assert.True(retrievedDomainCharacters.SequenceEqual(actualDomainCharacters.Value));
            mockDbAdapter.Verify(adapter => adapter.GetAllCharacters());
        }

        [Fact]
        public void AddDomainCharacter_ShouldAddDomainCharacterToDb()
        {
            DomainCharacter characterToAdd = new DomainCharacter { Name = "banana" };
            mockDbAdapter.Setup(adapter => adapter.SaveCharacter(It.IsAny<DomainCharacter>())).Returns(characterToAdd);

            ActionResult<DomainCharacter> addedCharacter = domainCharacterController.AddDomainCharacter(characterToAdd);

            mockDbAdapter.Verify(adapter => adapter.SaveCharacter(characterToAdd));
            Assert.Equal(characterToAdd.Name, addedCharacter.Value.Name);
        }
        
        [Fact]
        public void AddCharacter_ShouldConvertToDomainAndAddDomainCharacterToDb()
        {
            Character characterToAdd = new Character { Name = "banana" };
            DomainCharacter convertedCharacter = new DomainCharacter { Name = "banana" };
            mockDbAdapter.Setup(adapter => adapter.SaveCharacter(It.IsAny<DomainCharacter>())).Returns(convertedCharacter);

            ActionResult<DomainCharacter> addedCharacter = domainCharacterController.AddCharacter(characterToAdd);

            Assert.Equal(characterToAdd.Name, addedCharacter.Value.Name);
            mockDbAdapter.Verify(adapter => adapter.SaveCharacter(It.Is<DomainCharacter>(c => c.Name == convertedCharacter.Name)));
        }

        [Fact]
        public void UpdateDomainCharacter_ShouldCallUpdateAndReturnCharacter()
        {
            DomainCharacter characterToUpdate = new DomainCharacter { Name = "banana" };
            DomainCharacter characterToUpdateWithId = new DomainCharacter { Name = "banana", Id = 3 };
            mockDbAdapter.Setup(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => c.Id == 3))).Returns(characterToUpdateWithId);

            ActionResult<DomainCharacter> addedCharacter = domainCharacterController.UpdateDomainCharacter(characterToUpdate, 3);

            mockDbAdapter.Verify(adapter => adapter.UpdateCharacter(It.Is<DomainCharacter>(c => (c.Name == characterToUpdateWithId.Name) && (c.Id == characterToUpdateWithId.Id))));
            Assert.Equal(characterToUpdateWithId, addedCharacter.Value);
        }
        
        [Fact]
        public void UpdateDomainCharacter_ShouldReturnCharacterRetrieval()
        {
            DomainCharacter characterToGet = new DomainCharacter { Name = "banana", Id = 3 };
            mockDbAdapter.Setup(adapter => adapter.GetCharacter(It.Is<long>(l => l == 3))).Returns(characterToGet);

            ActionResult<DomainCharacter> gottenCharacter = domainCharacterController.GetDomainCharacter(3);

            mockDbAdapter.Verify(adapter => adapter.GetCharacter(It.Is<long>(l => l == 3)));
            Assert.Equal(characterToGet, gottenCharacter.Value);
        }

    }
}

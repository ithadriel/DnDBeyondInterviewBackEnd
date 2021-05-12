using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Core.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BackEndChallengeTests.Adapters.Persistence
{
    public class CharacterRepositoryAdapterTest
    {
        private Mock<CharacterDbContext> mockDbContext;
        private CharacterRepositoryAdapter repository;

        public CharacterRepositoryAdapterTest()
        {
            mockDbContext = new Mock<CharacterDbContext>();
            repository = new CharacterRepositoryAdapter(mockDbContext.Object);
        }

        [Fact]
        public void GetAllCharacters_ShouldReturnAllCharactersInDb()
        {
            List<DomainCharacter> retrievedCharacters = new List<DomainCharacter>
            {
                new DomainCharacter { Name = "sue" },
                new DomainCharacter { Name = "bob" },
            };
            mockDbContext.Setup(dbContext => dbContext.DomainCharacters).Returns(GetQueryableMockDbSet<DomainCharacter>(retrievedCharacters));

            List<DomainCharacter> actualCharacters = repository.GetAllCharacters();

            Assert.True(retrievedCharacters.SequenceEqual(actualCharacters));
        }

        [Fact]
        public void GetCharacter_ShouldFindCharacterWithMatchingIdAndReturn()
        {
            DomainCharacter retrievedCharacter = new DomainCharacter { Name = "sue" };
            mockDbContext.Setup(dbContext => dbContext.Find<DomainCharacter>(It.Is<long>(l => l == 3))).Returns(retrievedCharacter);

            DomainCharacter actualCharacter = repository.GetCharacter(3);

            Assert.Equal(retrievedCharacter, actualCharacter);
        }
        
        [Fact]
        public void SaveCharacter_ShouldSaveCharacterToDb_returnCharacter()
        {
            DomainCharacter characterToAdd = new DomainCharacter { Name = "sue" };

            mockDbContext.Setup(dbContext => dbContext.DomainCharacters).Returns(GetQueryableMockDbSet<DomainCharacter>(new List<DomainCharacter>()));

            DomainCharacter character = repository.SaveCharacter(characterToAdd);

            mockDbContext.Verify(context => context.Add(It.Is<DomainCharacter>(c => c.Name == characterToAdd.Name)));
            mockDbContext.Verify(context => context.SaveChanges(), Times.Once());
            Assert.Equal(characterToAdd, character);
        }
        
        [Fact]
        public void SaveCharacter_ShouldUpdateCharacterInDb_returnCharacter()
        {
            DomainCharacter characterToUpdate = new DomainCharacter { Name = "sue" };

            mockDbContext.Setup(dbContext => dbContext.DomainCharacters).Returns(GetQueryableMockDbSet<DomainCharacter>(new List<DomainCharacter>()));

            DomainCharacter character = repository.UpdateCharacter(characterToUpdate);

            mockDbContext.Verify(context => context.Update(It.Is<DomainCharacter>(c => c.Name == characterToUpdate.Name)));
            mockDbContext.Verify(context => context.SaveChanges(), Times.Once());
            Assert.Equal(characterToUpdate, character);
        }


        //credit Liam from https://stackoverflow.com/questions/31349351/how-to-add-an-item-to-a-mock-dbset-using-moq
        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
    }
}

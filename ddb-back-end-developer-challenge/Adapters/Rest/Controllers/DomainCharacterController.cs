using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Adapters.Rest.Models;
using ddb_back_end_developer_challenge.Adapters.Utilities;
using ddb_back_end_developer_challenge.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Controllers
{
    [ApiController]
    public class DomainCharacterController : ControllerBase
    {
        private ICharacterRepositoryAdapter repositoryAdapter;

        public DomainCharacterController(ICharacterRepositoryAdapter repositoryAdapter)
        {
            this.repositoryAdapter = repositoryAdapter;
        }

        [HttpGet]
        [Route("/api/characters")]
        [Route("/api/characters/index")]
        public ActionResult<List<DomainCharacter>> Index()
        {
            return repositoryAdapter.GetAllCharacters();
        }

        [HttpPost]
        [Route("/api/characters")]
        [Route("/api/characters/addDomainCharacter")]
        public ActionResult<DomainCharacter> AddDomainCharacter([FromBody] DomainCharacter characterToAdd)
        {
            return repositoryAdapter.SaveCharacter(characterToAdd);
        }

        [HttpPost]
        [Route("/api/characters/addCharacter")]
        public ActionResult<DomainCharacter> AddCharacter([FromBody] Character characterToConvert)
        {
            return repositoryAdapter.SaveCharacter(CharacterConverter.ToDomain(characterToConvert));
        }

        [HttpPost]
        [Route("/api/characters/{id}")]
        public ActionResult<DomainCharacter> UpdateDomainCharacter(DomainCharacter characterToUpdate, long id)
        {
            characterToUpdate.Id = id;
            return repositoryAdapter.UpdateCharacter(characterToUpdate);
        }

        [HttpGet]
        [Route("/api/characters/{id}")]
        public ActionResult<DomainCharacter> GetDomainCharacter(int id)
        {
            return repositoryAdapter.GetCharacter(id);
        }
    }
}

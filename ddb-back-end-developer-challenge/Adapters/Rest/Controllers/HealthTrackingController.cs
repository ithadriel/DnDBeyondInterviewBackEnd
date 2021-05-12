using ddb_back_end_developer_challenge.Core.Models;
using ddb_back_end_developer_challenge.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge.Adapters.Rest.Controllers
{
    [ApiController]
    public class HealthTrackingController : ControllerBase
    {
        private ICharacterService characterService;

        public HealthTrackingController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [HttpPost]
        [Route("/api/characters/{id}/damage")]
        public ActionResult<DomainCharacter> DamageCharacter(int id, int amount, string damageType)
        {
            return characterService.DamageCharacter(id, amount, damageType);
        }

        [HttpPost]
        [Route("/api/characters/{id}/heal")]
        public ActionResult<DomainCharacter> HealCharacter(int id, int amount)
        {
            return characterService.HealCharacter(id, amount);
        }

        [HttpPost]
        [Route("/api/characters/{id}/addTemporaryHp")]
        public ActionResult<DomainCharacter> AddTemporaryHitpoints(int id, int amount)
        {
            return characterService.AddTemporaryHitpoints(id, amount);
        }
    }
}

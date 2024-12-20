using JobExpressBack.Models.Entities;
using JobExpressBack.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobExpressBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvisController : ControllerBase
    {
        private readonly IGenericRepository<Avis> avisRepository;

        public AvisController(IGenericRepository<Avis> AvisRepository)
        {
            avisRepository = AvisRepository;
        }

        [HttpGet("getAllAvis")]
        [Authorize] // Accessible à tous les utilisateurs connectés
        public async Task<IActionResult> GetAllAvis()
        {
            var aviss = await avisRepository.GetAll();
            return Ok(aviss);
        }

        [HttpGet("getAvisById/{id}")]
        [Authorize] // Accessible à tous les utilisateurs connectés
        public async Task<IActionResult> GetAvisById(int id)
        {
            var avis = await avisRepository.GetById(id);
            if (avis == null)
            {
                return NotFound(new { Message = "avis non trouvée" });
            }
            return Ok(avis);
        }

        [HttpPost("createAvis")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateAvis([FromBody] Avis avis)
        {
            if (avis == null)
            {
                return BadRequest(new { Message = "Données invalides" });
            }
            await avisRepository.Add(avis);
            return CreatedAtAction(nameof(GetAllAvis), new { id = avis.AvisID }, avis);
        }

        [HttpPut("updateAvis/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAvis(int id, [FromBody] Avis avis)
        {
            if (id != avis.AvisID)
            {
                return BadRequest();
            }
            await avisRepository.Update(avis);
            return NoContent();
        }

        [HttpDelete("deleteAvis/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAvis(int id)
        {
            var existingavis = await avisRepository.GetById(id);
            if (existingavis == null)
            {
                return NotFound(new { Message = "avis non trouvée" });
            }

            await avisRepository.Delete(id);
            return NoContent();
        }

    }
}

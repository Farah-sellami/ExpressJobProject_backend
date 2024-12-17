using JobExpressBack.Models.Entities;
using JobExpressBack.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobExpressBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly IGenericRepository<Categorie> categorieRepository;

        public CategorieController(IGenericRepository<Categorie> CategorieRepository )
        {
            this.categorieRepository = CategorieRepository;
        }

        [HttpGet("getAllCategories")]
        [Authorize] // Accessible à tous les utilisateurs connectés
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categorieRepository.GetAll();
            return Ok(categories);
        }

        [HttpGet(" getCategorieById/{id}")]
        [Authorize] // Accessible à tous les utilisateurs connectés
        public async Task<IActionResult> GetCategorie(int id)
        {
            var categorie = await categorieRepository.GetById(id);
            if (categorie == null)
            {
                return NotFound(new { Message = "Catégorie non trouvée" });
            }
            return Ok(categorie);
        }

        [HttpPost("createCategorie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategorie([FromBody] Categorie categorie)
        {
            if (categorie == null)
            {
                return BadRequest(new { Message = "Données invalides" });
            }
            await categorieRepository.Add(categorie);
            return CreatedAtAction(nameof(GetAllCategories), new { id = categorie.CategorieID }, categorie);
        }

        [HttpPut("updateCategorie/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategorie(int id, [FromBody] Categorie categorie)
        {
            if (id != categorie.CategorieID)
            {
                return BadRequest();
            }
            await categorieRepository.Update(categorie);
            return NoContent();
        }

        [HttpDelete("deleteCategorie/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            var existingCategorie = await categorieRepository.GetById(id);
            if (existingCategorie == null)
            {
                return NotFound(new { Message = "Catégorie non trouvée" });
            }

            await categorieRepository.Delete(id);
            return NoContent();
        }

    }
}

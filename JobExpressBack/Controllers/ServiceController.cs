using JobExpressBack.Models;
using JobExpressBack.Models.Entities;
using JobExpressBack.Models.Repositories;
using JobExpressBack.Models.Repositories.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobExpressBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
          
            this.serviceRepository = serviceRepository;
        }

        [HttpGet("GetAllServices")]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await serviceRepository.GetAllServicesWithDetails();
            return Ok(services);
        }

        [HttpGet("GetServiceById/{id}")]
       
        public async Task<IActionResult> GetService(int id)
        {
            var service = await serviceRepository.GetServiceByIdWithDetails(id);
            if (service == null)
            {
                return NotFound();
            }
            // Charger explicitement la catégorie associée
            var serviceWithCategory = await serviceRepository.GetById(id);

            return Ok(serviceWithCategory);
        }

        [HttpPost("createService")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest();
            }
            // Utiliser la méthode du repository pour créer le service et associer la catégorie
            var createdService = await serviceRepository.CreateServiceWithCategory(service);

            if (createdService == null)
            {
                return NotFound("La catégorie spécifiée n'existe pas.");
            }

            return CreatedAtAction(nameof(GetService), new { id = createdService.ServiceID }, createdService);
        }

        [HttpPut("updateService/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] Service service)
        {
            if (id != service.ServiceID)
            {
                return BadRequest();
            }
            await serviceRepository.Update(service);
            return NoContent();
        }

        [HttpDelete("deleteService/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var existingService = await serviceRepository.GetById(id);
            if (existingService == null)
            {
                return NotFound(new { Message = "Service non trouvé" });
            }
            await serviceRepository.Delete(id);
            return NoContent();
        }

        [HttpGet("getByCategory/{categoryId}")]
       
        public async Task<IActionResult> GetServicesByCategory(int categoryId)
        {
            var services = await serviceRepository.GetServicesByCategory(categoryId);

            // Vérifier si des services existent pour cette catégorie
            if (services == null || !services.Any())
            {
                return NotFound(new { message = "Aucun service trouvé pour cette catégorie." });
            }

            return Ok(services);
        }
    }
}

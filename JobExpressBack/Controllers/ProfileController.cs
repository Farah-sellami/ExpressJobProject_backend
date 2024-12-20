using CloudinaryDotNet.Actions;
using JobExpressBack.Models;
using JobExpressBack.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace JobExpressBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Nécessite une authentification avec JWT
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ExJobDBContext exJobDBContext;

        public ProfileController(UserManager<ApplicationUser> userManager, ExJobDBContext exJobDBContext)
        {
            _userManager = userManager;
            this.exJobDBContext = exJobDBContext;
        }

        //Admin, client et professionnel peuvent consulter leurs propres données.
        // GET: api/Profile
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            // Récupérer et afficher les claims pour le débogage
            var claims = User.Claims.ToList();
            Console.WriteLine("Claims dans le jeton :");
            foreach (var claim in claims)
            {
                Console.WriteLine($"Type : {claim.Type}, Valeur : {claim.Value}");
            }

            // Récupération de l'ID utilisateur
            var userId = User.FindFirstValue("uid");

            if (userId == null)
            {
                return Unauthorized(new { Message = "Aucun ID utilisateur trouvé dans le jeton" });
            }
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
          
            if (user == null)
            {
                return NotFound(new { Message = $"Utilisateur non trouvé pour l'ID {userId}" });
            }
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Address,
                user.Telephone,
                user.PhotoProfile,
                Role = roles.FirstOrDefault(),
                Skills = roles.Contains("Professionnel") ? user.Skills : null,
                Disponibility = roles.Contains("Professionnel") ? user.Disponibility : null 
            });
        }

        //client ou professionnel peuvent modifier leurs données personnelles.
        // PUT: api/Profile
        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] Dictionary<string, object> updates)
        {
            var userId = User.FindFirstValue("uid");
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { Message = "Utilisateur non trouvé" });
            }

            // Récupérer le rôle de l'utilisateur (Client ou Professionnel)
            var roles = await _userManager.GetRolesAsync(user);
            var isProfessional = roles.Contains("Professionnel");

            // Mise à jour des champs dynamiquement
            foreach (var update in updates)
            {
                switch (update.Key.ToLower())
                {
                    case "firstname":
                        user.FirstName = update.Value?.ToString();
                        break;
                    case "lastname":
                        user.LastName = update.Value?.ToString();
                        break;
                    case "address":
                        user.Address = update.Value?.ToString();
                        break;
                    case "email":
                        user.Email = update.Value?.ToString();
                        break;
                    case "photoProfile":
                        user.PhotoProfile = update.Value?.ToString();
                        break;
                    case "telephone":
                        user.Telephone = update.Value?.ToString();
                        break;
                    
                    case "skills":
                        if (!isProfessional)
                            return BadRequest(new { Message = "Seuls les professionnels peuvent mettre à jour les compétences." });
                        user.Skills = update.Value?.ToString();
                        break;
                    case "disponibility":
                        if (!isProfessional)
                            return BadRequest(new { Message = "Seuls les professionnels peuvent mettre à jour leur disponibilité." });
                        user.Disponibility = update.Value?.ToString();
                        break;
                    default:
                        return BadRequest(new { Message = $"Champ '{update.Key}' non valide." });
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Mise à jour échouée", Errors = result.Errors });
            }

            return Ok(new { Message = "Profil mis à jour avec succès" });
        }

        // Seuls les administrateurs peuvent supprimer les comptes des utilisateurs
        // DELETE: api/Profile
        [HttpDelete("DeleteAccount/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            // Rechercher l'utilisateur à supprimer
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { Message = "Utilisateur non trouvé" });
            }

            // Supprimer l'utilisateur
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Échec de la suppression", Errors = result.Errors });
            }

            return Ok(new { Message = "Compte supprimé avec succès" });
        }

        //permet aux utilisateurs de supprimer leurs propres données personnelles (sans supprimer le compte)
        [HttpDelete("DeletePersonalData")]
        public async Task<IActionResult> DeletePersonalData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { Message = "Utilisateur non trouvé" });
            }

            // Suppression des données personnelles
            user.FirstName = null;
            user.LastName = null;
            user.Address = null;
            user.Telephone = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Échec de la suppression des données personnelles", Errors = result.Errors });
            }

            return Ok(new { Message = "Données personnelles supprimées avec succès" });
        }

        // GET: api/Profile/GetAllUsers
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")] // Seuls les administrateurs peuvent récupérer tous les utilisateurs
        public async Task<IActionResult> GetAllUsers()
        {
            // Récupérer tous les utilisateurs
            var users = await _userManager.Users.ToListAsync();

            if (users == null || !users.Any())
            {
                return NotFound(new { Message = "Aucun utilisateur trouvé" });
            }

            // Créer une liste des utilisateurs avec les informations pertinentes et leurs rôles
            var usersList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Récupérer les rôles de l'utilisateur de manière asynchrone

                usersList.Add(new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Address,
                    user.Telephone,
                    user.PhotoProfile,
                    Roles = roles // Liste des rôles
                });
            }

            return Ok(new { Users = usersList });
        }

        // GET: api/Profile/CountClients
        [HttpGet("CountClients")]
        [Authorize(Roles = "Admin")] // Seuls les administrateurs peuvent accéder à ces information
          public async Task<ActionResult<int>> CountClients()
        {
            var users = await exJobDBContext.Users.ToListAsync();
            int count = 0;

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Client"))
                {
                    count++;
                }
            }

            return count;
        }

        // GET: api/Profile/CountProfessionals
        [HttpGet("CountProfessionals")]
        [Authorize(Roles = "Admin")] // Seuls les administrateurs peuvent accéder à ces informations
        public async Task<ActionResult<int>> CountProfessionals()
       {
            var users = await exJobDBContext.Users.ToListAsync();
            int count = 0;

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Professionnel"))
                {
                    count++;
                }
            }

            return count;
        }



    }
}

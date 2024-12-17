﻿using FirebaseAdmin.Messaging;
using JobExpressBack.Models.DTOs;
using JobExpressBack.Models.Entities;
using JobExpressBack.Models.Repositories;
using JobExpressBack.Models.Repositories.RepoDemandeService;
using JobExpressBack.Models.Repositories.RepoNotification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobExpressBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeServiceController : ControllerBase
    {
        private readonly IDemandeServiceRepository demandeServiceRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public DemandeServiceController(IDemandeServiceRepository demandeServiceRepo, UserManager<ApplicationUser> userManager)
        {
            this.demandeServiceRepo = demandeServiceRepo;
            this.userManager = userManager;
        }

        // Méthode pour obtenir toutes les demandes (uniquement pour les administrateurs)
        [HttpGet("getAllDemandeService")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDemandeService()
        {
            // Vérifier si l'utilisateur est un administrateur
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                return Unauthorized(new { Message = "Accès interdit. Uniquement les administrateurs peuvent voir toutes les demandes." });
            }

            var demandeServices = await demandeServiceRepo.GetAll();
            return Ok(demandeServices);
        }


        // Méthode pour obtenir une demande spécifique (avec contrôle d'accès)
        [HttpGet("GetDemandeServiceById/{clientId}/{professionnelId}/{dateDemande}")]
        public async Task<IActionResult> GetDemandeService(string clientId, string professionnelId, DateTime dateDemande)
        {
            var demandeService = await demandeServiceRepo.GetByIds(clientId, professionnelId, dateDemande);
            if (demandeService == null)
            {
                return NotFound(new { Message = "Demande de service non trouvée." });
            }

            // Vérifier si l'utilisateur est un client ou un professionnel
            // et si l'accès est autorisé
            //// Récupérer l'ID et le rôle de l'utilisateur connecté
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userRole == "Client" && demandeService.ClientId != userId)
            {
                return Unauthorized(new { Message = "Vous ne pouvez consulter que vos propres demandes." });
            }

            if (userRole == "Professionnel" && demandeService.ProfessionnelId != userId)
            {
                return Unauthorized(new { Message = "Vous ne pouvez consulter que les demandes où vous êtes le prestataire." });
            }

            return Ok(demandeService);
        }

        [HttpPost("createDemandeService")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateDemandeService([FromBody] DemandeService demandeService)
        {
            if (demandeService == null)
            {
                return BadRequest(new { Message = "Les données de la demande de service sont invalides." });
            }
            // Extraire l'ID du client authentifié depuis le token
            var clientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Récupérer l'ID du client depuis le token

            if (clientId == null)
            {
                return Unauthorized(new { Message = "Utilisateur non authentifié." });
            }

            // Assigner le ClientId de l'utilisateur authentifié
            demandeService.ClientId = clientId;
            
            Console.WriteLine($"ProfessionnelId fourni : {demandeService.ProfessionnelId}");

            // Vérifier si le professionnel existe
            var professionnel = await userManager.FindByIdAsync(demandeService.ProfessionnelId);
            if (professionnel == null)
            {
                Console.WriteLine($"Professionnel introuvable pour l'ID : {demandeService.ProfessionnelId}");
                return NotFound(new { Message = "Professionnel introuvable." });
            }
            else if (string.IsNullOrEmpty(professionnel.FcmToken))
            {
                Console.WriteLine($"FcmToken manquant pour le professionnel ID : {demandeService.ProfessionnelId}");
                return BadRequest(new { Message = "FcmToken non disponible pour le professionnel." });
            }

            // Ajouter la demande au dépôt
            await demandeServiceRepo.Add(demandeService);

            // Recharger la demande avec les relations incluses
            var demandeAvecRelations = await demandeServiceRepo.GetByIds(
                demandeService.ClientId,
                demandeService.ProfessionnelId,
                demandeService.DateDemande
            );

            if (demandeAvecRelations == null)
            {
                return StatusCode(500, new { Message = "Erreur lors de la récupération de la demande après création." });
            }

            // Envoyer la notification via Firebase Cloud Messaging
            try
            {
                var firebaseService = new FirebaseService();
                await firebaseService.SendNotificationAsync(professionnel.FcmToken);

                return CreatedAtAction(nameof(CreateDemandeService), new
                {
                    demandeService.ClientId,
                    demandeService.ProfessionnelId,
                    demandeService.DateDemande
                }, new { Message = "Demande de service créée et notification envoyée." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de la notification : {ex.Message}");
                return StatusCode(500, new { Message = "Erreur interne lors de l'envoi de la notification." });
            }
        }

            // Méthode fictive pour récupérer le token FCM d'un professionnel
            private async Task<string> GetProfessionnelTokenAsync(DemandeService demandeService)
        {
            if (demandeService.Professionnel == null)
            {
                // Charger la relation si elle n'est pas incluse
                demandeService.Professionnel = await userManager.FindByIdAsync(demandeService.ProfessionnelId);
            }

            if (demandeService.Professionnel?.FcmToken == null)
            {
                Console.WriteLine($"Token FCM introuvable pour le professionnel ID {demandeService.ProfessionnelId}.");
                return null;
            }

            return demandeService.Professionnel.FcmToken;
        }


        /**cette méthode depuis l'application cliente pour mettre à jour le FcmToken chaque fois que l'application est démarrée ou que le FcmToken est rafraîchi.**/
        [HttpPost("update-token-fcm")]
        [Authorize]
        public async Task<IActionResult> UpdateFcmToken([FromBody] string token)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var professionnel = await userManager.FindByIdAsync(userId);
            if (professionnel == null)
            {
                return NotFound(new { Message = "Utilisateur non trouvé." });
            }

            professionnel.FcmToken = token;
            await userManager.UpdateAsync(professionnel);
            return Ok(new { Message = "Token FCM mis à jour." });
        }


        [HttpPut("{clientId}/{professionnelId}/{dateDemande}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdateDemandeService(string clientId,
        string professionnelId,
        DateTime dateDemande,
        [FromBody] DemandeService demandeService)
        {
            // Vérifier si l'utilisateur est le client ou le professionnel de la demande
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != clientId )
            {
                return Unauthorized(new { Message = "Vous ne pouvez modifier que vos propres demandes." });
            }

            if (clientId != demandeService.ClientId || professionnelId != demandeService.ProfessionnelId || dateDemande != demandeService.DateDemande)
            {
                return BadRequest("Les informations de clé primaire ne correspondent pas.");
            }
            await demandeServiceRepo.Update(demandeService);
            return NoContent();
        }

        [HttpDelete("{clientId}/{professionnelId}/{dateDemande}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> DeleteDemandeService(string clientId, string professionnelId, DateTime dateDemande)
        {
            // Vérifier si l'utilisateur est le client ou le professionnel de la demande
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != clientId)
            {
                return Unauthorized(new { Message = "Vous ne pouvez supprimer que vos propres demandes." });
            }
            // Appel de la méthode DeleteDemandeService dans le repository
            await demandeServiceRepo.DeleteDemandeService(clientId, professionnelId, dateDemande);

            return NoContent();
        }
    }
}

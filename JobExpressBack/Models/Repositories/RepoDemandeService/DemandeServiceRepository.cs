using JobExpressBack.Models.Entities;
using JobExpressBack.Models.Repositories.RepoServices;
using Microsoft.EntityFrameworkCore;

namespace JobExpressBack.Models.Repositories.RepoDemandeService
{
    public class DemandeServiceRepository : GenericRepository<DemandeService>, IDemandeServiceRepository
    {
        private readonly ExJobDBContext exJobDBContext;

        public DemandeServiceRepository(ExJobDBContext exJobDBContext) : base(exJobDBContext)
        {
            this.exJobDBContext = exJobDBContext;
        }
        public async Task<DemandeService> GetByIds(string clientId, string professionnelId, DateTime dateDemande)
        {
            return await exJobDBContext.DemandeServices
                .Include(ds => ds.Client) // Charger le client
                .Include(ds => ds.Professionnel) // Charger le professionnel
                .FirstOrDefaultAsync(ds => ds.ClientId == clientId && ds.ProfessionnelId == professionnelId && ds.DateDemande == dateDemande);
        }

        public async Task DeleteDemandeService(string clientId, string professionnelId, DateTime dateDemande)
        {
            // Trouver l'entité DemandeService avec la clé primaire composée
            var demandeService = await exJobDBContext.DemandeServices
                .FirstOrDefaultAsync(ds => ds.ClientId == clientId && ds.ProfessionnelId == professionnelId && ds.DateDemande == dateDemande);

            if (demandeService != null)
            {
                // Supprimer l'entité de la base de données
                exJobDBContext.DemandeServices.Remove(demandeService);
                await exJobDBContext.SaveChangesAsync();
            }
        }
    }
}

using JobExpressBack.Models.Entities;

namespace JobExpressBack.Models.Repositories.RepoDemandeService
{
    public interface IDemandeServiceRepository : IGenericRepository<DemandeService>
    {
        Task<DemandeService> GetByIds(string clientId, string professionnelId, DateTime dateDemande);
        Task DeleteDemandeService(string clientId, string professionnelId, DateTime dateDemande);
    }
}

using JobExpressBack.Models.Entities;

namespace JobExpressBack.Models.Repositories.RepoServices
{//ajoutant des méthodes spécifiques à Service a part CRUD si nécessaire.
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<IEnumerable<Service>> GetServicesByCategory(int categoryId);
        Task<Service> CreateServiceWithCategory(Service service);
        Task<IEnumerable<Service>> GetAllServicesWithDetails();
        Task<Service> GetServiceByIdWithDetails(int serviceId);
    }
}

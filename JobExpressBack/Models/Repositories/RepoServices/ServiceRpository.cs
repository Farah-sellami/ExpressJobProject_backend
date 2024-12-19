using JobExpressBack.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobExpressBack.Models.Repositories.RepoServices
{
    public class ServiceRpository : GenericRepository<Service>, IServiceRepository
    {
        private readonly ExJobDBContext exJobDBContext;

        public ServiceRpository(ExJobDBContext exJobDBContext) : base(exJobDBContext)
        {
            this.exJobDBContext = exJobDBContext;
        }
        // Méthode spécifique à ServiceRepository pour récupérer les services par catégorie

        public async Task<IEnumerable<Service>> GetServicesByCategory(int categoryId)
        {
            return await exJobDBContext.Services
                .Where(s => s.CategorieID == categoryId)
                .Include(s => s.Categorie)
                .Include(s => s.Professionnels)
                
                .ToListAsync();
        }

        //pour recuperer categorie lors de creation de service
        public async Task<Service> CreateServiceWithCategory(Service service)
        {
            // Vérifier si la catégorie existe
            var categorie = await exJobDBContext.Categories.FindAsync(service.CategorieID);
            if (categorie == null)
            {
                return null; // ou lancer une exception si tu préfères
            }

            // Assigner la catégorie au service
            service.Categorie = categorie;

            // Ajouter le service à la base de données
            await Add(service);
            return service;
        }

        // Méthode pour récupérer tous les services avec catégorie et professionnels
        public async Task<IEnumerable<Service>> GetAllServicesWithDetails()
        {
            return await exJobDBContext.Services
                .Include(s => s.Categorie)        // Inclure la catégorie
                .Include(s => s.Professionnels)   // Inclure les professionnels (s'il y en a)
                .ToListAsync();
        }

        public async Task<Service> GetServiceByIdWithDetails(int serviceId)
        {
            return await exJobDBContext.Services
                .Include(s => s.Categorie)        // Inclure la catégorie
                .Include(s => s.Professionnels)   // Inclure les professionnels
                .FirstOrDefaultAsync(s => s.ServiceID == serviceId);
        }



    }
}

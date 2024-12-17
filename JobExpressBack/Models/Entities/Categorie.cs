using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobExpressBack.Models.Entities
{
    public class Categorie
    {
        public int CategorieID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<Service>? Services { get; set; }
    }
}

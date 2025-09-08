using System.ComponentModel.DataAnnotations;

namespace NimapTask.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        // Navigation property
        public virtual ICollection<Product> Products { get; set; }
    }
}
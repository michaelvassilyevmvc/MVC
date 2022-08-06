using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky_Models
{
    public class ProductManager
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }
    }
}
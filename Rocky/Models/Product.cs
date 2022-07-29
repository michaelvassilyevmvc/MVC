using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }
    }
}
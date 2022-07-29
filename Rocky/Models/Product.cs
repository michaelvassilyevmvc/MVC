using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Product Name")]
        public string Name { get; set; }
    }
}
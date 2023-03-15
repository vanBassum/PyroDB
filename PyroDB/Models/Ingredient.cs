using System.ComponentModel.DataAnnotations;

namespace PyroDB.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; }
        public virtual Chemical? Chemical { get; set; }
    }

}
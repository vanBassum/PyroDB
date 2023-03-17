using System.ComponentModel.DataAnnotations;

namespace PyroDB.Models.Database
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public float? Quantity { get; set; }
        public virtual Chemical? Chemical { get; set; }
    }

}
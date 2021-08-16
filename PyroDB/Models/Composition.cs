using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;

namespace PyroDB.Models
{

    public class Composition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        [NotMapped]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string IngredientsJSON { get; set; }

        [NotMapped]
        public Ingredient[] Ingredients
        {
            get => JsonSerializer.Deserialize<Ingredient[]>(StringCompressor.DecompressString(IngredientsJSON));
            set => IngredientsJSON = StringCompressor.CompressString(JsonSerializer.Serialize<Ingredient[]>(value));
        }


    }

    public class Ingredient
    {
        public Chemical Chemical { get; set; }
        public int Amount { get; set; } //percentage in thousands
        public string Notes { get; set; } //e.g. charcoal specify wood, grainsize etc.
    }
}

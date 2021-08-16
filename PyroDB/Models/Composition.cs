using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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


    public class Chemical
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Notes { get; set; }
        public GHSSymbols GHS { get; set; }

    }
    /*
    [Flags]
    public enum GHSSymbols
    {
        Explosive = 1,
        Flammable = 2,
        Oxidizing = 3,
        Compressed_Gas = 4,
        Corrosive = 5,
        Toxic = 6,
        Harmful = 7,
        Health_Hazard = 8,
        Environmental_Hazard = 9,
    }
    */
     [Flags]
     public enum GHSSymbols
     { 
         Explosive      = 1,
         Flammable      = 2,
         Oxidizing      = 4,
         Compressed     = 8,
         Corrosive      = 16,
         Toxic          = 32,
         Harmful        = 64,
         Health         = 128,
         Environmental  = 256,
     }

}

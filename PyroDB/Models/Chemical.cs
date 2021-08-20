using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PyroDB.Models
{
    public class Chemical
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Notes { get; set; }
        public GHSSymbols GHS { get; set; }


        [NotMapped]
        public IList<GHSSymbols> CheckedSymbols { get; set; }


        public static IList<GHSSymbols> GetAvailableGHS()
        {
            return (GHSSymbols[])Enum.GetValues(typeof(GHSSymbols));
        }
        
    }


    [Flags]
    public enum GHSSymbols
    {
        Explosive = 1,
        Flammable = 2,
        Oxidizing = 4,
        Compressed = 8,
        Corrosive = 16,
        Toxic = 32,
        Harmful = 64,
        Health = 128,
        Environmental = 256,
    }
}

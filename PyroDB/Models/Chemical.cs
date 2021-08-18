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
        public IList<string> SelectedGHS
        {
            get
            {
                var result = new List<string>();
                foreach (GHSSymbols flag in Enum.GetValues(typeof(GHSSymbols)))
                {
                    if (GHS.HasFlag(flag))
                        result.Add(flag.ToString());
                }
                return result;
            }

            set
            {
                GHS = 0;
                foreach (GHSSymbols flag in Enum.GetValues(typeof(GHSSymbols)))
                {
                    if (value.Contains(flag.ToString()))
                        GHS |= flag;
                }
            }
        }

        public static IList<SelectListItem> GetAvailableGHS()
        {
            var result = new List<SelectListItem>();

            foreach (var flag in Enum.GetValues(typeof(GHSSymbols)))
            {
                result.Add(new SelectListItem() { Text = flag.ToString(), Value = flag.ToString() });
            }
            return result;
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

﻿using System.ComponentModel.DataAnnotations;

namespace PyroDB.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public virtual DataSourceInfo? DataSourceInfo { get; set; }
    }

}
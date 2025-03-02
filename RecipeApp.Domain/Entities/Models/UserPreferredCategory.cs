﻿using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class UserPreferredCategory
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
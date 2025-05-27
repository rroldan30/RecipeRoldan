using MobSysFinalsBase1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobSysFinalsBase1.Services
{
    public class RecipeService
    {
        private readonly List<Recipe> _recipes = new();

        public List<Recipe> GetAll() => _recipes;

        public Recipe? GetById(Guid id) => _recipes.FirstOrDefault(r => r.Id == id);

        public void Add(Recipe recipe) => _recipes.Add(recipe);

        public void Update(Recipe recipe)
        {
            var existing = GetById(recipe.Id);
            if (existing != null)
            {
                existing.Title = recipe.Title;
                existing.PrepTimeMinutes = recipe.PrepTimeMinutes;
                existing.Ingredients = recipe.Ingredients;
                existing.Instructions = recipe.Instructions;
            }
        }

        public void Delete(Guid id)
        {
            var recipe = GetById(id);
            if (recipe != null)
            {
                _recipes.Remove(recipe);
            }
        }
    }

}

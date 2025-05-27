using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using MobSysFinalsBase1.Models;

namespace MobSysFinalsBase1.Services
{
    public class RecipeDbService
    {
        private readonly string _dbPath;

        public RecipeDbService()
        {
            var folder = FileSystem.AppDataDirectory;
            _dbPath = Path.Combine(folder, "recipes.db");
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Recipes (
                Id TEXT PRIMARY KEY,
                Title TEXT,
                PrepTimeMinutes INTEGER,
                Ingredients TEXT,
                Instructions TEXT,
                ImagePath TEXT
            );
        ";
            cmd.ExecuteNonQuery();
        }

        public List<Recipe> GetAll()
        {
            var recipes = new List<Recipe>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Title, PrepTimeMinutes, Ingredients, Instructions FROM Recipes";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                recipes.Add(new Recipe
                {
                    Id = reader.GetGuid(0),
                    Title = reader.GetString(1),
                    PrepTimeMinutes = reader.GetInt32(2),
                    Ingredients = reader.GetString(3).Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Instructions = reader.GetString(4).Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList()
                });
            }

            return recipes;
        }


        public void AddOrUpdate(Recipe recipe)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"
        INSERT INTO Recipes (Id, Title, PrepTimeMinutes, Ingredients, Instructions)
        VALUES ($id, $title, $prepTime, $ingredients, $instructions)
        ON CONFLICT(Id) DO UPDATE SET
            Title = $title,
            PrepTimeMinutes = $prepTime,
            Ingredients = $ingredients,
            Instructions = $instructions;
    ";

            command.Parameters.AddWithValue("$id", recipe.Id);
            command.Parameters.AddWithValue("$title", recipe.Title);
            command.Parameters.AddWithValue("$prepTime", recipe.PrepTimeMinutes);
            command.Parameters.AddWithValue("$ingredients", string.Join('\n', recipe.Ingredients));
            command.Parameters.AddWithValue("$instructions", string.Join('\n', recipe.Instructions));

            command.ExecuteNonQuery();
        }


        public void Delete(Guid id)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Recipes WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CNN_1
{
    internal class Controller
    {
        private List<Movie> movies;

        public Controller(string jsonFilePath)
        {
            LoadMovies(jsonFilePath);
        }

        private void LoadMovies(string jsonFilePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                movies = JsonConvert.DeserializeObject<List<Movie>>(jsonContent) ?? new List<Movie>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке JSON-файла: {ex.Message}");
                movies = new List<Movie>();
            }
        }

        public List<Movie> GetMovies(string genre, int? minRating = null, int? maxDuration = null, string actor = null)
        {
            return movies.Where(movie =>
                (string.IsNullOrEmpty(genre) || movie.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)) &&
                (!minRating.HasValue || movie.Rating >= minRating) &&
                (string.IsNullOrEmpty(actor) || movie.Actors.Contains(actor, StringComparer.OrdinalIgnoreCase)) &&
                (!maxDuration.HasValue || movie.Duration <= maxDuration)
            ).ToList();
        }
        public List<string> GetUniqueGenres()
        {
            return movies.Select(movie => movie.Genre).Distinct().ToList();
        }

        public List<string> GetUniqueActors()
        {
            return movies.SelectMany(movie => movie.Actors).Distinct().ToList();
        }

        public List<int> GetUniqueRatings()
        {
            return movies.Select(movie => movie.Rating).Distinct().ToList();
        }

        public List<int> GetUniqueDurations()
        {
            return movies.Select(movie => movie.Duration).Distinct().ToList();
        }
    }

    internal class Movie
    {
        public string Title { get; set; } 
        public string Genre { get; set; }
        public int Rating { get; set; }
        public List<string> Actors { get; set; }
        public int Duration { get; set; }
    }

}

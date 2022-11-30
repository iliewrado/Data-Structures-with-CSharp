using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MovieDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieDatabase movieDatabase = new MovieDatabase();

            Movie Movie = new Movie("asd", "bsd", 3000, 3000, new List<string>(new string[] { "Pesho", "Gosho" }));
            Movie Movie2 = new Movie("dsd", "esd", 2020, 5000, null);

            movieDatabase.AddMovie(Movie);
            movieDatabase.AddMovie(Movie2);

            movieDatabase.GetAllMoviesOrderedByActorPopularityThenByRatingThenByYear();
        }
        private static Movie GetRandomMovie()
        {
            return new Movie(
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    (int)Math.Min(1, new Random().Next(0, 2_000)),
                    (int)Math.Min(1, new Random().Next(0, 10)),
                    new List<string>(Enumerable.Range(1, (int)Math.Min(1, new Random().Next(0, 10))).Select(x => Guid.NewGuid().ToString()).ToList()));
        }
    }
}

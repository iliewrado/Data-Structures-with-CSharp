using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MovieDatabase
{
    public class MovieDatabase : IMovieDatabase
    {
        private Dictionary<string, Movie> movies;
        private Dictionary<Movie, Dictionary<string, HashSet<Movie>>> actorPopularity;

        public MovieDatabase()
        {
            this.movies = new Dictionary<string, Movie>();
            this.actorPopularity =
                new Dictionary<Movie, Dictionary<string, HashSet<Movie>>>();
        }

        public int Count => this.movies.Count;

        public void AddMovie(Movie movie)
        {
            if (!this.movies.ContainsKey(movie.Id))
                this.movies.Add(movie.Id, movie);
            this.actorPopularity.Add(movie, new Dictionary<string, HashSet<Movie>>());

            foreach (var actor in movie.Actors)
            {
                if (!this.actorPopularity[movie].ContainsKey(actor))
                    this.actorPopularity[movie][actor] = new HashSet<Movie>();

                this.actorPopularity[movie][actor].Add(movie);
            }
        }

        public bool Contains(Movie movie)
        {
            return this.movies.ContainsKey(movie.Id);
        }

        public IEnumerable<Movie>
            GetAllMoviesOrderedByActorPopularityThenByRatingThenByYear()
        {
            if (this.movies.Count == 0)
                return new List<Movie>();

            return this.actorPopularity
                .OrderByDescending(x => x.Value.Values.Count)
                .ThenByDescending(x => x.Key.Rating)
                .ThenByDescending(x => x.Key.ReleaseYear)
                .Select(x => x.Key);
        }

        public IEnumerable<Movie> GetMoviesByActor(string actorName)
        {
            List<Movie> result = new List<Movie>();

            foreach (var movie in this.movies.Values)
            {
                if (movie.Actors.Contains(actorName))
                {
                    result.Add(movie);
                }
            }

            if (result.Count == 0)
                throw new ArgumentException();

            return result
                .OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.ReleaseYear);
        }

        public IEnumerable<Movie> GetMoviesByActors(List<string> actors)
        {
            List<Movie> result = new List<Movie>();

            foreach (var movie in this.movies.Values)
            {
                bool allOfActors = movie.Actors
                    .Intersect(actors)
                    .Count() == movie.Actors.Count();
                if (allOfActors)
                {
                    result.Add(movie);
                }
            }

            if (result.Count == 0)
                throw new ArgumentException();

            return result
                .OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.ReleaseYear);
        }

        public IEnumerable<Movie> GetMoviesByYear(int releaseYear)
        {
            List<Movie> result = this.movies.Values
                .Where(x => x.ReleaseYear == releaseYear)
                .OrderByDescending(x => x.Rating)
                .ToList();

            return result;
        }

        public IEnumerable<Movie> GetMoviesInRatingRange(double lowerBound, double upperBound)
        {
            return this.movies.Values
                .Where(x => x.Rating >= lowerBound && x.Rating <= upperBound)
                .OrderByDescending(x => x.Rating);
        }

        public void RemoveMovie(string movieId)
        {
            if (!this.movies.ContainsKey(movieId))
                throw new ArgumentException();

            Movie movie = this.movies[movieId];
            this.actorPopularity.Remove(movie);
            this.movies.Remove(movieId);
        }
    }
}

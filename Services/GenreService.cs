using System.Collections.Generic;
using Consul;
using genre_api.Models;
using MongoDB.Driver;

namespace GenresApi.Services
{
    public class GenreService
    {
        private readonly IMongoCollection<Genre> _genres;

        public GenreService(IGenresDatabaseSettings settings, IConsulClient consulClient)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _genres = database.GetCollection<Genre>(settings.GenresCollectionName);
        }

        public List<Genre> Get() =>
            _genres.Find(Genre => true).ToList();

        public Genre Get(string id) =>
            _genres.Find<Genre>(Genre => Genre.Id == new MongoDB.Bson.ObjectId(id)).FirstOrDefault();

        public Genre Create(Genre Genre)
        {
            _genres.InsertOne(Genre);
            return Genre;
        }

        public void Update(string id, Genre GenreIn) =>
            _genres.ReplaceOne(Genre => Genre.Id == new MongoDB.Bson.ObjectId(id), GenreIn);

        public void Remove(Genre GenreIn) =>
            _genres.DeleteOne(Genre => Genre.Id == GenreIn.Id);

        public void Remove(string id) =>
            _genres.DeleteOne(Genre => Genre.Id == new MongoDB.Bson.ObjectId(id));
    }
}
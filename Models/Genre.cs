using System;
using MongoDB.Bson;

namespace genre_api.Models
{
    public class Genre
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}



namespace genre_api.Models
{
    public class GenresDatabaseSettings : IGenresDatabaseSettings
    {
        public string GenresCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IGenresDatabaseSettings
    {
        string GenresCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
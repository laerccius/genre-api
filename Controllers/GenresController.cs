using System;
using System.Collections.Generic;
using System.Linq;
using genre_api.Models;
using GenresApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace genre_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenresController : ControllerBase
    {

        private readonly ILogger<GenresController> _logger;
        private readonly GenreService _genreService;

        public GenresController(ILogger<GenresController> logger, GenreService genreService)
        {
            _logger = logger;
            _genreService = genreService;
        }

        [HttpGet]
        public IEnumerable<Genre> Get()
        {
            return _genreService.Get();
        }
    }
}

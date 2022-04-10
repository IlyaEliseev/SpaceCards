namespace SpaceCards.API.Controllers
{
    public class CardsController
    {
        private readonly ILogger<CardsController> _logger;

        public CardsController(ILogger<CardsController> logger)
        {
            _logger = logger;
        }
    }
}

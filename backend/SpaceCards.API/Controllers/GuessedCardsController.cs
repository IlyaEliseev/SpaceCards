using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceCards.Domain.Interfaces;

namespace SpaceCards.API.Controllers
{
    [Authorize]
    public class GuessedCardsController : BaseApiController
    {
        private readonly ILogger<GuessedCardsController> _logger;
        private readonly IGuessedCardsService _service;
        private readonly IMapper _mapper;

        public GuessedCardsController(
            ILogger<GuessedCardsController> logger,
            IGuessedCardsService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Save guessed card.
        /// </summary>
        /// <param name="groupId">Groups id.</param>
        /// <param name="cardId">Card id.</param>
        /// <returns>Successful result.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveGuessedCard([FromBody] int groupId, [FromQuery] int cardId)
        {
            var result = await _service.SaveGuessedCard(groupId, cardId);

            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}

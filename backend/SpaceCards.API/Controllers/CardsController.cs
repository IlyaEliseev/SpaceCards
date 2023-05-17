using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.API.Controllers
{
    [Authorize]
    public class CardsController : BaseApiController
    {
        private readonly ILogger<CardsController> _logger;
        private readonly ICardsService _service;
        private readonly IMapper _mapper;

        public CardsController(ILogger<CardsController> logger, ICardsService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Create new card.
        /// </summary>
        /// <param name="request">New card.</param>
        /// <returns>Created card.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCardRequest request)
        {
            var userId = UserId.Value;

            var (cardId, errors) = await _service.Create(request.FrontSide, request.BackSide, userId);

            if (errors.Any() || cardId == default)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(cardId);
        }

        /// <summary>
        /// Get all cards.
        /// </summary>
        /// <returns>Cards.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contracts.GetCardResponse[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var userId = UserId.Value;

            var cards = await _service.Get(userId);
            var cardsContract = _mapper.Map<Card[], Contracts.GetCardResponse[]>(cards);

            return Ok(cardsContract);
        }

        /// <summary>
        /// Delete card by id.
        /// </summary>
        /// <param name="cardId">Card id.</param>
        /// <returns>Successful delete card.</returns>
        [HttpDelete("{cardId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int cardId)
        {
            var (result, errors) = await _service.Delete(cardId);

            if (errors.Any() || !result)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(result);
        }

        /// <summary>
        /// Find card and update.
        /// </summary>
        /// <param name="cardId">Card search parameter.</param>
        /// <param name="card">Card with new parameters.</param>
        /// <returns>Successful update card.</returns>
        [HttpPut("{cardId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int cardId, [FromBody] UpdateCardRequest card)
        {
            var (result, errors) = await _service.Update(cardId, card.FrontSide, card.BackSide);

            if (errors.Any() || !result)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(result);
        }
    }
}

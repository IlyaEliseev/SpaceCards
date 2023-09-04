using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Cache;
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
        private readonly ICacheService _cache;

        public CardsController(
            ILogger<CardsController> logger,
            ICardsService service,
            IMapper mapper,
            ICacheService cache)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _cache = cache;
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
            var result = await _service.Create(request.FrontSide, request.BackSide, userId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            await _cache.RemoveData($"{userId}-cards");

            return Ok(result.Value);
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
            var cachCards = await _cache.GetData<Card[]?>($"{userId}-cards");
            if (cachCards is not null)
            {
                return Ok(_mapper.Map<Card[], Contracts.GetCardResponse[]>(cachCards));
            }

            var cards = await _service.Get(userId);
            await _cache.SetData<Card[]?>($"{userId}-cards", cards, DateTime.Now.AddMinutes(5));

            return Ok(_mapper.Map<Card[], Contracts.GetCardResponse[]>(cards));
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
            var userId = UserId.Value;
            var result = await _service.Delete(cardId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            await _cache.RemoveData($"{userId}-cards");

            return Ok(result.Value);
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
        public async Task<IActionResult> Update([FromRoute] int cardId, [FromBody] UpdateCardRequest card)
        {
            var userId = UserId.Value;
            var result = await _service.Update(cardId, card.FrontSide, card.BackSide);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            await _cache.RemoveData($"{userId}-cards");

            return Ok(result.Value);
        }
    }
}

﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceCards.Domain;

namespace SpaceCards.API.Controllers
{
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
        /// Get guessed card.
        /// </summary>
        /// <param name="groupId">Groups id.</param>
        /// <param name="cardId">Card id.</param>
        /// <returns>Successful result.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TakeGuessedCard([FromBody] int groupId, [FromQuery] int cardId)
        {
            var (result, errors) = await _service.TakeGuessedCard(groupId, cardId);

            if (errors.Any() || result is false)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(result);
        }
    }
}

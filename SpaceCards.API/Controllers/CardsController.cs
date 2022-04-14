using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
using SpaceCards.Domain;
using System.Net.Mime;

namespace SpaceCards.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class CardsController : ControllerBase
    {
        private readonly ILogger<CardsController> _logger;

        public CardsController(ILogger<CardsController> logger)
        {
            _logger = logger;
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
            var card = Card.Create(request.Word, request.WordTranslate);

            if (card.Result == null)
            {
                return Ok(card.Errors);
            }

            return Ok(card.Result);
        }

        /// <summary>
        /// Get all cards.
        /// </summary>
        /// <returns>Cards.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Card[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var card = Card.Get();

            return Ok(card);
        }

        /// <summary>
        /// Delete card by card word.
        /// </summary>
        /// <param name="cardName">Word in card.</param>
        /// <returns>Successful delete card.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string cardName)
        {
            var deletedCard = Card.Delete(cardName);

            if (!deletedCard.Result)
            {
                return Ok(deletedCard.Errors);
            }

            return Ok(deletedCard.Result);
        }

        /// <summary>
        /// Find card and update.
        /// </summary>
        /// <param name="cardWord">Card search parametr.</param>
        /// <param name="card">Card with new parametrs.</param>
        /// <returns>Successful update card.</returns>
        [HttpPost("Update/{cardWord}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string cardWord, [FromBody] UpdateCardRequest card)
        {
            var result = Card.Update(cardWord, card.Word, card.WordTranslate);

            return Ok(result.Result);
        }
    }
}

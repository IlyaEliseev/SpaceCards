using SpaceCards.Domain;
using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for update card.
    /// </summary>
    public class UpdateCardRequest
    {
        [Required]
        [StringLength(Card.MAX_NAME_FRONTSIDE)]
        public string FrontSide { get; set; }

        [Required]
        [StringLength(Card.MAX_NAME_BACKSIDE)]
        public string BackSide { get; set; }
    }
}

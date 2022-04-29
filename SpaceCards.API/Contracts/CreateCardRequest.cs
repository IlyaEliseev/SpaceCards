using SpaceCards.Domain;
using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for creating card.
    /// </summary>
    public class CreateCardRequest
    {
        [Required]
        [StringLength(Card.MAX_NAME_FRONTSIDE)]
        public string FrontSide { get; set; }

        [Required]
        [StringLength(Card.MAX_NAME_BACKSIDE)]
        public string BackSide { get; set; }
    }
}

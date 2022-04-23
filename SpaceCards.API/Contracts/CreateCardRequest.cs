using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for creating card.
    /// </summary>
    public class CreateCardRequest
    {
        [Required]
        public string FrontSide { get; set; }

        [Required]
        public string BackSide { get; set; }
    }
}

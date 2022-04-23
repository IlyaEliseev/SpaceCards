using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for update card.
    /// </summary>
    public class UpdateCardRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FrontSide { get; set; }

        [Required]
        public string BackSide { get; set; }
    }
}

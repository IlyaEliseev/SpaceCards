using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for creating group.
    /// </summary>
    public class CreateGroupRequest
    {
        [Required]
        public string Name { get; set; }
    }
}

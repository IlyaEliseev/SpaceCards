using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for update group.
    /// </summary>
    public class UpdateGroupRequest
    {
        [Required]
        public string Name { get; set; }
    }
}

using SpaceCards.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for creating group.
    /// </summary>
    public class CreateGroupRequest
    {
        [Required]
        [StringLength(Group.MAX_NAME_LENGTH)]
        public string Name { get; set; }
    }
}

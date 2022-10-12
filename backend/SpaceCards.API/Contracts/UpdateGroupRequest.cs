using SpaceCards.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace SpaceCards.API.Contracts
{
    /// <summary>
    /// Contract for update group.
    /// </summary>
    public class UpdateGroupRequest
    {
        [Required]
        [StringLength(Group.MAX_NAME_LENGTH)]
        public string Name { get; set; }
    }
}

namespace SpaceCards.Domain
{
    public class Group
    {
        private Group(int id, string name, Card[] cards)
        {
            Id = id;
            Name = name;
            Cards = cards;
        }

        public int Id { get; }

        public string Name { get; }

        public Card[] Cards { get; }

        public static (Group? Result, string[] Errors) Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, new[] { $"'{nameof(name)}' cannot be null or whitespace." });
            }

            var group = new Group(0, name, Array.Empty<Card>());
            GroupRepository.Groups.Add(group);

            return (group, Array.Empty<string>());
        }

        public static (string? Word, string[] Errors) AddCard(string groupName, string cardWord)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                return (null, new[] { $"'{nameof(groupName)}' cannot be null or whitespace." });
            }

            if (string.IsNullOrWhiteSpace(groupName))
            {
                return (null, new[] { $"'{nameof(groupName)}' cannot be null or whitespace." });
            }

            var existedCard = CardRepository.Cards
                .FirstOrDefault(x => x.Word == cardWord);

            var existedGroup = GroupRepository.Groups
                .FirstOrDefault(x => x.Name == groupName);

            existedGroup.Cards.Append(existedCard).ToArray();

            return (existedCard.Word, Array.Empty<string>());
        }

        public static (Group[] Result, string[] Errors) GetAll()
        {
            var group = GroupRepository.Groups
                .ToArray();

            if (group == Array.Empty<Group>())
            {
                return (Array.Empty<Group>(), new[] { "Groups is empty." });
            }

            return (group, Array.Empty<string>());
        }

        public static (Group? Result, string[] Errors) Update(string groupName, string groupUdateName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                return (null, new[] { $"'{nameof(groupName)}' cannot be null or whitespace." });
            }

            if (string.IsNullOrWhiteSpace(groupUdateName))
            {
                return (null, new[] { $"'{nameof(groupUdateName)}' cannot be null or whitespace." });
            }

            var groups = GroupRepository.Groups;
            var findGroup = groups.FirstOrDefault(x => x.Name == groupName);

            if (!groups.Contains(findGroup))
            {
                return (null, new[] { "Group not found." });
            }

            var updatedGroup = new Group(0, groupUdateName, findGroup.Cards);
            groups[groups.IndexOf(findGroup)] = updatedGroup;

            return (updatedGroup, Array.Empty<string>());
        }

        public static (bool Result, string[] Errors) Delete(string groupName)
        {
            var deletedGroup = GroupRepository.Groups.FirstOrDefault(x => x.Name == groupName);

            if (string.IsNullOrWhiteSpace(groupName))
            {
                return (false, new[] { $"'{nameof(groupName)}' cannot be null or whitespace." });
            }

            if (deletedGroup == null)
            {
                return (false, new[] { $"Group with this {nameof(groupName)} not found." });
            }

            if (deletedGroup.Cards.Any())
            {
                return (false, new[] { "Group is not empty." });
            }

            GroupRepository.Groups.Remove(deletedGroup);

            return (true, Array.Empty<string>());
        }
    }
}

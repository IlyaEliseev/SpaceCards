using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Microsoft.EntityFrameworkCore.Update;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.BusinessLogic
{
    public class GroupsService : IGroupsService
    {
        private readonly IGroupsRepository _groupRepository;
        private readonly ICardsRepository _cardsRepository;

        public GroupsService(
            IGroupsRepository groupRepository,
            ICardsRepository cardsRepository)
        {
            _groupRepository = groupRepository;
            _cardsRepository = cardsRepository;
        }

        public async Task<Result<int>> Create(string name, Guid? userId)
        {
            var group = Group.Create(name, userId);
            if (group.IsFailure)
            {
                return Result.Failure<int>(group.Error);
            }

            var groupId = await _groupRepository.Add(group.Value);

            return groupId;
        }

        public async Task<Group[]> Get(Guid? userId)
        {
            return await _groupRepository.Get(userId);
        }

        public async Task<Result<bool>> AddCard(
            int cardId,
            int groupId)
        {
            var card = await _cardsRepository.GetById(cardId);
            if (card is null)
            {
                return Result.Failure<bool>($"'{nameof(card)}' not found.");
            }

            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return Result.Failure<bool>($"'{nameof(group)}' not found.");
            }

            await _groupRepository.AddCard(card.Id, group.Id);

            return true;
        }

        public async Task<Result<bool>> Update(
            int groupId,
            string updatedGroupName)
        {
            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return Result.Failure<bool>("Group is null");
            }

            var updatedGroup = Group.Create(updatedGroupName, group.UserId);
            if (updatedGroup.IsFailure)
            {
                return Result.Failure<bool>(updatedGroup.Error);
            }

            await _groupRepository.Update(updatedGroup.Value with { Id = group.Id });

            return true;
        }

        public async Task<Result<bool>> Delete(int groupId)
        {
            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return Result.Failure<bool>($"'{nameof(group)}' not found.");
            }

            if (group.Cards.Count() != 0)
            {
                return Result.Failure<bool>($"'{nameof(group)}' is not empty.");
            }

            await _groupRepository.Delete(group.Id);

            return true;
        }

        public async Task<Result<Group>> GetById(int groupId)
        {
            var group = await _groupRepository.GetById(groupId);
            if (group is null)
            {
                return Result.Failure<Group>($"'{nameof(group)}' not found.");
            }

            return group;
        }

        public async Task<Result<Card[]?>> GetRandomCards(int countCards, Guid? userId)
        {
            if (countCards <= default(int))
            {
                return Result.Failure<Card[]?>($"'{nameof(countCards)}' cannot be 0 or less 0.");
            }

            var randomCards = await _groupRepository.GetRandomCards(countCards, userId);

            return randomCards;
        }
    }
}

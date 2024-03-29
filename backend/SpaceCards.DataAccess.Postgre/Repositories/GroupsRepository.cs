﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Repositories
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly SpaceCardsDbContext _context;
        private readonly IMapper _mapper;

        public GroupsRepository(SpaceCardsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Add(Group group)
        {
            var groupEntity = _mapper.Map<Group, Entites.GroupEntity>(group);
            await _context.Groups.AddAsync(groupEntity);
            await _context.SaveChangesAsync();

            return groupEntity.Id;
        }

        public async Task<Group[]> Get(Guid? userId)
        {
            var groups = await _context.Groups
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToArrayAsync();

            return _mapper.Map<Entites.GroupEntity[], Group[]>(groups);
        }

        public async Task<Group?> GetById(int groupId)
        {
            var group = await _context.Groups
                .AsNoTracking()
                .Include(x => x.Cards)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            return _mapper.Map<Entites.GroupEntity, Group>(group);
        }

        public async Task Update(Group group)
        {
            var groupEntity = _mapper.Map<Group, Entites.GroupEntity>(group);
            _context.Groups.Update(groupEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int groupId)
        {
            var group = await _context.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == groupId);

            if (group is null)
            {
                return false;
            }

            _context.Groups.Remove(new Entites.GroupEntity { Id = groupId });
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddCard(int cardId, int groupId)
        {
            var card = await _context.Cards
                .FirstOrDefaultAsync(x => x.Id == cardId);

            if (card is null)
            {
                return false;
            }

            card.GroupId = groupId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Card[]> GetRandomCards(int countCards, Guid? userId)
        {
            var rnd = new Random();

            var randomCards = _context.Groups
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Cards)
                .SelectMany(x => x)
                .AsEnumerable()
                .OrderBy(x => rnd.Next())
                .Take(countCards)
                .ToArray();

            return _mapper.Map<Entites.CardEntity[], Card[]>(randomCards);
        }

        public async Task<Card?> GetCardFromGroupById(int groupId, int cardId)
        {
            var card = await _context.Groups
                .AsNoTracking()
                .Include(x => x.Cards)
                .Where(x => x.Id == groupId)
                .Select(x => x.Cards)
                .SelectMany(x => x)
                .FirstOrDefaultAsync(x => x.Id == cardId);

            if (card is null)
            {
                return null;
            }

            return _mapper.Map<Entites.CardEntity, Card>(card);
        }
    }
}

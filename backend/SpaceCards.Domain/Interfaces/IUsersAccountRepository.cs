﻿using CSharpFunctionalExtensions;
using SpaceCards.Domain.Model;

namespace SpaceCards.Domain.Interfaces
{
    public interface IUsersAccountRepository
    {
        Task<Result<bool>> Create(User user);

        Task<Result<User>> FindUserByEmail(string email);

        Task<Result<User>> UpdateUser(string email, string password);

        Task<Result<bool>> CheckPassword(User user, string passwordHash);
    }
}

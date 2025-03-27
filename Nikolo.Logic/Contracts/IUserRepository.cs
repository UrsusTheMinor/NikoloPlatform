using Nikolo.Api.DTOs;

namespace Nikolo.Logic.Contracts;

public interface IUserRepository
{
    public Task CreateOrUpdateUser(UserUpsertDto dto);
}
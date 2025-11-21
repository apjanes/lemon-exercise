using System.Diagnostics.CodeAnalysis;
using TaskList.Domain.Entities;

namespace TaskList.WebApi.Dtos;

public class UserDto
{
    public static UserDto? Create(User? user)
    {
        if (user == null) return null;
        return new UserDto(user);
    }

    public UserDto()
    {

    }

    [SetsRequiredMembers]
    public UserDto(User user)
    {
        Id = user.Id;
        Name = $"{user.FirstName} {user.LastName}";
    }
    public Guid? Id { get; set; }

    public required string Name { get; set; }
}
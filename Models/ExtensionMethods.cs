using System;
using BlogAPI.PostgreSQL;

namespace BlogAPI.Models;

public static partial class PostEntityExtensionMethods
{
    public static PostDto? AsDto(this PostEntity post)
    {
        return (post == null) ? null : new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            IsPrivate = post.IsPrivate,
            AuthorId = post.Author.Id
        };
    }
}

public static partial class UserEntityExtensionMethods
{
    public static UserDto? AsDto(this UserEntity user)
    {
        return (user == null) ? null : new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            EmailAddress = user.EmailAddress
        };
    }

    public static string Stringify(this UserDto user)
    {
        return $"Id: {user.Id} -> {user.UserName}";
    }


}


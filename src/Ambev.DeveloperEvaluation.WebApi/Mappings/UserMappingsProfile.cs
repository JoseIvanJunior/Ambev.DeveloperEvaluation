using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class UserMappingsProfile : Profile
{
    public UserMappingsProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();

        CreateMap<GetUserResult, GetUserResponse>();
    }
}
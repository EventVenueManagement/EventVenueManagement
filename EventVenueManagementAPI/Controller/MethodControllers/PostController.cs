using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller.MethodControllers;

public interface PostController<in T, R> where R : IResult
{
    public Task<R> Execute(T input);
}
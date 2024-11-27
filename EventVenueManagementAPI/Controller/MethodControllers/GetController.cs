using EventVenueManagementCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventVenueManagementAPI.Controller.MethodControllers;


public interface GetController<out R> where R : IResult
{
    public Task<Ok<IEnumerable<Event.EventBrief>>> Execute();
}

public interface GetController<in T, out R> where R : IResult
{
    public Task<Results<NotFound, Ok<Event>>> Execute(T input);
}
namespace EventVenueManagementAPI.Controller.MethodControllers;

public interface PostController<in T, out R> where R : IResult
{
    public R Execute(T input);
}
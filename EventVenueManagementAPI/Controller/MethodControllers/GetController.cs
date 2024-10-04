namespace EventVenueManagementAPI.Controller.MethodControllers;


public interface GetController<out R> where R : IResult
{
    public R Execute();
}

public interface GetController<in T, out R> where R : IResult
{
    public R Execute(T input);
}
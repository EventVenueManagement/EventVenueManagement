namespace EventVenueManagementAPI.Controller;


public interface GetController<out T>
{
    public T Execute();
}

public interface PostController<in T>
{
    public void Execute(T input);
}

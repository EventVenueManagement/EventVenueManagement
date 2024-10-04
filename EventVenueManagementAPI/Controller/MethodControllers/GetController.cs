namespace EventVenueManagementAPI.Controller.MethodControllers;


public interface GetController<out T>
{
    public T Execute();
}

public interface GetController<in T, out K>
{
    public K Execute(T input);
}
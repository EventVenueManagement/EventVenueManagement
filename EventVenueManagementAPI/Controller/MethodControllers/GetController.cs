namespace EventVenueManagementAPI.Controller.MethodControllers;


public interface GetController<out T>
{
    public T Execute();
}
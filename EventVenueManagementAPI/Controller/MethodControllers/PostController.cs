namespace EventVenueManagementAPI.Controller.MethodControllers;

public interface PostController<in T>
{
    public HttpResponseMessage Execute(T input);
}
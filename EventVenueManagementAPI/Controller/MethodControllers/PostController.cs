namespace EventVenueManagementAPI.Controller.MethodControllers;

public interface PostController<in T>
{
    public HttpResponseMessage Execute(T input);
}
public interface PostController<in T, out K>
{
    public K Execute(T input);
}
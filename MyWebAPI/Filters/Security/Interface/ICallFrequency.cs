namespace MyWebAPI.Filters.Security.Interface
{
    public interface ICallFrequency
    {
        void Validate(string appId);
    }
}

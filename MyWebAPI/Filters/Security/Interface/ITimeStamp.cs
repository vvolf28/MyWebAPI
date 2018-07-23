namespace MyWebAPI.Filters.Security.Interface
{
    public interface ITimeStamp
    {
        void Validate(string timestamp);
    }
}

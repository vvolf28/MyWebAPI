using MyWebAPI.Filters.Security.Interface;

namespace MyWebAPI.Filters.Security.DefaultHandle
{
    public class DefaultCallFrequency : ICallFrequency
    {
        public void Validate(string appId)
        {
            return;
        }
    }
}
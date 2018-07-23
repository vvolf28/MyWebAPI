using System.Collections.Generic;

namespace MyWebAPI.Filters.Security.Interface
{
    public interface IIpAddress
    {
        void Validate(List<string> accessIpList);
    }
}

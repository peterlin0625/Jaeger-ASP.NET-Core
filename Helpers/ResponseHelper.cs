using System;

namespace Helpers
{
    public class ResponseHelper
    {
        public string GetResponse(string[] values)
        {
            return $"return \"{string.Join(',', values)}\" @ {DateTimeOffset.UtcNow}";
        }
    }
}
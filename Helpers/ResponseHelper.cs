using System;
using System.Collections.Generic;
using OpenTracing;

namespace Helpers
{
    public class ResponseHelper
    {
        private readonly ITracer _tracer;

        public ResponseHelper(ITracer tracer)
        {
            _tracer = tracer;
        }

        public string GetResponse(string[] values)
        {
            using (IScope scope = _tracer.BuildSpan(GetType().Name).StartActive(finishSpanOnDispose: true))
            {
                string output = $"return \"{string.Join(',', values)}\" @ {DateTimeOffset.UtcNow}";
                scope.Span.SetTag("module", $"{GetType().Name}:{System.Reflection.MethodBase.GetCurrentMethod().Name}");

                scope.Span.Log(new Dictionary<string, object>
                {
                    ["module"] = GetType().Name,
                    ["function"] = System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ["eventTime"] = DateTime.Now,
                    ["input"] = values,
                    ["output"] = output
                });

                return output;
            }
        }
    }
}
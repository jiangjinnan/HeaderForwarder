using HeaderForwarder;
using System.Diagnostics;

var processor = OutgoingHeaderProcessor.Create("foo","bar");
var traceId = ActivityTraceId.CreateRandom().ToHexString();
using (processor.AddHeaders(("apm-traceid",traceId),("foo", "123"), ("bar", "456"), ("baz", "789")))
{
    await new HttpClient().GetAsync("http://localhost:5000/test");
}
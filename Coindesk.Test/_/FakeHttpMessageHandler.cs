using System.Net;

namespace Coindesk.Test;
public class FakeHttpMessageHandler : HttpMessageHandler
{
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var content = new StringContent(@"{
                                              ""time"": {
                                                ""updated"": ""Dec 01, 2015 13:58:08 UTC"",
                                                ""updatedISO"": ""1997-02-15T02:58:08+00:00"",
                                                ""updateduk"": ""Dec 31, 2022 at 21:15 GMT""
                                              },
                                              ""disclaimer"": ""OuO"",
                                              ""chartName"": ""RRRRRR你好"",
                                              ""bpi"": {
                                                ""USD"": {
                                                  ""code"": ""USD"",
                                                  ""symbol"": ""&#36;"",
                                                  ""rate"": ""999,888.777"",
                                                  ""description"": ""我是美金唷"",
                                                  ""rate_float"": 999888.777
                                                },
                                                ""GBP"": {
                                                  ""code"": ""GBP"",
                                                  ""symbol"": ""&pound;"",
                                                  ""rate"": ""79,277.927"",
                                                  ""description"": ""British Pound Sterling"",
                                                  ""rate_float"": 79277.9273
                                                },
                                                ""XXX"": {
                                                  ""code"": ""AuA"",
                                                  ""symbol"": ""aaaaaaaaaa"",
                                                  ""rate"": ""123"",
                                                  ""description"": ""啊啊啊啊啊啊"",
                                                  ""rate_float"": 123
                                                },
                                                ""JPY"": {
                                                  ""code"": ""JPY"",
                                                  ""symbol"": ""Y"",
                                                  ""rate"": ""1,548.7"",
                                                  ""description"": ""日幣呀"",
                                                  ""rate_float"": 1548.7
                                                }
                                              }
                                            }");

        return await Task.Run(() => new HttpResponseMessage()
        {
            Content = content,
            StatusCode = HttpStatusCode.OK
        });
    }
}
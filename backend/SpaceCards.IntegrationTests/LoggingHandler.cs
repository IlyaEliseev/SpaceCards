﻿using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SpaceCards.IntegrationTests
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ITestOutputHelper _outputHelper;

        public LoggingHandler(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        private string ReadFromStream(HttpContent content)
        {
            var contentStream = content.ReadAsStream();
            var resultBytes = new byte[contentStream.Length];
            contentStream.Read(resultBytes, 0, resultBytes.Length);
            var contentString = Encoding.UTF8.GetString(resultBytes);

            return contentString;
        }

        private void PrintJsonContent(HttpContent content)
        {
            var contentJson = ReadFromStream(content);
            if (contentJson is not null)
            {
                var json = JToken.Parse(contentJson).ToString();
                _outputHelper.WriteLine(json);
            }
        }

        private async Task PrintJsonContentAsync(HttpContent content)
        {
            var contentJson = await content.ReadAsStringAsync();
            if (contentJson is not null && contentJson.Length != 0)
            {
                var json = JToken.Parse(contentJson).ToString();
                _outputHelper.WriteLine(json);
            }
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content is not null)
            {
                PrintJsonContent(request.Content);
            }

            var response = base.Send(request, cancellationToken);
            if (response.Content is not null)
            {
                _outputHelper.WriteLine(request.RequestUri.ToString());
                PrintJsonContent(request.Content);
            }

            return response;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content is not null)
            {
                await PrintJsonContentAsync(request.Content);
            }

            var response = await base.SendAsync(request, cancellationToken);
            if (response.Content is not null)
            {
                _outputHelper.WriteLine(request.RequestUri.ToString());
                await PrintJsonContentAsync(response.Content);
            }

            return response;
        }
    }
}

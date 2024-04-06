using AuctionSystem.Service.Contract;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Implementation
{
    public class ClientFactory : IClientFactory
    {
        private readonly ILogger<ClientFactory> _logger;

        public ClientFactory(ILogger<ClientFactory> logger)
        {
            _logger = logger;
        }

        public async Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpResponse = await client.PostAsync(endPoint, content);

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

        public async Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto, string authorizationHeader)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeader);

                var httpResponse = await client.PostAsync(endPoint, content);

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

        public async Task<SampleResponse> PostDataAsyncForm<SampleResponse, SampleRequest>(string endPoint, EmailRequest dto, string authorizationHeader)
        {
            try
            {
                using var client = new HttpClient();

                var boundary = Guid.NewGuid().ToString();

                var content = new MultipartFormDataContent(boundary);

                content.Add(new StringContent(dto.From ?? string.Empty), "From");
                content.Add(new StringContent(dto.DisplayName ?? string.Empty), "DisplayName");
                content.Add(new StringContent(dto.To ?? string.Empty), "To");
                content.Add(new StringContent(dto.Cc ?? string.Empty), "Cc");
                content.Add(new StringContent(dto.Bcc ?? string.Empty), "Bcc");
                content.Add(new StringContent(dto.MailMessage ?? string.Empty), "MailMessage");
                content.Add(new StringContent(dto.Subject ?? string.Empty), "Subject");

                if (dto.Attachments != null)
                {
                    foreach (var file in dto.Attachments)
                    {
                        using var fileStream = file.OpenReadStream();

                        var memoryStream = new MemoryStream();
                        await fileStream.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileContent = new StreamContent(memoryStream);
                        content.Add(fileContent, "Attachments", file.FileName);
                    }
                }


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeader);

                Stopwatch timer = new Stopwatch();
                timer.Start();

                var httpResponse = await client.PostAsync(endPoint, content);

                timer.Stop();
                if (timer.ElapsedMilliseconds > 1000)
                {
                    _logger.LogWarning($"Response for {endPoint} took {timer.ElapsedMilliseconds}ms to complete");
                }

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }

        }

        public async Task<SampleResponse> GetDataAsync<SampleResponse>(string endPoint)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpResponse = await client.GetAsync(endPoint);

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

    }
}

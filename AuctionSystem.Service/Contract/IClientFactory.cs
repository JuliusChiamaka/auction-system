using AuctionSystem.Service.DTO.Request;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract
{
    public interface IClientFactory
    {
        Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto);
        Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto, string authorizationHeader);
        Task<SampleResponse> PostDataAsyncForm<SampleResponse, SampleRequest>(string endPoint, EmailRequest dto, string authorizationHeader);
        Task<SampleResponse> GetDataAsync<SampleResponse>(string endPoint);
    }
}

using Blazored.LocalStorage;
using InterviewApp.Helper;
using InterviewApp.IServices;
using InterviewApp.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace InterviewApp.Services
{
    public class GearService : IGearService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public GearService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public Task DeleteGearAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<GearViewModel[]> GetGearHistoryAsync(string gearType, int pageIndex, int pageSize)
        {
            int gearTypeId = gearType == "ski" ? 1 : 2;
            return (await _httpClient.GetFromJsonAsync<GearViewModel[]>("sample-data/travaux_realises.json"))
                    .Where(x => x.GearTypeId == gearTypeId)
                    .Select(x => new GearViewModel
                    {
                        Description = x.Description,
                        DurationPerMin = x.DurationPerMin,
                        GearTypeName = gearType ,
                        GearTypeId = gearTypeId,
                        Price = x.Price,
                        SerialNumber = x.SerialNumber,
                        TechnicienUserName = x.TechnicienUserName,
                        EmailClient = x.EmailClient,
                    })
                    .ToArray();
        }
    }
}

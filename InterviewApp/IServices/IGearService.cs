using InterviewApp.Models;

namespace InterviewApp.IServices
{
    public interface IGearService
    {
        Task<GearViewModel[]> GetGearHistoryAsync(string gearType, int pageIndex, int pageSize);
        Task DeleteGearAsync(int id);
    }
}

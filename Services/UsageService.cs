using Microsoft.EntityFrameworkCore;
using MobileProvider.Data;
using MobileProvider.Models.DTOs;
using MobileProvider.Models.Entities;

namespace MobileProvider.Services
{
    public class UsageService
    {
        private readonly MobileProviderDbContext _context;

        public UsageService(MobileProviderDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUsageAsync(AddUsageDto dto)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.SubscriberNo == dto.SubscriberNo);

            if (subscriber == null)
                return false;

            if (dto.Amount <= 0)
                return false;


            var usage = new Usage
            {
                SubscriberId = subscriber.Id,
                Month = dto.Month,
                Year = dto.Year,
                Type = dto.Type,
                Amount = dto.Amount
            };

            _context.Usages.Add(usage);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

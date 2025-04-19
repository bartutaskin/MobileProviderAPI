using Microsoft.EntityFrameworkCore;
using MobileProvider.Data;
using MobileProvider.Models.DTOs;
using MobileProvider.Models.Entities;

namespace MobileProvider.Services
{
    public class BillService
    {
        private readonly MobileProviderDbContext _context;

        public BillService(MobileProviderDbContext context)
        {
            _context = context;
        }

        public decimal CalculateBill(CalculateBillDto dto)
        {
            var subscriber = _context.Subscribers
                .FirstOrDefault(s => s.SubscriberNo == dto.SubscriberNo);

            if (subscriber == null)
            {
                throw new InvalidOperationException("Subscriber not found.");
            }

            var phoneUsage = _context.Usages
                .Where(u => u.SubscriberId == subscriber.Id && u.Type.ToLower() == "phone" && u.Month == dto.Month && u.Year == dto.Year)
                .Sum(u => u.Amount);

            var internetUsage = _context.Usages
                .Where(u => u.SubscriberId == subscriber.Id && u.Type.ToLower() == "internet" && u.Month == dto.Month && u.Year == dto.Year)
                .Sum(u => u.Amount);

            decimal billAmount = 0;

            if (phoneUsage > 1000)
            {
                billAmount += (Math.Ceiling((phoneUsage - 1000) / 1000m) * 10m); 
            }

            if (internetUsage > 20)
            {
                billAmount += (Math.Ceiling((internetUsage - 20) / 10m) * 10m); 
            }

            var bill = new Bill
            {
                SubscriberId = subscriber.Id,
                Month = dto.Month,
                Year = dto.Year,
                TotalAmount = billAmount,
                PaidStatus = false 
            };

            _context.Bills.Add(bill);
            _context.SaveChanges();

            return billAmount;
        }
    }
}

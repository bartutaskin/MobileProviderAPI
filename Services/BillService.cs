using Microsoft.AspNetCore.Mvc.RazorPages;
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
                PhoneCharge = phoneUsage,  
                InternetCharge = internetUsage, 
                PaidStatus = false
            };

            _context.Bills.Add(bill);
            _context.SaveChanges();

            return billAmount;
        }

        public Bill GetBillSummary(string subscriberNo, int month, int year)
        {
            var subscriber = _context.Subscribers.FirstOrDefault(u => u.SubscriberNo == subscriberNo);

            if (subscriber == null)
            {
                throw new InvalidOperationException("Subscriber not found.");
            }

            var bill = _context.Bills.Include(b => b.Subscriber).FirstOrDefault(b => b.SubscriberId == subscriber.Id && b.Month == month && b.Year == year);

            if (bill == null)
            {
                throw new InvalidOperationException("Bill not found.");
            }

            return bill;
        }

        public PagedResult<BillDetailsDto> GetBillDetails(string subscriberNo, int year, int page, int pageSize)
        {
            var subscriber = _context.Subscribers.FirstOrDefault(s => s.SubscriberNo == subscriberNo);
            if (subscriber == null)
                throw new InvalidOperationException("Subscriber not found.");

            var query = _context.Bills
                .Where(b => b.SubscriberId == subscriber.Id && b.Year == year);

            var totalCount = query.Count();

            var bills = query
            .OrderBy(b => b.Month)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BillDetailsDto
                {
                    TotalAmount = b.TotalAmount,
                    PhoneCharge = b.PhoneCharge,
                    InternetCharge = b.InternetCharge,
                    PaidStatus = b.PaidStatus,
                    Month = b.Month
                })
                .ToList();

            return new PagedResult<BillDetailsDto>
            {
                Items = bills,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<string> PayBillAsync(PayBillDto dto)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.SubscriberNo == dto.SubscriberNo);
            if (subscriber == null)
            {
                throw new InvalidOperationException("Subscriber not found.");
            }
            var bill = await _context.Bills
                .FirstOrDefaultAsync(b => b.SubscriberId == subscriber.Id && b.Month == dto.Month && b.Year == dto.Year);
            if (bill == null)
            {
                throw new InvalidOperationException("Bill not found.");
            }
            if (bill.PaidStatus)
            {
                return "Bill already paid.";
            }
            if (dto.PaymentAmount < bill.TotalAmount)
            {
                return "Insufficient payment amount.";
            }
            bill.PaidStatus = true;
            await _context.SaveChangesAsync();
            return "Payment successful.";
        }
    }
}

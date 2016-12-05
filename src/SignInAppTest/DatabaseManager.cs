using System;
using System.Linq;
using System.Threading;
using DaVinciCollegeAuthenticationService.Data;

namespace DaVinciCollegeAuthenticationService
{
    public class DatabaseManager
    {
        private readonly ApplicationDbContext _context;
        private readonly Timer _timer;

        public DatabaseManager(ApplicationDbContext context)
        {
            _context = context;
            _timer = new Timer(RemoveExpiredData, new TimerState {Timer = _timer, Canceled = false}, 0, TimeSpan.FromHours(24).Seconds);
        }


        public void RemoveExpiredData(object state)
        {
            var expiredResets = _context.PasswordResets.Where(pr => pr.ValidTill < DateTime.Now).ToList();
            _context.PasswordResets.RemoveRange(expiredResets);

            var expiredAccesstokens = _context.Accesstokens.Where(t => t.ValidTill < DateTime.Now).ToList();
            _context.Accesstokens.RemoveRange(expiredAccesstokens);

            _context.SaveChanges();
        }

        private class TimerState
        {
            public Timer Timer { get; set; }
            public bool Canceled { get; set; }
        }
    }
}
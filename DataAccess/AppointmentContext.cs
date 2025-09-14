using AppointmentPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentPlanner.DataAccess
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
        {
        }

        public DbSet<Hospital> Hospitals {get;set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<WaitingList> WaitingLists { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Fleet> Fleets { get; set; }
        public DbSet<Depot> Depots { get; set; }
       


    }
}

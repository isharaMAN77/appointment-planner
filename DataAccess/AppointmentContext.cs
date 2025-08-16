using AppointmentPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentPlanner.DataAccess
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
        {
        }

        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<WaitingList> WaitingLists { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { DepartmentId = 1, Id = "generalmedicine", Text = "General Medicine", Color = "#F538B2" },
                new Specialization { DepartmentId = 2, Id = "neurology", Text = "Neurology", Color = "#33C7E8" },
                new Specialization { DepartmentId = 3, Id = "dermatology", Text = "Dermatology", Color = "#916DE4" },
                new Specialization { DepartmentId = 4, Id = "orthopedics", Text = "Orthopedics", Color = "#388CF5" },
                new Specialization { DepartmentId = 5, Id = "diabetology", Text = "Diabetology", Color = "#60F238" },
                new Specialization { DepartmentId = 6, Id = "cardiology", Text = "Cardiology", Color = "#F29438" }
            );

            modelBuilder.Entity<Doctor>().HasData(
                new { Id = 1, Name = "Nembo Lukeni", Gender = "Male", Text = "NemboLukni", DepartmentId = 1, Color = "#ea7a57", Education = "MBBS, DMRD", Specialization = "generalmedicine", Experience = "10+ years", Designation = "Senior Specialist", DutyTiming = "Shift1", Email = "nembo36@sample.com", Mobile = "(206) 555-9482", Availability = "busy", StartHour = "08:00", EndHour = "17:00", NewDoctorClass = "" },
                new { Id = 2, Name = "Mollie Cobb", Gender = "Female", Text = "MollieCobb", DepartmentId = 2, Color = "#7fa900", Education = "MBBS, MD PAEDIATRICS, DM NEUROLOGY", Specialization = "neurology", Experience = "2+ years", Designation = "Junior Specialist", DutyTiming = "Shift2", Email = "mollie65@rpy.com", Mobile = "(206) 555-3412", Availability = "available", StartHour = "10:00", EndHour = "19:00", NewDoctorClass = "" },
                new { Id = 3, Name = "Yara Barros", Gender = "Female", Text = "YaraBarros", DepartmentId = 1, Color = "#fec200", Education = "MBBS, DNB (FAMILY MEDICINE)", Specialization = "generalmedicine", Experience = "5+ years", Designation = "Senior Specialist", DutyTiming = "Shift3", Email = "yara105@sample.com", Mobile = "(206) 555-8122", Availability = "away", StartHour = "12:00", EndHour = "21:00", NewDoctorClass = "" },
                new { Id = 4, Name = "Paul Walker", Gender = "Male", Text = "PaulWalker", DepartmentId = 3, Color = "#865fcf", Education = "MBBS, MD (Dermatology)", Specialization = "dermatology", Experience = "10+ years", Designation = "Senior Specialist", DutyTiming = "Shift1", Email = "paul39@mail.com", Mobile = "(071) 555-4848", Availability = "busy", StartHour = "08:00", EndHour = "17:00", NewDoctorClass = "" },
                new { Id = 5, Name = "Amelia Edwards", Gender = "Female", Text = "AmeliaEdwards", DepartmentId = 4, Color = "#1aaa55", Education = "MBBS, MD", Specialization = "orthopedics", Experience = "10+ years", Designation = "Junior Specialist", DutyTiming = "Shift2", Email = "amelia101@rpy.com", Mobile = "(071) 555-7773", Availability = "available", StartHour = "10:00", EndHour = "19:00", NewDoctorClass = "" },
                new { Id = 6, Name = "Alexa Richardson", Gender = "Female", Text = "AlexaRichardson", DepartmentId = 5, Color = "#1aaa55", Education = "MBBS, MD", Specialization = "diabetology", Experience = "1+ years", Designation = "Practitioner", DutyTiming = "Shift2", Email = "alexa55@sample.com", Mobile = "(071) 555-5598", Availability = "busy", StartHour = "10:00", EndHour = "19:00", NewDoctorClass = "" },
                new { Id = 7, Name = "Amelia Nout Golstein", Gender = "Male", Text = "NoutGolstein", DepartmentId = 6, Color = "#00bdae", Education = "MS", Specialization = "cardiology", Experience = "2+ years", Designation = "Junior Specialist", DutyTiming = "Shift3", Email = "nout49@rpy.com", Mobile = "(206) 555-1189", Availability = "busy", StartHour = "12:00", EndHour = "21:00", NewDoctorClass = "" }
            );

            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, Name = "Laura", Text = "Laura", DOB = new DateTime(1980, 8, 3), Mobile = "(071) 555-4444", Email = "laura90@mail.com", Address = "507 - 20th Ave. E.\r\nApt. 2A", Disease = "Eye Checkup", DepartmentName = "GENERAL", BloodGroup = "O +ve", Gender = "Female", Symptoms = "Sweating, Chills and Shivering" },
                new Patient { Id = 2, Name = "Milka", Text = "Milka", DOB = new DateTime(2000, 3, 5), Mobile = "(071) 555-4445", Email = "milka40@sample.com", Address = "908 W. Capital Way", Disease = "Bone Fracture", DepartmentName = "ORTHOPEDICS", BloodGroup = "AB +ve", Gender = "Female", Symptoms = "Swelling or bruising over a bone, Pain in the injured area" },
                new Patient { Id = 3, Name = "Adams", Text = "Adams", DOB = new DateTime(1985, 2, 3), Mobile = "(071) 555-4454", Email = "adams89@rpy.com", Address = "722 Moss Bay Blvd.", Disease = "Eye and Spectactles", DepartmentName = "GENERAL", BloodGroup = "B +ve", Gender = "Male", Symptoms = "Frequent squinting, Eye fatigue or strain" },
                new Patient { Id = 4, Name = "Janet", Text = "Janet", DOB = new DateTime(2000, 7, 3), Mobile = "(071) 555-4544", Email = "janet79@rpy.com", Address = "4110 Old Redmond Rd.", Disease = "Biological Problem", DepartmentName = "GENERAL", BloodGroup = "B +ve", Gender = "Male", Symptoms = "Physical aches or pain, Memory difficulties or personality change" },
                new Patient { Id = 5, Name = "Mercy", Text = "Mercy", DOB = new DateTime(2005, 4, 29), Mobile = "(071) 555-5444", Email = "mercy60@sample.com", Address = "14 Garrett Hill", Disease = "Skin Hives", DepartmentName = "DERMATOLOGY", BloodGroup = "AB -ve", Gender = "Female", Symptoms = "outbreak of swollen, pale red bumps or plaques" },
                new Patient { Id = 6, Name = "Richa", Text = "Richa", DOB = new DateTime(1989, 10, 29), Mobile = "(206) 555-4444", Email = "richa46@mail.com", Address = "Coventry House\r\nMiner Rd.", Disease = "Arm Fracture", DepartmentName = "ORTHOPEDICS", BloodGroup = "B +ve", Gender = "Female", Symptoms = "Swelling, warmth, or redness in the joint" },
                new Patient { Id = 7, Name = "Maud Oliver", Text = "Maud Oliver", DOB = new DateTime(1989, 10, 29), Mobile = "(206) 666-4444", Email = "moud46@rpy.com", Address = "Coventry House\r\nMiner Rd.", Disease = "Racing heartbeat", DepartmentName = "CARDIOLOGY", BloodGroup = "B +ve", Gender = "Male", Symptoms = "A fluttering in your chest" }
            );

            modelBuilder.Entity<Hospital>().HasData(
                new { Id = 1000, Name = "Milka", StartTime = new DateTime(2020, 2, 5, 10, 30, 0), EndTime = new DateTime(2020, 2, 5, 11, 30, 0), Disease = "Bone Fracture", DepartmentName = "ORTHOPEDICS", DepartmentId = 4, DoctorId = 5, PatientId = 2, Symptoms = "Swelling or bruising over a bone, Pain in the injured area", IsBlock = false },
                new { Id = 1001, Name = "Janet", StartTime = new DateTime(2020, 2, 3, 11, 0, 0), EndTime = new DateTime(2020, 2, 3, 12, 0, 0), Disease = "Biological Problems", DepartmentName = "GENERAL", DepartmentId = 1, DoctorId = 3, PatientId = 4, Symptoms = "Physical aches or pain, Memory difficulties or personality changes", IsBlock = false },
                new { Id = 1002, Name = "Mercy", StartTime = new DateTime(2020, 2, 2, 10, 0, 0), EndTime = new DateTime(2020, 2, 2, 11, 0, 0), Disease = "Skin Problem", DepartmentName = "DERMATOLOGY", DepartmentId = 3, DoctorId = 4, PatientId = 5, Symptoms = "outbreak of swollen, pale red bumps or plaques", IsBlock = false }
            );

            modelBuilder.Entity<WaitingList>().HasData(
                new WaitingList { Id = 1, Name = "Laura", StartTime = new DateTime(2020, 2, 3, 8, 30, 0), EndTime = new DateTime(2020, 2, 3, 9, 30, 0), Disease = "Sudden loss of vision", DepartmentName = "GENERAL", Treatment = "CHECKUP", DepartmentId = 1, PatientId = 1 },
                new WaitingList { Id = 2, Name = "Milka", StartTime = new DateTime(2020, 2, 4, 8, 30, 0), EndTime = new DateTime(2020, 2, 4, 10, 30, 0), Disease = "Bone Fracture", DepartmentName = "ORTHOPEDICS", Treatment = "SURGERY", DepartmentId = 4, PatientId = 2 },
                new WaitingList { Id = 3, Name = "Adams", StartTime = new DateTime(2020, 2, 4, 9, 30, 0), EndTime = new DateTime(2020, 2, 4, 10, 30, 0), Disease = "Skin Hives", DepartmentName = "DERMATOLOGY", Treatment = "CHECKUP", DepartmentId = 3, PatientId = 3 }
            );

            modelBuilder.Entity<Activity>().HasData(
                new Activity { Id = 1, Name = "Added New Doctor", Message = "Dr.Johnson James, Cardiologist", Time = "5 mins ago", Type = "doctor", ActivityTime = new DateTime(2020, 2, 1, 9, 0, 0) },
                new Activity { Id = 2, Name = "Added New Appointment", Message = "Laura for General Checkup on 7th March, 2020 @ 8.30 AM with Dr.Molli Cobb, Cardiologist", Time = "5 mins ago", Type = "appointment", ActivityTime = new DateTime(2020, 2, 1, 11, 0, 0) },
                new Activity { Id = 3, Name = "Added New Patient", Message = "James Richard for Fever and cold", Time = "5 mins ago", Type = "patient", ActivityTime = new DateTime(2020, 2, 1, 10, 0, 0) }
            );

            modelBuilder.Entity<WorkDay>().HasData(
                new { Id = 1, Day = "Sunday", Index = 0, Enable = true, WorkStartHour = new DateTime(2020, 2, 1, 8, 0, 0), WorkEndHour = new DateTime(2020, 2, 1, 17, 0, 0), BreakStartHour = new DateTime(2020, 2, 1, 12, 0, 0), BreakEndHour = new DateTime(2020, 2, 1, 13, 0, 0), State = "AddBreak", DoctorId = 1 },
                new { Id = 2, Day = "Monday", Index = 1, Enable = false, WorkStartHour = new DateTime(2020, 2, 2, 8, 0, 0), WorkEndHour = new DateTime(2020, 2, 2, 17, 0, 0), BreakStartHour = new DateTime(2020, 2, 2, 12, 0, 0), BreakEndHour = new DateTime(2020, 2, 2, 13, 0, 0), State = "TimeOff", DoctorId = 1 },
                new { Id = 3, Day = "Tuesday", Index = 2, Enable = true, WorkStartHour = new DateTime(2020, 2, 3, 10, 0, 0), WorkEndHour = new DateTime(2020, 2, 3, 19, 0, 0), BreakStartHour = new DateTime(2020, 2, 3, 14, 0, 0), BreakEndHour = new DateTime(2020, 2, 3, 15, 0, 0), State = "AddBreak", DoctorId = 2 },
                new { Id = 4, Day = "Wednesday", Index = 3, Enable = true, WorkStartHour = new DateTime(2020, 2, 4, 10, 0, 0), WorkEndHour = new DateTime(2020, 2, 4, 19, 0, 0), BreakStartHour = new DateTime(2020, 2, 4, 14, 0, 0), BreakEndHour = new DateTime(2020, 2, 4, 15, 0, 0), State = "AddBreak", DoctorId = 2 }
            );
        }
    }
}

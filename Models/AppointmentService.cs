using System.Globalization;
using AppointmentPlanner.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AppointmentPlanner.Models
{
    public class AppointmentService
    {
        private readonly AppointmentContext _context;

        public AppointmentService(AppointmentContext context)
        {
            _context = context;
            StartDate = new DateTime(2020, 2, 5, 0, 0, 0, 0);
            ActiveDoctors = _context.Doctors.FirstOrDefault();
            ActivePatients = _context.Patients.FirstOrDefault();
            StartHours = DataProvider.GetStartHours();
            EndHours = DataProvider.GetEndHours();
            Views = DataProvider.GetViews();
            ColorCategory = DataProvider.GetColorCategory();
            BloodGroups = DataProvider.GetBloodGroupData();
            DayOfWeekList = DataProvider.GetDayOfWeekList();
            TimeSlot = DataProvider.GetTimeSlot();
            DutyTimings = DataProvider.DutyTimingsData();
            Experience = DataProvider.ExperienceData();
            CalendarSettings = new CalendarSetting { BookingColor = "Doctors", Calendar = new Calendar { Start = "08:00", End = "21:00" }, CurrentView = "Week", Interval = 60, FirstDayOfWeek = 0 };
        }
        public DateTime StartDate { get; set; }
        public Doctor ActiveDoctors { get; set; }
        public Patient ActivePatients { get; set; }
        public List<TextValueData> StartHours { get; set; }
        public List<TextValueData> EndHours { get; set; }
        public List<TextValueData> Views { get; set; }
        public List<TextValueData> ColorCategory { get; set; }
        public List<TextValueData> BloodGroups { get; set; }
        public List<TextValueNumericData> DayOfWeekList { get; set; }
        public List<TextValueNumericData> TimeSlot { get; set; }
        public IQueryable<Hospital> Hospitals => _context.Hospitals;
        public IQueryable<Patient> Patients => _context.Patients;
        public IQueryable<Doctor> Doctors => _context.Doctors;
        public List<Doctor> FilteredDoctors { get; set; }
        public IQueryable<WaitingList> WaitingLists => _context.WaitingLists;
        public IQueryable<Specialization> Specializations => _context.Specializations;
        public List<TextIdData> DutyTimings { get; set; }
        public List<TextIdData> Experience { get; set; }
        public IQueryable<Activity> Activities => _context.Activities;
        public CalendarSetting CalendarSettings { get; set; }

        public DateTime GetWeekFirstDate(DateTime date)
        {
            return date.AddDays(DayOfWeek.Monday - date.DayOfWeek);
        }

        public string GetFormatDate(DateTime date, string type)
        {
            return date.ToString(type, CultureInfo.InvariantCulture);
        }

        public string TimeSince(DateTime activityTime)
        {
            if (Math.Round((DateTime.Now - activityTime).Days / (365.25 / 12)) > 0)
            {
                return Math.Round((DateTime.Now - activityTime).Days / (365.25 / 12)).ToString() + " months ago";
            }
            else if (Math.Round((DateTime.Now - activityTime).TotalDays) > 0)
            {
                return Math.Round((DateTime.Now - activityTime).TotalDays).ToString() + " days ago";
            }
            else if (Math.Round((DateTime.Now - activityTime).TotalHours) > 0)
            {
                return Math.Round((DateTime.Now - activityTime).TotalHours).ToString() + " hours ago";
            }
            else if (Math.Round((DateTime.Now - activityTime).TotalMinutes) > 0)
            {
                return Math.Round((DateTime.Now - activityTime).TotalMinutes).ToString() + " mins ago";
            }
            else if (Math.Round((DateTime.Now - activityTime).TotalSeconds) > 0)
            {
                return Math.Round((DateTime.Now - activityTime).TotalSeconds).ToString() + " seconds ago";
            }
            return Math.Round((DateTime.Now - activityTime).TotalMilliseconds).ToString() + " milliSeconds ago";
        }

        public Doctor GetDoctorDetails(int id)
        {
            return _context.Doctors.Include(d => d.WorkDays).FirstOrDefault(i => i.Id.Equals(id));
        }

        public string GetSpecializationText(string text)
        {
            return _context.Specializations.FirstOrDefault(item => item.Id.Equals(text)).Text;
        }
        public string GetAvailability(Doctor doctor)
        {
            List<WorkDay> workDays = doctor.WorkDays.ToList();
            if (workDays != null)
            {
                List<string> result = workDays.Where(item => item.Enable.Equals(true)).Select(item => item.Day.Substring(0, 3).ToUpper()).ToList();
                return string.Join(",", result).ToString();

            }
            return string.Empty;
        }

        public List<Hospital> GetFilteredData(DateTime StartDate, DateTime EndDate)
        {
            return _context.Hospitals.Where(hospital => hospital.StartTime >= StartDate && hospital.EndTime <= EndDate).ToList();
        }

        public ChartData GetChartData(List<Hospital> hospitals, DateTime startDate)
        {
            int eventCount = hospitals.Where(hospital => ResetTime(hospital.StartTime) == ResetTime(startDate)).Count();
            return new ChartData() { Date = startDate, EventCount = eventCount };
        }


        public DateTime ResetTime(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public List<ChartData> GetAllChartPoints(List<Hospital> hospitals, DateTime date)
        {
            List<ChartData> chartPoints = new List<ChartData>();
            for (int i = 0; i < 7; i++)
            {
                chartPoints.Add(GetChartData(hospitals, date));
                date = date.AddDays(1);
            }
            return chartPoints;
        }

        public void UpdatePreference(Params param)
        {
            switch (param.Prop)
            {
                case "CurrentView":
                    CalendarSettings.CurrentView = param.Value;
                    break;
                case "CalendarStart":
                    CalendarSettings.Calendar.Start = param.Value;
                    break;
                case "CalendarEnd":
                    CalendarSettings.Calendar.End = param.Value;
                    break;
                case "Duration":
                    CalendarSettings.Interval = Convert.ToInt32(param.Value);
                    break;
                case "BookingColor":
                    CalendarSettings.BookingColor = param.Value;
                    break;
                case "FirstDayOfWeek":
                    CalendarSettings.FirstDayOfWeek = Convert.ToInt32(param.Value);
                    break;
            }
        }

        public void UpdateDoctors(Params param)
        {
            if (!string.IsNullOrEmpty(param.Value))
            {
                FilteredDoctors = _context.Doctors.Where(item => item.DepartmentId.Equals(Convert.ToInt32(param.Value))).ToList();
            }
            else
            {
                FilteredDoctors = _context.Doctors.ToList();
            }
        }
    }
}
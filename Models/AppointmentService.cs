using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using AppointmentPlanner.DataAccess;

namespace AppointmentPlanner.Models
{
    public class AppointmentService
    {
        private readonly DatabaseHelper _dbHelper;

        public AppointmentService(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
            StartDate = new DateTime(2020, 2, 5, 0, 0, 0, 0);
            ActiveDoctors = Doctors.FirstOrDefault();
            ActivePatients = Patients.FirstOrDefault();
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
        public List<Hospital> Hospitals => DataTableHelper.ToList<Hospital>(_dbHelper.ExecuteQuery(DataProvider.GetHospitalsQuery()));
        public List<Patient> Patients => DataTableHelper.ToList<Patient>(_dbHelper.ExecuteQuery(DataProvider.GetPatientsQuery()));
        public List<Doctor> Doctors => DataTableHelper.ToList<Doctor>(_dbHelper.ExecuteQuery(DataProvider.GetDoctorsQuery()));
        public List<Doctor> FilteredDoctors { get; set; }
        public List<WaitingList> WaitingLists => DataTableHelper.ToList<WaitingList>(_dbHelper.ExecuteQuery(DataProvider.GetWaitingListsQuery()));
        public List<Specialization> Specializations => DataTableHelper.ToList<Specialization>(_dbHelper.ExecuteQuery(DataProvider.GetSpecializationsQuery()));
        public List<TextIdData> DutyTimings { get; set; }
        public List<TextIdData> Experience { get; set; }
        public List<Activity> Activities => DataTableHelper.ToList<Activity>(_dbHelper.ExecuteQuery(DataProvider.GetActivitiesQuery()));
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
            var parameter = new SqlParameter("@Id", id);
            var doctor = DataTableHelper.ToList<Doctor>(_dbHelper.ExecuteQuery(DataProvider.GetDoctorByIdQuery(), new[] { parameter })).FirstOrDefault();
            if (doctor != null)
            {
                var workDaysParameter = new SqlParameter("@DoctorId", doctor.Id);
                doctor.WorkDays = DataTableHelper.ToList<WorkDay>(_dbHelper.ExecuteQuery(DataProvider.GetWorkDaysByDoctorIdQuery(), new[] { workDaysParameter }));
            }
            return doctor;
        }

        public string GetSpecializationText(string text)
        {
            var parameter = new SqlParameter("@Id", text);
            return DataTableHelper.ToList<Specialization>(_dbHelper.ExecuteQuery(DataProvider.GetSpecializationByIdQuery(), new[] { parameter })).FirstOrDefault()?.Text;
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
            var parameters = new[]
            {
                new SqlParameter("@StartTime", StartDate),
                new SqlParameter("@EndTime", EndDate)
            };
            return DataTableHelper.ToList<Hospital>(_dbHelper.ExecuteQuery(DataProvider.GetFilteredHospitalsQuery(), parameters));
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
                var parameter = new SqlParameter("@DepartmentId", Convert.ToInt32(param.Value));
                FilteredDoctors = DataTableHelper.ToList<Doctor>(_dbHelper.ExecuteQuery(DataProvider.GetFilteredDoctorsQuery(), new[] { parameter }));
            }
            else
            {
                FilteredDoctors = Doctors;
            }
        }
    }
}
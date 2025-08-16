using AppointmentPlanner.DataAccess;
using AppointmentPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Syncfusion.EJ2.Schedule;
using System.Data.SqlClient;

namespace AppointmentPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppointmentService service;
        private readonly DatabaseHelper _dbHelper;

        public HomeController(AppointmentService appointmentService, IConfiguration configuration)
        {
            service = appointmentService;
            _dbHelper = new DatabaseHelper(configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult DashBoard()
        {
            DateTime startDate = service.StartDate;
            List<Patient> patients = service.Patients.ToList();
            ViewBag.AvailableDoctors = service.Doctors.ToList();
            ViewBag.Activities = service.Activities.ToList();
            ViewBag.SpecializationsData = service.Specializations.ToList();
            ViewBag.FirstDayOfWeek = service.GetWeekFirstDate(startDate);
            ViewBag.CurrentDayEvents = service.GetFilteredData(startDate, startDate.AddDays(1));
            ViewBag.CurrentViewEvents = service.GetFilteredData(ViewBag.FirstDayOfWeek, ViewBag.FirstDayOfWeek.AddDays(6));
            ViewBag.GridData = GetAppoinment((List<Hospital>)ViewBag.CurrentDayEvents);
            List<Hospital> diabetologyData = (ViewBag.CurrentViewEvents as List<Hospital>).Where(item => item.DepartmentId == 5).ToList();
            List<Hospital> orthopaedicsData = (ViewBag.CurrentViewEvents as List<Hospital>).Where(item => item.DepartmentId == 4).ToList();
            List<Hospital> cardiologyData = (ViewBag.CurrentViewEvents as List<Hospital>).Where(item => item.DepartmentId == 6).ToList();
            ViewBag.ChartData = service.GetAllChartPoints(diabetologyData, ViewBag.FirstDayOfWeek);
            ViewBag.ChartData1 = service.GetAllChartPoints(orthopaedicsData, ViewBag.FirstDayOfWeek);
            ViewBag.ChartData2 = service.GetAllChartPoints(cardiologyData, ViewBag.FirstDayOfWeek);
            return PartialView("DashBoard/DashBoard");
        }

        private List<Appointment> GetAppoinment(List<Hospital> currentDayEvents)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Hospital eventData in currentDayEvents)
            {
                Patient? filteredPatients = service.Patients.FirstOrDefault(item => item.Id.Equals(eventData.PatientId));
                Doctor? filteredDoctors = service.Doctors.FirstOrDefault(item => item.Id.Equals(eventData.DoctorId));
                if (filteredPatients != null && filteredDoctors != null)
                {
                    appointments.Add(new Appointment { Time = service.GetFormatDate(eventData.StartTime, "hh:mm tt"), Name = filteredPatients.Name, DoctorName = filteredDoctors.Name, Symptoms = eventData.Symptoms, DoctorId = filteredDoctors.Id });
                }
            }
            return appointments;
        }

        [HttpGet]
        public PartialViewResult Calendar()
        {
            ViewBag.SelectedDate = service.StartDate;
            ViewBag.StartHour = service.CalendarSettings.Calendar.Start;
            ViewBag.EndHour = service.CalendarSettings.Calendar.End;
            ViewBag.WorkDays = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            ViewBag.CurrentView = (View)Enum.Parse(typeof(View), service.CalendarSettings.CurrentView);
            ViewBag.FirstDayOfWeek = service.CalendarSettings.FirstDayOfWeek;
            ViewBag.EventData = service.Hospitals.ToList();
            ViewBag.BookingColor = service.CalendarSettings.BookingColor;
            ViewBag.ResourceDataSource = service.Doctors.ToList();
            ViewBag.SpecialistCategory = service.Specializations.ToList();
            ViewBag.WaitingList = service.WaitingLists.ToList();
            ViewBag.PatientsData = service.Patients.ToList();
            ViewBag.Interval = service.CalendarSettings.Interval;
            return PartialView("Calendar/Calendar");
        }

        [HttpGet]
        public PartialViewResult Doctors()
        {
            ViewBag.SpecializationData = service.Specializations.ToList();
            ViewBag.Doctors = service.Doctors.ToList();
            ViewBag.FilteredDoctors = (service.FilteredDoctors ?? service.Doctors.ToList()).ToList();
            return PartialView("Doctor/Doctors");
        }

        [HttpGet]
        public PartialViewResult Patients()
        {
            ViewBag.FilteredPatients = service.Patients.ToList();
            ViewBag.HospitalData = service.Hospitals.ToList();
            ViewBag.Doctors = service.Doctors.ToList();
            return PartialView("Patient/Patients");
        }

        [HttpGet]
        public PartialViewResult Preference()
        {
            ViewBag.SelectedView = service.CalendarSettings.CurrentView;
            ViewBag.Views = service.Views;
            ViewBag.SelectedStartHour = service.CalendarSettings.Calendar.Start;
            ViewBag.StartHours = service.StartHours;
            ViewBag.SelectedEndHour = service.CalendarSettings.Calendar.End;
            ViewBag.EndHours = service.EndHours;
            ViewBag.TimeSlots = service.TimeSlot;
            ViewBag.TimeInterval = service.CalendarSettings.Interval;
            ViewBag.ColorCategory = service.ColorCategory;
            ViewBag.SelectedCategory = service.CalendarSettings.BookingColor;
            ViewBag.DayOfWeeks = service.DayOfWeekList;
            ViewBag.SelectedDayOfWeek = service.CalendarSettings.FirstDayOfWeek;
            return PartialView("Preference/Preference");
        }

        [HttpGet]
        public PartialViewResult About()
        {
            return PartialView("About/About");
        }

        [HttpPost]
        public PartialViewResult DoctorDetails([FromBody] string id)
        {
            int doctorId = Convert.ToInt32(id);
            Doctor activeData = service.GetDoctorDetails(doctorId);
            activeData = activeData ?? service.GetDoctorDetails(1);
            if (activeData == null)
            {
                return PartialView(string.Empty);
            }
            service.ActiveDoctors = activeData;
            activeData.WorkDays = activeData.WorkDays ?? new List<WorkDay>();
            ViewBag.GetSpecializationText = service.GetSpecializationText(activeData.Specialization);
            ViewBag.GetAvailability = service.GetAvailability(activeData);
            ViewBag.SpecializationData = service.Specializations.ToList();
            ViewBag.ExperienceData = service.Experience;
            ViewBag.DutyTimingsData = service.DutyTimings;
            ViewBag.ActiveData = activeData;
            service.FilteredDoctors = null;
            return PartialView("Doctor/DoctorDetails");
        }

        [HttpPost]
        public IActionResult UpdatePreference([FromBody] Params param)
        {
            service.UpdatePreference(param);
            return Ok();
        }

        [HttpPost]
        public PartialViewResult FilterDoctors([FromBody] Params param)
        {
            ViewBag.SpecializationValue = !string.IsNullOrEmpty(param.Value) ? param.Value : null;
            service.UpdateDoctors(param);
            return PartialView("Doctor/DoctorsList", Doctors());
        }

        [HttpPost]

        public PartialViewResult UpdateDoctors([FromBody] Doctor doctor)
        {
            UpdateCalendarDoctors(doctor);
            return PartialView("Doctor/DoctorsList", Doctors());
        }

        [HttpPost]
        public PartialViewResult RefreshDoctorsDialog()
        {
            return PartialView("Calendar/SpecialistDialogContent", Doctors());
        }

        [HttpPost]
        public PartialViewResult RefreshWaitingDialog([FromBody] string[] activeIds)
        {
            UpdateWaitingListData(activeIds);
            return PartialView("Calendar/WaitingListDialogContent");
        }

        [HttpPost]
        public PartialViewResult UpdateDoctorDetail([FromBody] Doctor doctor)
        {
            UpdateCalendarDoctors(doctor);
            return PartialView("Doctor/DoctorDetails", DoctorDetails(doctor.Id.ToString()));
        }

        [HttpPost]
        public IActionResult UpdateBreakHours([FromBody] List<WorkDay> workdays)
        {
            service.ActiveDoctors.WorkDays = workdays;
            foreach (var workday in workdays)
            {
                var parameters = new[]
                {
                    new SqlParameter("@Day", workday.Day),
                    new SqlParameter("@Index", workday.Index),
                    new SqlParameter("@Enable", workday.Enable),
                    new SqlParameter("@WorkStartHour", workday.WorkStartHour),
                    new SqlParameter("@WorkEndHour", workday.WorkEndHour),
                    new SqlParameter("@BreakStartHour", workday.BreakStartHour),
                    new SqlParameter("@BreakEndHour", workday.BreakEndHour),
                    new SqlParameter("@State", workday.State),
                    new SqlParameter("@DoctorId", workday.DoctorId),
                    new SqlParameter("@Id", workday.Id)
                };
                _dbHelper.ExecuteNonQuery(DataProvider.UpdateWorkDayQuery(), parameters);
            }
            return PartialView("Doctor/DoctorDetails", DoctorDetails(service.ActiveDoctors.Id.ToString()));
        }

        [HttpPost]
        public PartialViewResult DeleteDoctorDetail([FromBody] Params param)
        {
            if (!string.IsNullOrEmpty(param.Value))
            {
                var parameter = new SqlParameter("@Id", Convert.ToInt32(param.Value));
                _dbHelper.ExecuteNonQuery(DataProvider.DeleteDoctorQuery(), new[] { parameter });
            }
            var firstDoctor = service.Doctors.FirstOrDefault();
            return PartialView("Doctor/DoctorDetails", DoctorDetails(firstDoctor?.Id.ToString()));
        }

        [HttpPost]
        public IActionResult UpdateCalendarDoctors([FromBody] Doctor doctor)
        {
            if (doctor != null)
            {
                string dialogState = string.Empty;
                if (doctor.Id == 0)
                {
                    dialogState = "new";
                    doctor.Text = "default";
                    doctor.Availability = "available";
                    doctor.Color = "#7575ff";
                    doctor.NewDoctorClass = "new-doctor";
                    UpdateWorkHours(doctor);
                    var parameters = new[]
                    {
                        new SqlParameter("@Name", doctor.Name),
                        new SqlParameter("@Gender", doctor.Gender),
                        new SqlParameter("@Text", doctor.Text),
                        new SqlParameter("@DepartmentId", doctor.DepartmentId),
                        new SqlParameter("@Color", doctor.Color),
                        new SqlParameter("@Education", doctor.Education),
                        new SqlParameter("@Specialization", doctor.Specialization),
                        new SqlParameter("@Experience", doctor.Experience),
                        new SqlParameter("@Designation", doctor.Designation),
                        new SqlParameter("@DutyTiming", doctor.DutyTiming),
                        new SqlParameter("@Email", doctor.Email),
                        new SqlParameter("@Mobile", doctor.Mobile),
                        new SqlParameter("@Availability", doctor.Availability),
                        new SqlParameter("@StartHour", doctor.StartHour),
                        new SqlParameter("@EndHour", doctor.EndHour),
                        new SqlParameter("@NewDoctorClass", doctor.NewDoctorClass)
                    };
                    _dbHelper.ExecuteNonQuery(DataProvider.InsertDoctorQuery(), parameters);
                }
                else
                {
                    UpdateWorkHours(doctor);
                    var parameters = new[]
                    {
                        new SqlParameter("@Name", doctor.Name),
                        new SqlParameter("@Gender", doctor.Gender),
                        new SqlParameter("@Mobile", doctor.Mobile),
                        new SqlParameter("@Email", doctor.Email),
                        new SqlParameter("@Specialization", doctor.Specialization),
                        new SqlParameter("@Experience", doctor.Experience),
                        new SqlParameter("@Education", doctor.Education),
                        new SqlParameter("@Designation", doctor.Designation),
                        new SqlParameter("@DutyTiming", doctor.DutyTiming),
                        new SqlParameter("@Id", doctor.Id)
                    };
                    _dbHelper.ExecuteNonQuery(DataProvider.UpdateDoctorQuery(), parameters);
                    service.ActiveDoctors = doctor;
                }
                Activity activity = new()
                {
                    Name = dialogState == "new" ? "Added New Doctor" : "Updated Doctor",
                    Message = "Dr." + doctor.Name + ", " + char.ToUpperInvariant(doctor.Specialization[0]) + doctor.Specialization.Substring(1),
                    Time = "10 mins ago",
                    Type = "doctor",
                    ActivityTime = DateTime.Now
                };
                var activityParameters = new[]
                {
                    new SqlParameter("@Name", activity.Name),
                    new SqlParameter("@Message", activity.Message),
                    new SqlParameter("@Time", activity.Time),
                    new SqlParameter("@Type", activity.Type),
                    new SqlParameter("@ActivityTime", activity.ActivityTime)
                };
                _dbHelper.ExecuteNonQuery(DataProvider.InsertActivityQuery(), activityParameters);
            }
            if (service.FilteredDoctors != null && service.FilteredDoctors.Count != service.Doctors.Count())
            {
                service.FilteredDoctors = service.Doctors.Where(item => item.DepartmentId.Equals(service.FilteredDoctors.First().DepartmentId)).ToList();
            }
            return Ok(doctor);
        }

        private void UpdateWorkHours(Doctor data)
        {
            string dutyString = service.DutyTimings.Where(item => item.Id.Equals(data.DutyTiming)).FirstOrDefault().Text;
            TimeSpan startValue;
            TimeSpan endValue;
            if (dutyString == "10:00 AM - 7:00 PM")
            {
                startValue = new TimeSpan(10, 0, 0);
                endValue = new TimeSpan(19, 0, 0);
                data.StartHour = "10:00";
                data.EndHour = "19:00";
            }
            else if (dutyString == "08:00 AM - 5:00 PM")
            {
                startValue = new TimeSpan(8, 0, 0);
                endValue = new TimeSpan(17, 0, 0);
                data.StartHour = "08:00";
                data.EndHour = "17:00";
            }
            else
            {
                startValue = new TimeSpan(12, 0, 0);
                endValue = new TimeSpan(21, 0, 0);
                data.StartHour = "12:00";
                data.EndHour = "21:00";
            }
            if (data.WorkDays != null)
            {
                foreach (var x in data.WorkDays)
                {
                    x.WorkStartHour = x.WorkStartHour.HasValue ? x.WorkStartHour.Value.Date.Add(startValue) : x.WorkStartHour;
                    x.WorkEndHour = x.WorkEndHour.HasValue ? x.WorkEndHour.Value.Date.Add(endValue) : x.WorkEndHour;
                }
            }
        }

        [HttpPost]
        public void UpdateWaitingListData([FromBody] string[] activeIds)
        {
            if (activeIds != null && activeIds.Count() > 0)
            {
                foreach (string ID in activeIds)
                {
                    var parameter = new SqlParameter("@Id", Convert.ToInt32(ID));
                    _dbHelper.ExecuteNonQuery(DataProvider.DeleteWaitingListQuery(), new[] { parameter });
                }
            }
        }

        [HttpPost]
        public void UpdateActivityData([FromBody] Activity activityData)
        {
            if (activityData != null)
            {
                activityData.ActivityTime = DateTime.Now;
                var parameters = new[]
                {
                    new SqlParameter("@Name", activityData.Name),
                    new SqlParameter("@Message", activityData.Message),
                    new SqlParameter("@Time", activityData.Time),
                    new SqlParameter("@Type", activityData.Type),
                    new SqlParameter("@ActivityTime", activityData.ActivityTime)
                };
                _dbHelper.ExecuteNonQuery(DataProvider.InsertActivityQuery(), parameters);
            }
        }

        [HttpPost]
        public void UpdateHospitalData([FromBody] EditParams param)
        {
            if (param.added != null && param.added.Count() > 0)
            {
                foreach (Hospital item in param.added)
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@Name", item.Name),
                        new SqlParameter("@StartTime", item.StartTime),
                        new SqlParameter("@EndTime", item.EndTime),
                        new SqlParameter("@Disease", item.Disease),
                        new SqlParameter("@DepartmentName", item.DepartmentName),
                        new SqlParameter("@DepartmentId", item.DepartmentId),
                        new SqlParameter("@DoctorId", item.DoctorId),
                        new SqlParameter("@PatientId", item.PatientId),
                        new SqlParameter("@Symptoms", item.Symptoms),
                        new SqlParameter("@IsBlock", item.IsBlock)
                    };
                    _dbHelper.ExecuteNonQuery(DataProvider.InsertHospitalQuery(), parameters);
                }
            }
            if (param.changed != null && param.changed.Count() > 0)
            {
                foreach (Hospital item in param.changed)
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@Name", item.Name),
                        new SqlParameter("@StartTime", item.StartTime),
                        new SqlParameter("@EndTime", item.EndTime),
                        new SqlParameter("@Disease", item.Disease),
                        new SqlParameter("@DepartmentName", item.DepartmentName),
                        new SqlParameter("@DepartmentId", item.DepartmentId),
                        new SqlParameter("@DoctorId", item.DoctorId),
                        new SqlParameter("@PatientId", item.PatientId),
                        new SqlParameter("@Symptoms", item.Symptoms),
                        new SqlParameter("@IsBlock", item.IsBlock),
                        new SqlParameter("@Id", item.Id)
                    };
                    _dbHelper.ExecuteNonQuery(DataProvider.UpdateHospitalQuery(), parameters);
                }
            }
            if (param.deleted != null && param.deleted.Count() > 0)
            {
                foreach (Hospital item in param.deleted)
                {
                    var parameter = new SqlParameter("@Id", item.Id);
                    _dbHelper.ExecuteNonQuery(DataProvider.DeleteHospitalQuery(), new[] { parameter });
                }
            }
        }

        [HttpPost]
        public void UpdatePatients([FromBody] Params param)
        {
            if (!string.IsNullOrEmpty(param.Value))
            {
                var parameter = new SqlParameter("@Id", Convert.ToInt32(param.Value));
                _dbHelper.ExecuteNonQuery(DataProvider.DeletePatientQuery(), new[] { parameter });
            }
        }

        [HttpPost]
        public IActionResult UpdatePatientData([FromBody] Patient patient)
        {
            if (patient != null)
            {
                string dialogState = string.Empty;
                if (patient.Id == 0)
                {
                    dialogState = "new";
                    var parameters = new[]
                    {
                        new SqlParameter("@Name", patient.Name),
                        new SqlParameter("@Text", patient.Text),
                        new SqlParameter("@DOB", patient.DOB),
                        new SqlParameter("@Mobile", patient.Mobile),
                        new SqlParameter("@Email", patient.Email),
                        new SqlParameter("@Address", patient.Address),
                        new SqlParameter("@Disease", patient.Disease),
                        new SqlParameter("@DepartmentName", patient.DepartmentName),
                        new SqlParameter("@BloodGroup", patient.BloodGroup),
                        new SqlParameter("@Gender", patient.Gender),
                        new SqlParameter("@Symptoms", patient.Symptoms)
                    };
                    _dbHelper.ExecuteNonQuery(DataProvider.InsertPatientQuery(), parameters);
                }
                else
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@Name", patient.Name),
                        new SqlParameter("@Text", patient.Text),
                        new SqlParameter("@DOB", patient.DOB),
                        new SqlParameter("@Mobile", patient.Mobile),
                        new SqlParameter("@Email", patient.Email),
                        new SqlParameter("@Address", patient.Address),
                        new SqlParameter("@Disease", patient.Disease),
                        new SqlParameter("@DepartmentName", patient.DepartmentName),
                        new SqlParameter("@BloodGroup", patient.BloodGroup),
                        new SqlParameter("@Gender", patient.Gender),
                        new SqlParameter("@Symptoms", patient.Symptoms),
                        new SqlParameter("@Id", patient.Id)
                    };
                    _dbHelper.ExecuteNonQuery(DataProvider.UpdatePatientQuery(), parameters);
                    service.ActivePatients = patient;
                }
                Activity activity = new()
                {
                    Name = dialogState == "new" ? "Added New Patient" : "Updated Patient",
                    Message = patient.Name + " for " + patient.Symptoms,
                    Time = "10 mins ago",
                    Type = "patient",
                    ActivityTime = DateTime.Now
                };
                var activityParameters = new[]
                {
                    new SqlParameter("@Name", activity.Name),
                    new SqlParameter("@Message", activity.Message),
                    new SqlParameter("@Time", activity.Time),
                    new SqlParameter("@Type", activity.Type),
                    new SqlParameter("@ActivityTime", activity.ActivityTime)
                };
                _dbHelper.ExecuteNonQuery(DataProvider.InsertActivityQuery(), activityParameters);
            }
            return Ok(patient);
        }

    }
}
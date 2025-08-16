using System.ComponentModel.DataAnnotations;

namespace AppointmentPlanner.Models
{
    public class Hospital
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Disease { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string RecurrenceRule { get; set; }
        public string Symptoms { get; set; }
        public bool? IsAllDay { get; set; }
        public string ElementType { get; set; }
        public bool IsBlock { get; set; }
        public Nullable<int> RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
    }

    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter a valid name.")]
        public string Name { get; set; }
        public string Text { get; set; }
        [Required(ErrorMessage = "Select a valid DOB.")]
        public DateTime? DOB { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Enter a valid mobile number.")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "A valid email address is required.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "A valid email address is required.")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Disease { get; set; }
        public string DepartmentName { get; set; }
        [Required]
        public string BloodGroup { get; set; } = "AB +ve";
        public string Gender { get; set; } = "Male";
        public string Symptoms { get; set; }
    }

    public class Appointment
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public string DoctorName { get; set; }
        public string Symptoms { get; set; }
        public int DoctorId { get; set; }
    }

    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter a valid name.")]
        public string Name { get; set; }
        public string Gender { get; set; } = "Male";
        public string Text { get; set; }
        public int DepartmentId { get; set; } = 1;
        public string Color { get; set; }
        public string Education { get; set; }
        public string Specialization { get; set; } = "generalmedicine";
        public string Experience { get; set; } = "1+ years";
        public string Designation { get; set; }
        public string NewDoctorClass { get; set; } = string.Empty;
        public string DutyTiming { get; set; } = "Shift1";
        [Required(ErrorMessage = "A valid email address is required.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "A valid email address is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter a valid mobile number.")]
        public string Mobile { get; set; }
        public string Availability { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }
    }

    public class WaitingList
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Disease { get; set; }
        public string? DepartmentName { get; set; }
        public string? Treatment { get; set; }
        public int DepartmentId { get; set; }
        public int PatientId { get; set; }
    }

    public class Specialization
    {
        [Key]
        public int DepartmentId { get; set; }
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? Color { get; set; }
    }

    public class TextIdData
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
    }

    public class TextValueData
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
    }

    public class TextValueNumericData
    {
        public int Value { get; set; }
        public string? Text { get; set; }
    }

    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? Time { get; set; }
        public string? Type { get; set; }
        public DateTime ActivityTime { get; set; }
    }

    public class Block
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? RecurrenceRule { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsBlock { get; set; }
        public int[] DoctorId { get; set; }
    }

    public class ChartData
    {
        public DateTime Date { get; set; }
        public int? EventCount { get; set; }
    }

    public class CalendarSetting
    {
        public string? BookingColor { get; set; }
        public Calendar? Calendar { get; set; }
        public string? CurrentView { get; set; }
        public int Interval { get; set; } = 60;
        public int FirstDayOfWeek { get; set; } = 1;
    }

    public class Calendar
    {
        public string? Start { get; set; }
        public string? End { get; set; }
        public bool? Highlight { get; set; }
    }
    public class TemplateArgs
    {
        public string? ElementType { get; set; }
        public DateTime? Date { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Disease { get; set; }
        public string? DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string? Symptoms { get; set; }
    }

    public class TimeSheetGroupData
    {
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
    }

    public class Params
    {
        public string Prop { get; set; }
        public string Value { get; set; }
    }

    public class EditParams
    {
        public List<Hospital> added { get; set; }
        public List<Hospital> changed { get; set; }
        public List<Hospital> deleted { get; set; }
    }

    public static class DataProvider
    {
        public static List<TextIdData> ExperienceData()
        {
            List<TextIdData> data = new List<TextIdData>
            {
                new TextIdData { Id = "1+ years", Text = "1+ years" },
                new TextIdData { Id = "2+ years", Text = "2+ years" },
                new TextIdData { Id = "5+ years", Text = "5+ years" },
                new TextIdData { Id = "10+ years", Text = "10+ years" },
                new TextIdData { Id = "15+ years", Text = "15+ years" },
                new TextIdData { Id = "20+ years", Text = "20+ years" }
            };
            return data;
        }
        public static List<TextIdData> DutyTimingsData()
        {
            List<TextIdData> data = new List<TextIdData>
            {
                new TextIdData { Id = "Shift1", Text = "08:00 AM - 5:00 PM" },
                new TextIdData { Id = "Shift2", Text = "10:00 AM - 7:00 PM" },
                new TextIdData { Id = "Shift3", Text = "12:00 PM - 9:00 PM" }
            };
            return data;
        }

        public static List<TextValueData> GetStartHours()
        {
            List<TextValueData> data = new List<TextValueData>
            {
                new TextValueData { Value = "08:00", Text = "8.00 AM" },
                new TextValueData { Value = "9:00", Text = "9.00 AM" },
                new TextValueData { Value = "10:00", Text = "10.00 AM" },
                new TextValueData { Value = "11:00", Text = "11.00 AM" },
                new TextValueData { Value = "12:00", Text = "12.00 AM" }
            };
            return data;
        }
        public static List<TextValueData> GetEndHours()
        {
            List<TextValueData> data = new List<TextValueData>
            {
                new TextValueData { Value = "16:00", Text = "4.00 PM" },
                new TextValueData { Value = "17:00", Text = "5.00 PM" },
                new TextValueData { Value = "18:00", Text = "6.00 PM" },
                new TextValueData { Value = "19:00", Text = "7.00 PM" },
                new TextValueData { Value = "20:00", Text = "8.00 PM" },
                new TextValueData { Value = "21:00", Text = "9.00 PM" }
            };
            return data;
        }
        public static List<TextValueData> GetViews()
        {
            List<TextValueData> data = new List<TextValueData>
            {
                new TextValueData { Value = "Day", Text = "Daily" },
                new TextValueData { Value = "Week", Text = "Weekly" },
                new TextValueData { Value = "Month", Text = "Monthly" }
            };
            return data;
        }
        public static List<TextValueData> GetColorCategory()
        {
            List<TextValueData> data = new List<TextValueData>
            {
                new TextValueData { Value = "Departments", Text = "Department Colors" },
                new TextValueData { Value = "Doctors", Text = "Doctors Colors" }
            };
            return data;
        }
        public static List<TextValueData> GetBloodGroupData()
        {
            List<TextValueData> data = new List<TextValueData>
            {
                new TextValueData { Value = "AB +ve", Text = "AB +ve" },
                new TextValueData { Value = "A +ve", Text = "A +ve" },
                new TextValueData { Value = "B +ve", Text = "B +ve" },
                new TextValueData { Value = "O +ve", Text = "O +ve" },
                new TextValueData { Value = "AB -ve", Text = "AB -ve" },
                new TextValueData { Value = "A -ve", Text = "A -ve" },
                new TextValueData { Value = "B -ve", Text = "B -ve" },
                new TextValueData { Value = "O -ve", Text = "O -ve" }
            };
            return data;
        }
        public static List<TextValueNumericData> GetTimeSlot()
        {
            List<TextValueNumericData> data = new List<TextValueNumericData>
            {
                new TextValueNumericData { Value = 10, Text = "10 mins" },
                new TextValueNumericData { Value = 20, Text = "20 mins" },
                new TextValueNumericData { Value = 30, Text = "30 mins" },
                new TextValueNumericData { Value = 60, Text = "60 mins" },
                new TextValueNumericData { Value = 120, Text = "120 mins" }
            };
            return data;
        }

        public static List<TextValueNumericData> GetDayOfWeekList()
        {
            List<TextValueNumericData> data = new List<TextValueNumericData>
            {
                new TextValueNumericData { Value = 0, Text = "Sunday" },
                new TextValueNumericData { Value = 1, Text = "Monday" },
                new TextValueNumericData { Value = 2, Text = "Tuesday" },
                new TextValueNumericData { Value = 3, Text = "Wednesday" },
                new TextValueNumericData { Value = 4, Text = "Thursday" },
                new TextValueNumericData { Value = 5, Text = "Friday" },
                new TextValueNumericData { Value = 6, Text = "Saturday" }
            };
            return data;
        }
    }
}
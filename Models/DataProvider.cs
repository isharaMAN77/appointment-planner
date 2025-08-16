namespace AppointmentPlanner.Models
{
    public static class DataProvider
    {
        public static string GetHospitalsQuery()
        {
            return "SELECT * FROM Hospitals";
        }

        public static string GetPatientsQuery()
        {
            return "SELECT * FROM Patients";
        }

        public static string GetDoctorsQuery()
        {
            return "SELECT * FROM Doctors";
        }

        public static string GetWaitingListsQuery()
        {
            return "SELECT * FROM WaitingLists";
        }

        public static string GetSpecializationsQuery()
        {
            return "SELECT * FROM Specializations";
        }

        public static string GetActivitiesQuery()
        {
            return "SELECT * FROM Activities";
        }

        public static string GetDoctorByIdQuery()
        {
            return "SELECT * FROM Doctors WHERE Id = @Id";
        }

        public static string GetWorkDaysByDoctorIdQuery()
        {
            return "SELECT * FROM WorkDays WHERE DoctorId = @DoctorId";
        }

        public static string GetSpecializationByIdQuery()
        {
            return "SELECT * FROM Specializations WHERE Id = @Id";
        }

        public static string GetFilteredHospitalsQuery()
        {
            return "SELECT * FROM Hospitals WHERE StartTime >= @StartTime AND EndTime <= @EndTime";
        }

        public static string GetFilteredDoctorsQuery()
        {
            return "SELECT * FROM Doctors WHERE DepartmentId = @DepartmentId";
        }

        public static string DeleteDoctorQuery()
        {
            return "DELETE FROM Doctors WHERE Id = @Id";
        }

        public static string InsertDoctorQuery()
        {
            return "INSERT INTO Doctors (Name, Gender, Text, DepartmentId, Color, Education, Specialization, Experience, Designation, DutyTiming, Email, Mobile, Availability, StartHour, EndHour, NewDoctorClass) VALUES (@Name, @Gender, @Text, @DepartmentId, @Color, @Education, @Specialization, @Experience, @Designation, @DutyTiming, @Email, @Mobile, @Availability, @StartHour, @EndHour, @NewDoctorClass)";
        }

        public static string UpdateDoctorQuery()
        {
            return "UPDATE Doctors SET Name = @Name, Gender = @Gender, Mobile = @Mobile, Email = @Email, Specialization = @Specialization, Experience = @Experience, Education = @Education, Designation = @Designation, DutyTiming = @DutyTiming WHERE Id = @Id";
        }

        public static string InsertActivityQuery()
        {
            return "INSERT INTO Activities (Name, Message, Time, Type, ActivityTime) VALUES (@Name, @Message, @Time, @Type, @ActivityTime)";
        }

        public static string DeleteWaitingListQuery()
        {
            return "DELETE FROM WaitingLists WHERE Id = @Id";
        }

        public static string InsertHospitalQuery()
        {
            return "INSERT INTO Hospitals (Name, StartTime, EndTime, Disease, DepartmentName, DepartmentId, DoctorId, PatientId, Symptoms, IsBlock) VALUES (@Name, @StartTime, @EndTime, @Disease, @DepartmentName, @DepartmentId, @DoctorId, @PatientId, @Symptoms, @IsBlock)";
        }

        public static string UpdateHospitalQuery()
        {
            return "UPDATE Hospitals SET Name = @Name, StartTime = @StartTime, EndTime = @EndTime, Disease = @Disease, DepartmentName = @DepartmentName, DepartmentId = @DepartmentId, DoctorId = @DoctorId, PatientId = @PatientId, Symptoms = @Symptoms, IsBlock = @IsBlock WHERE Id = @Id";
        }

        public static string DeleteHospitalQuery()
        {
            return "DELETE FROM Hospitals WHERE Id = @Id";
        }

        public static string DeletePatientQuery()
        {
            return "DELETE FROM Patients WHERE Id = @Id";
        }

        public static string InsertPatientQuery()
        {
            return "INSERT INTO Patients (Name, Text, DOB, Mobile, Email, Address, Disease, DepartmentName, BloodGroup, Gender, Symptoms) VALUES (@Name, @Text, @DOB, @Mobile, @Email, @Address, @Disease, @DepartmentName, @BloodGroup, @Gender, @Symptoms)";
        }

        public static string UpdatePatientQuery()
        {
            return "UPDATE Patients SET Name = @Name, Text = @Text, DOB = @DOB, Mobile = @Mobile, Email = @Email, Address = @Address, Disease = @Disease, DepartmentName = @DepartmentName, BloodGroup = @BloodGroup, Gender = @Gender, Symptoms = @Symptoms WHERE Id = @Id";
        }

        public static string UpdateWorkDayQuery()
        {
            return "UPDATE WorkDays SET Day = @Day, Index = @Index, Enable = @Enable, WorkStartHour = @WorkStartHour, WorkEndHour = @WorkEndHour, BreakStartHour = @BreakStartHour, BreakEndHour = @BreakEndHour, State = @State, DoctorId = @DoctorId WHERE Id = @Id";
        }
    }
}

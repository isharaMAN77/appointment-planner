-- Create the database
CREATE DATABASE AppointmentPlanner;
GO

USE AppointmentPlanner;
GO

-- Create the Doctors table
CREATE TABLE Doctors (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(MAX) NOT NULL,
    Gender NVARCHAR(MAX) NOT NULL,
    Text NVARCHAR(MAX),
    DepartmentId INT NOT NULL,
    Color NVARCHAR(MAX),
    Education NVARCHAR(MAX),
    Specialization NVARCHAR(MAX) NOT NULL,
    Experience NVARCHAR(MAX) NOT NULL,
    Designation NVARCHAR(MAX),
    NewDoctorClass NVARCHAR(MAX) NOT NULL,
    DutyTiming NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Mobile NVARCHAR(MAX) NOT NULL,
    Availability NVARCHAR(MAX),
    StartHour NVARCHAR(MAX),
    EndHour NVARCHAR(MAX)
);
GO

-- Create the Patients table
CREATE TABLE Patients (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(MAX) NOT NULL,
    Text NVARCHAR(MAX),
    DOB DATETIME2,
    Mobile NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Address NVARCHAR(MAX),
    Disease NVARCHAR(MAX),
    DepartmentName NVARCHAR(MAX),
    BloodGroup NVARCHAR(MAX) NOT NULL,
    Gender NVARCHAR(MAX) NOT NULL,
    Symptoms NVARCHAR(MAX)
);
GO

-- Create the WorkDays table
CREATE TABLE WorkDays (
    Id INT PRIMARY KEY IDENTITY,
    [Day] NVARCHAR(MAX),
    [Index] INT NOT NULL,
    Enable BIT NOT NULL,
    WorkStartHour DATETIME2,
    WorkEndHour DATETIME2,
    BreakStartHour DATETIME2,
    BreakEndHour DATETIME2,
    State NVARCHAR(MAX),
    DoctorId INT NOT NULL,
    CONSTRAINT FK_WorkDays_Doctors FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
);
GO

-- Create the Hospitals table (Appointments)
CREATE TABLE Hospitals (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(MAX),
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    Disease NVARCHAR(MAX),
    DepartmentName NVARCHAR(MAX),
    DepartmentId INT NOT NULL,
    DoctorId INT NOT NULL,
    PatientId INT NOT NULL,
    RecurrenceRule NVARCHAR(MAX),
    Symptoms NVARCHAR(MAX),
    IsAllDay BIT,
    ElementType NVARCHAR(MAX),
    IsBlock BIT NOT NULL,
    RecurrenceID INT,
    RecurrenceException NVARCHAR(MAX),
    CONSTRAINT FK_Hospitals_Doctors FOREIGN KEY (DoctorId) REFERENCES Doctors(Id),
    CONSTRAINT FK_Hospitals_Patients FOREIGN KEY (PatientId) REFERENCES Patients(Id)
);
GO

-- Create the WaitingLists table
CREATE TABLE WaitingLists (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(MAX),
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    Disease NVARCHAR(MAX),
    DepartmentName NVARCHAR(MAX),
    Treatment NVARCHAR(MAX),
    DepartmentId INT NOT NULL,
    PatientId INT NOT NULL,
    CONSTRAINT FK_WaitingLists_Patients FOREIGN KEY (PatientId) REFERENCES Patients(Id)
);
GO

-- Create the Specializations table
CREATE TABLE Specializations (
    DepartmentId INT PRIMARY KEY,
    Id NVARCHAR(450),
    Text NVARCHAR(MAX),
    Color NVARCHAR(MAX)
);
GO

-- Create the Activities table
CREATE TABLE Activities (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(MAX),
    Message NVARCHAR(MAX),
    Time NVARCHAR(MAX),
    Type NVARCHAR(MAX),
    ActivityTime DATETIME2 NOT NULL
);
GO

-- Insert initial data
INSERT INTO Specializations (DepartmentId, Id, Text, Color) VALUES
(1, 'generalmedicine', 'General Medicine', '#F538B2'),
(2, 'neurology', 'Neurology', '#33C7E8'),
(3, 'dermatology', 'Dermatology', '#916DE4'),
(4, 'orthopedics', 'Orthopedics', '#388CF5'),
(5, 'diabetology', 'Diabetology', '#60F238'),
(6, 'cardiology', 'Cardiology', '#F29438');
GO

SET IDENTITY_INSERT Doctors ON;
INSERT INTO Doctors (Id, Name, Gender, Text, DepartmentId, Color, Education, Specialization, Experience, Designation, NewDoctorClass, DutyTiming, Email, Mobile, Availability, StartHour, EndHour) VALUES
(1, 'Nembo Lukeni', 'Male', 'NemboLukni', 1, '#ea7a57', 'MBBS, DMRD', 'generalmedicine', '10+ years', 'Senior Specialist', '', 'Shift1', 'nembo36@sample.com', '(206) 555-9482', 'busy', '08:00', '17:00'),
(2, 'Mollie Cobb', 'Female', 'MollieCobb', 2, '#7fa900', 'MBBS, MD PAEDIATRICS, DM NEUROLOGY', 'neurology', '2+ years', 'Junior Specialist', '', 'Shift2', 'mollie65@rpy.com', '(206) 555-3412', 'available', '10:00', '19:00'),
(3, 'Yara Barros', 'Female', 'YaraBarros', 1, '#fec200', 'MBBS, DNB (FAMILY MEDICINE)', 'generalmedicine', '5+ years', 'Senior Specialist', '', 'Shift3', 'yara105@sample.com', '(206) 555-8122', 'away', '12:00', '21:00'),
(4, 'Paul Walker', 'Male', 'PaulWalker', 3, '#865fcf', 'MBBS, MD (Dermatology)', 'dermatology', '10+ years', 'Senior Specialist', '', 'Shift1', 'paul39@mail.com', '(071) 555-4848', 'busy', '08:00', '17:00'),
(5, 'Amelia Edwards', 'Female', 'AmeliaEdwards', 4, '#1aaa55', 'MBBS, MD', 'orthopedics', '10+ years', 'Junior Specialist', '', 'Shift2', 'amelia101@rpy.com', '(071) 555-7773', 'available', '10:00', '19:00'),
(6, 'Alexa Richardson', 'Female', 'AlexaRichardson', 5, '#1aaa55', 'MBBS, MD', 'diabetology', '1+ years', 'Practitioner', '', 'Shift2', 'alexa55@sample.com', '(071) 555-5598', 'busy', '10:00', '19:00'),
(7, 'Amelia Nout Golstein', 'Male', 'NoutGolstein', 6, '#00bdae', 'MS', 'cardiology', '2+ years', 'Junior Specialist', '', 'Shift3', 'nout49@rpy.com', '(206) 555-1189', 'busy', '12:00', '21:00');
SET IDENTITY_INSERT Doctors OFF;
GO

SET IDENTITY_INSERT Patients ON;
INSERT INTO Patients (Id, Name, Text, DOB, Mobile, Email, Address, Disease, DepartmentName, BloodGroup, Gender, Symptoms) VALUES
(1, 'Laura', 'Laura', '1980-08-03', '(071) 555-4444', 'laura90@mail.com', '507 - 20th Ave. E. Apt. 2A', 'Eye Checkup', 'GENERAL', 'O +ve', 'Female', 'Sweating, Chills and Shivering'),
(2, 'Milka', 'Milka', '2000-03-05', '(071) 555-4445', 'milka40@sample.com', '908 W. Capital Way', 'Bone Fracture', 'ORTHOPEDICS', 'AB +ve', 'Female', 'Swelling or bruising over a bone, Pain in the injured area'),
(3, 'Adams', 'Adams', '1985-02-03', '(071) 555-4454', 'adams89@rpy.com', '722 Moss Bay Blvd.', 'Eye and Spectactles', 'GENERAL', 'B +ve', 'Male', 'Frequent squinting, Eye fatigue or strain'),
(4, 'Janet', 'Janet', '2000-07-03', '(071) 555-4544', 'janet79@rpy.com', '4110 Old Redmond Rd.', 'Biological Problem', 'GENERAL', 'B +ve', 'Male', 'Physical aches or pain, Memory difficulties or personality change'),
(5, 'Mercy', 'Mercy', '2005-04-29', '(071) 555-5444', 'mercy60@sample.com', '14 Garrett Hill', 'Skin Hives', 'DERMATOLOGY', 'AB -ve', 'Female', 'outbreak of swollen, pale red bumps or plaques'),
(6, 'Richa', 'Richa', '1989-10-29', '(206) 555-4444', 'richa46@mail.com', 'Coventry House Miner Rd.', 'Arm Fracture', 'ORTHOPEDICS', 'B +ve', 'Female', 'Swelling, warmth, or redness in the joint'),
(7, 'Maud Oliver', 'Maud Oliver', '1989-10-29', '(206) 666-4444', 'moud46@rpy.com', 'Coventry House Miner Rd.', 'Racing heartbeat', 'CARDIOLOGY', 'B +ve', 'Male', 'A fluttering in your chest');
SET IDENTITY_INSERT Patients OFF;
GO

SET IDENTITY_INSERT Hospitals ON;
INSERT INTO Hospitals (Id, Name, StartTime, EndTime, Disease, DepartmentName, DepartmentId, DoctorId, PatientId, Symptoms, IsBlock) VALUES
(1000, 'Milka', '2020-02-05 10:30:00', '2020-02-05 11:30:00', 'Bone Fracture', 'ORTHOPEDICS', 4, 5, 2, 'Swelling or bruising over a bone, Pain in the injured area', 0),
(1001, 'Janet', '2020-02-03 11:00:00', '2020-02-03 12:00:00', 'Biological Problems', 'GENERAL', 1, 3, 4, 'Physical aches or pain, Memory difficulties or personality changes', 0),
(1002, 'Mercy', '2020-02-02 10:00:00', '2020-02-02 11:00:00', 'Skin Problem', 'DERMATOLOGY', 3, 4, 5, 'outbreak of swollen, pale red bumps or plaques', 0);
SET IDENTITY_INSERT Hospitals OFF;
GO

SET IDENTITY_INSERT WaitingLists ON;
INSERT INTO WaitingLists (Id, Name, StartTime, EndTime, Disease, DepartmentName, Treatment, DepartmentId, PatientId) VALUES
(1, 'Laura', '2020-02-03 08:30:00', '2020-02-03 09:30:00', 'Sudden loss of vision', 'GENERAL', 'CHECKUP', 1, 1),
(2, 'Milka', '2020-02-4 08:30:00', '2020-02-04 10:30:00', 'Bone Fracture', 'ORTHOPEDICS', 'SURGERY', 4, 2),
(3, 'Adams', '2020-02-04 09:30:00', '2020-02-04 10:30:00', 'Skin Hives', 'DERMATOLOGY', 'CHECKUP', 3, 3);
SET IDENTITY_INSERT WaitingLists OFF;
GO

SET IDENTITY_INSERT Activities ON;
INSERT INTO Activities (Id, Name, Message, Time, Type, ActivityTime) VALUES
(1, 'Added New Doctor', 'Dr.Johnson James, Cardiologist', '5 mins ago', 'doctor', '2020-02-01 09:00:00'),
(2, 'Added New Appointment', 'Laura for General Checkup on 7th March, 2020 @ 8.30 AM with Dr.Molli Cobb, Cardiologist', '5 mins ago', 'appointment', '2020-02-01 11:00:00'),
(3, 'Added New Patient', 'James Richard for Fever and cold', '5 mins ago', 'patient', '2020-02-01 10:00:00');
SET IDENTITY_INSERT Activities OFF;
GO

SET IDENTITY_INSERT WorkDays ON;
INSERT INTO WorkDays (Id, [Day], [Index], Enable, WorkStartHour, WorkEndHour, BreakStartHour, BreakEndHour, State, DoctorId) VALUES
(1, 'Sunday', 0, 1, '2020-02-01 08:00:00', '2020-02-01 17:00:00', '2020-02-01 12:00:00', '2020-02-01 13:00:00', 'AddBreak', 1),
(2, 'Monday', 1, 0, '2020-02-02 08:00:00', '2020-02-02 17:00:00', '2020-02-02 12:00:00', '2020-02-02 13:00:00', 'TimeOff', 1),
(3, 'Tuesday', 2, 1, '2020-02-03 10:00:00', '2020-02-03 19:00:00', '2020-02-03 14:00:00', '2020-02-03 15:00:00', 'AddBreak', 2),
(4, 'Wednesday', 3, 1, '2020-02-04 10:00:00', '2020-02-04 19:00:00', '2020-02-04 14:00:00', '2020-02-04 15:00:00', 'AddBreak', 2);
SET IDENTITY_INSERT WorkDays OFF;
GO

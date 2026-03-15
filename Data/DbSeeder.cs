using System;
using System.Linq;
using System.Threading.Tasks;
using HealthcareSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthcareSystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(HealthcareDbContext context, IPasswordHasher<User> hasher)
        {
            // Ensure Database is created
            await context.Database.EnsureCreatedAsync();

            if (!context.Users.Any())
            {
                // Admin
                var adminUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "admin@healthcare.com",
                    Role = "Admin",
                    CreatedAt = DateTime.Now
                };
                adminUser.Password = hasher.HashPassword(adminUser, "Admin@123");
                context.Users.Add(adminUser);

                // Doctors
                var doctor1User = new User { Id = Guid.NewGuid().ToString(), Email = "doctor1@healthcare.com", Role = "Doctor", CreatedAt = DateTime.Now };
                doctor1User.Password = hasher.HashPassword(doctor1User, "Doctor@123");
                var doctor2User = new User { Id = Guid.NewGuid().ToString(), Email = "doctor2@healthcare.com", Role = "Doctor", CreatedAt = DateTime.Now };
                doctor2User.Password = hasher.HashPassword(doctor2User, "Doctor@123");
                context.Users.AddRange(doctor1User, doctor2User);

                var doctor1 = new Doctor
                {
                    Id = Guid.NewGuid().ToString(), UserId = doctor1User.Id, FirstName = "Doctor", LastName = " 1", Specialization = "Cardiology", Phone = "+123456789", Email = doctor1User.Email, ConsultationFee = 150.0, CreatedAt = DateTime.Now
                };
                var doctor2 = new Doctor
                {
                    Id = Guid.NewGuid().ToString(), UserId = doctor2User.Id, FirstName = "Doctor", LastName = " 2", Specialization = "Neurology", Phone = "+123456789", Email = doctor2User.Email, ConsultationFee = 200.0, CreatedAt = DateTime.Now
                };
                context.Doctors.AddRange(doctor1, doctor2);

                // Patients
                var patient1User = new User { Id = Guid.NewGuid().ToString(), Email = "patient1@healthcare.com", Role = "Patient", CreatedAt = DateTime.Now };
                patient1User.Password = hasher.HashPassword(patient1User, "Patient@123");
                var patient2User = new User { Id = Guid.NewGuid().ToString(), Email = "patient2@healthcare.com", Role = "Patient", CreatedAt = DateTime.Now };
                patient2User.Password = hasher.HashPassword(patient2User, "Patient@123");
                context.Users.AddRange(patient1User, patient2User);

                var patient1 = new Patient
                {
                    Id = Guid.NewGuid().ToString(), UserId = patient1User.Id, FirstName = "Patient", LastName = " 1", DateOfBirth = new DateTime(1990, 1, 1), Gender = "Male", Phone = "111-222-333", BloodGroup = "O+", CreatedAt = DateTime.Now
                };
                var patient2 = new Patient
                {
                    Id = Guid.NewGuid().ToString(), UserId = patient2User.Id, FirstName = "Patient ", LastName = "2", DateOfBirth = new DateTime(1985, 5, 5), Gender = "Female", Phone = "111-222-333", BloodGroup = "A-", CreatedAt = DateTime.Now
                };
                context.Patients.AddRange(patient1, patient2);

                // Appointments
                var apt1 = new Appointment
                {
                    Id = Guid.NewGuid().ToString(), PatientId = patient1.Id, DoctorId = doctor1.Id, AppointmentDate = DateTime.Now.AddDays(1), Status = "Scheduled", Reason = "Routine Checkup", CreatedAt = DateTime.Now
                };
                var apt2 = new Appointment
                {
                    Id = Guid.NewGuid().ToString(), PatientId = patient2.Id, DoctorId = doctor2.Id, AppointmentDate = DateTime.Now.AddDays(-2), Status = "Completed", Reason = "Headache", CreatedAt = DateTime.Now
                };
                context.Appointments.AddRange(apt1, apt2);

                await context.SaveChangesAsync();
            }
        }
    }
}

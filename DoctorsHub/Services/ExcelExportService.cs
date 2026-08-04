using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using ClosedXML.Excel;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;
using DoctorsHub.Application.DTOs.Reports.PatientsReport;



namespace DoctorsHub.Web.Services
{
    public class ExcelExportService
    {
        public MemoryStream ExportAppointmentExcelFile(List<AppointmentReportDto> appointmentReport) 
        {
            using var workBook = new XLWorkbook();

            var workSheet = workBook.Worksheets.Add("Appointments Report");

            //Adding Column Headers
            workSheet.Cell(1, 1).Value = "Appointment Id";
            workSheet.Cell(1, 2).Value = "Appointment Date";
            workSheet.Cell(1, 3).Value = "Patient Name";
            workSheet.Cell(1, 4).Value = "Doctor Name";
            workSheet.Cell(1, 5).Value = "Status";

            //Adding Data

            int row = 2;
            foreach (var appointment in appointmentReport)
            {
                workSheet.Cell(row, 1).Value = appointment.Id;
                workSheet.Cell(row, 2).Value = appointment.AppointmentDate.ToString("yyyy-MM-dd");
                workSheet.Cell(row, 3).Value = appointment.PatientName;
                workSheet.Cell(row, 4).Value = appointment.DoctorName;
                workSheet.Cell(row, 5).Value = appointment.Status.ToString();


                row++;
            }

            //Auto Fit Columns
            workSheet.Columns().AdjustToContents();

            //Create Stream 
            var stream = new MemoryStream();

            workBook.SaveAs(stream);

            stream.Position = 0;

            return stream;

        }

        public MemoryStream ExportBillingExcelFile(List<BillingReportDto> billingReport) 
        {
            //Creating a new workbook
            using var workbook = new XLWorkbook();

            //Adding a new worksheet
            var worksheet = workbook.Worksheets.Add("Billing Report");

            //Adding Column Headers
            worksheet.Cell(1, 1).Value = "BillId";
            worksheet.Cell(1, 2).Value = "BillDate";
            worksheet.Cell(1, 3).Value = "AppointmentDate";
            worksheet.Cell(1, 4).Value = "DoctorName";
            worksheet.Cell(1, 5).Value = "PatientName";
            worksheet.Cell(1, 6).Value = "ConsultationFee";
            worksheet.Cell(1, 7).Value = "AdditionalCharges";
            worksheet.Cell(1, 8).Value = "Discount";
            worksheet.Cell(1, 9).Value = "TotalAmount";
            worksheet.Cell(1, 10).Value = "PaymentStatus";

            //Adding coulmn data

            int row = 2;
            foreach (var bill in billingReport) 
            {
                worksheet.Cell(row, 1).Value = bill.BillId;
                worksheet.Cell(row, 2).Value = bill.BillDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 3).Value = bill.AppointmentDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 4).Value = bill.DoctorName;
                worksheet.Cell(row, 5).Value = bill.PatientName;
                worksheet.Cell(row, 6).Value = bill.ConsultationFee;
                worksheet.Cell(row, 7).Value = bill.AdditionalCharges;
                worksheet.Cell(row, 8).Value = bill.Discount;
                worksheet.Cell(row, 9).Value = bill.TotalAmount;
                worksheet.Cell(row, 10).Value = bill.PaymentStatus.ToString();

                row++;
            }

            //Auto Fit Columns
            worksheet.Columns().AdjustToContents();

            //Create Stream 
            var stream = new MemoryStream();

            workbook.SaveAs(stream);

            stream.Position = 0;

            return stream;
        }

        public MemoryStream ExportDoctorsExcelfile(List<DoctorsReportDto> doctorsReport) 
        {
            //Creating a new workbook

            using var workbook = new XLWorkbook();

            //Adding a new worksheet

            var worksheet = workbook.Worksheets.Add("Doctors Report");

            //Adding Column Headers
            worksheet.Cell(1,1).Value = "Doctor Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Phone Number";
            worksheet.Cell(1, 4).Value = "Qualification";
            worksheet.Cell(1, 5).Value = "Specialization";
            worksheet.Cell(1, 6).Value = "Experience";
            worksheet.Cell(1, 7).Value = "Consultation Fee";

            //Adding column data
            int row = 2;

            foreach(var doctor in doctorsReport)
            {
                worksheet.Cell(row, 1).Value = doctor.DoctorName;
                worksheet.Cell(row, 2).Value = doctor.Email;
                worksheet.Cell(row, 3).Value = doctor.PhoneNumber;
                worksheet.Cell(row, 4).Value = doctor.Qualification;
                worksheet.Cell(row, 5).Value = doctor.Specialization;
                worksheet.Cell(row, 6).Value = doctor.Experience;
                worksheet.Cell(row, 7).Value = doctor.ConsultationFee;

                row++;
            }

            //Auto Fit Columns
            worksheet.Columns().AdjustToContents();

            //Create Stream 
            var stream = new MemoryStream();

            workbook.SaveAs(stream);

            stream.Position = 0;

            return stream;
        }

        public MemoryStream ExportPatientsExcelfile(List<PatientsReportDto> patientsReport) 
        {
            //Creating a new workbook
            using var workbook = new XLWorkbook();
            //Adding a new worksheet
            var worksheet = workbook.Worksheets.Add("Patients Report");
            //Adding Column Headers
            worksheet.Cell(1, 1).Value = "PatientId";
            worksheet.Cell(1, 2).Value = "Patient Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Date of Birth";
            worksheet.Cell(1, 5).Value = "Age";
            worksheet.Cell(1, 6).Value = "Gender";
            worksheet.Cell(1, 7).Value = "Blood Group";
            //Adding column data
            int row = 2;
            foreach (var patient in patientsReport)
            {
                worksheet.Cell(row, 1).Value = patient.Id;
                worksheet.Cell(row, 2).Value = patient.PatientName;
                worksheet.Cell(row, 3).Value = patient.Email;
                worksheet.Cell(row, 4).Value = patient.DateOfBirth.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 5).Value = patient.Age;
                worksheet.Cell(row, 6).Value = patient.Gender;
                worksheet.Cell(row, 7).Value = patient.BloodGroup;
                row++;
            }
            //Auto Fit Columns
            worksheet.Columns().AdjustToContents();
            //Create Stream
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}

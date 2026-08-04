using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using ClosedXML.Excel;



namespace DoctorsHub.Web.Services
{
    public class ExcelExportService
    {
        public MemoryStream ExportAppointmentExcelFile(List<AppointmentReportDto> appointmentsReport) 
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
            foreach (var appointment in appointmentsReport)
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


    }
}

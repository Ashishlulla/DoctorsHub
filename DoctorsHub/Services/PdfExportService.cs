using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;
using DoctorsHub.Application.DTOs.Reports.PatientsReport;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DoctorsHub.Web.Services
{
    public class PdfExportService
    {
        private readonly IWebHostEnvironment _environment;

        public PdfExportService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        #region Private Methods

        private static IContainer HeaderCell(IContainer container)
        {
            return container
                .Background(Colors.Teal.Darken2)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(1)
                .AlignCenter()
                .AlignMiddle();
        }

        private static IContainer BodyCell(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .AlignCenter()
                .Padding(1);
        }

        private byte[]? GetLogo()
        {
            var logoPath = Path.Combine(
                _environment.WebRootPath,
                "images",
                "DoctorsHubLogo.png");

            if (File.Exists(logoPath))
                return File.ReadAllBytes(logoPath);

            return null;
        }

        private void BuildHeader(PageDescriptor page, string reportTitle)
        {
            var logoBytes = GetLogo();

            page.Header().Column(column =>
            {
                if (logoBytes != null)
                {
                    column.Item()
                        .AlignCenter()
                        .Height(70)
                        .Image(logoBytes);
                }

                column.Item()
                    .PaddingTop(4)
                    .AlignCenter()
                    .Text(reportTitle)
                    .Bold()
                    .FontSize(12)
                    .FontColor(Colors.Teal.Darken2);

                column.Item()
                    .AlignCenter()
                    .Text($"Generated On : {DateTime.Now:dd MMM yyyy hh:mm tt}")
                    .FontSize(9);

                column.Item()
                    .PaddingTop(10)
                    .LineHorizontal(1);
            });
        }

        private void BuildFooter(PageDescriptor page)
        {
            page.Footer()
                .PaddingTop(10)
                .AlignCenter()
                .Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
        }

        #endregion
        public byte[] ExportAppointmentsPdf(List<AppointmentReportDto> appointments)
        {

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    var reportName = $"AppointmentsReport-{DateTime.UtcNow.ToString("yyyy-MM-dd")}";
                    //Header
                    BuildHeader(page, reportName);

                    
                    // CONTENT
                    page.Content().PaddingTop(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("ID").Bold().FontColor(Colors.White);
                            header.Cell().Element(HeaderCell).Text("Date").Bold().FontColor(Colors.White);
                            header.Cell().Element(HeaderCell).Text("Patient").Bold().FontColor(Colors.White);
                            header.Cell().Element(HeaderCell).Text("Doctor").Bold().FontColor(Colors.White);
                            header.Cell().Element(HeaderCell).Text("Status").Bold().FontColor(Colors.White);
                        });

                        // Data
                        foreach (var item in appointments)
                        {
                            table.Cell().Element(BodyCell).Text(item.Id.ToString());
                            table.Cell().Element(BodyCell).Text(item.AppointmentDate.ToString("dd MMM yyyy"));
                            table.Cell().Element(BodyCell).Text(item.PatientName);
                            table.Cell().Element(BodyCell).Text(item.DoctorName);
                            table.Cell().Element(BodyCell).Text(item.Status.ToString());
                        }
                    });

                    // FOOTER


                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

        public byte[] ExportBillingPdf(List<BillingReportDto> bills)
        {

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    var reportName = $"BillingReport-{DateTime.UtcNow.ToString("yyyy-MM-dd")}";
                    //Header
                    BuildHeader(page, reportName);


                    // CONTENT
                    page.Content().PaddingTop(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                columns.RelativeColumn();
                            }
                        });

                        // Header
                        table.Header(header =>
                        {
                            
                            header.Cell().Element(HeaderCell).Text("BiilId").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("ApptDate").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("BillDate").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Doctor").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Patient").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Fee").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Add.Charges").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Dist.").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Total").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Status").Bold().FontColor(Colors.White);
                        });

                        // Data
                        foreach (var item in bills)
                        {
                            table.Cell().Element(BodyCell).Text(item.BillId.ToString());
                            
                            table.Cell().Element(BodyCell).Text(item.AppointmentDate.ToString("dd-MM-yy"));

                            table.Cell().Element(BodyCell).Text(item.BillDate.ToString("dd-MM-yyyy"));

                            table.Cell().Element(BodyCell).Text(item.DoctorName);
                           
                            table.Cell().Element(BodyCell).Text(item.PatientName);

                            table.Cell().Element(BodyCell).Text(item.ConsultationFee.ToString());

                            table.Cell().Element(BodyCell).Text(item.AdditionalCharges.ToString());

                            table.Cell().Element(BodyCell).Text(item.Discount.ToString());
                            
                            table.Cell().Element(BodyCell).Text(item.TotalAmount.ToString());

                            table.Cell().Element(BodyCell).Text(item.PaymentStatus.ToString());
                        }
                    });

                    // FOOTER
                    BuildFooter(page);

                });

            }).GeneratePdf();
        }

        public byte[] ExportDoctorsPdf(List<DoctorsReportDto> doctors)
        {

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    var reportName = $"DoctorReport-{DateTime.UtcNow.ToString("yyyy-MM-dd")}";
                    //Header
                    BuildHeader(page, reportName);


                    // CONTENT
                    page.Content().PaddingTop(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                columns.RelativeColumn();
                            }
                        });

                        // Header
                        table.Header(header =>
                        {

                            header.Cell().Element(HeaderCell).Text("Name").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Email").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Phone").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Qual.").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Spec.").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Exp.").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("fee").Bold().FontColor(Colors.White);
                        });

                        // Data
                        foreach (var item in doctors)
                        {
                            table.Cell().Element(BodyCell).Text(item.DoctorName);
                            table.Cell().Element(BodyCell).Text(item.Email);
                            table.Cell().Element(BodyCell).Text(item.PhoneNumber);
                            table.Cell().Element(BodyCell).Text(item.Qualification);

                            table.Cell().Element(BodyCell).Text(item.Specialization);

                            table.Cell().Element(BodyCell).Text(item.Experience.ToString());

                            table.Cell().Element(BodyCell).Text(item.ConsultationFee.ToString());


                        }
                    });

                    // FOOTER


                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

        public byte[] ExportPatientsPdf(List<PatientsReportDto> patients)
        {

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    var reportName = $"DoctorReport-{DateTime.UtcNow.ToString("yyyy-MM-dd")}";
                    //Header
                    BuildHeader(page, reportName);


                    // CONTENT
                    page.Content().PaddingTop(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                columns.RelativeColumn();
                            }
                        });

                        // Header
                        table.Header(header =>
                        {


                            header.Cell().Element(HeaderCell).Text("Id").Bold().FontColor(Colors.White);


                            header.Cell().Element(HeaderCell).Text("Name").Bold().FontColor(Colors.White);


                            header.Cell().Element(HeaderCell).Text("Email").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Phonenumber").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("DOB").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Age").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Gender").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Address").Bold().FontColor(Colors.White);
                        });

                        // Data
                        foreach (var item in patients)
                        {
                            table.Cell().Element(BodyCell).Text(item.Id.ToString());
                            
                            table.Cell().Element(BodyCell).Text(item.PatientName);

                            table.Cell().Element(BodyCell).Text(item.Email);
                            
                            table.Cell().Element(BodyCell).Text(item.PhoneNumber);

                            table.Cell().Element(BodyCell).Text(item.DateOfBirth.ToString("yyyy-MM-dd"));
                           

                            table.Cell().Element(BodyCell).Text(item.Age.ToString());

                            table.Cell().Element(BodyCell).Text(item.Gender);

                            table.Cell().Element(BodyCell).Text(item.Address);
                        }
                    });

                    // FOOTER


                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

    }
}
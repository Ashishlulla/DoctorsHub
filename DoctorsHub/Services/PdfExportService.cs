using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
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
                .Padding(3)
                .AlignCenter()
                .AlignMiddle();
        }

        private static IContainer BodyCell(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(3);
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
                    .PaddingTop(5)
                    .AlignCenter()
                    .Text(reportTitle)
                    .Bold()
                    .FontSize(12)
                    .FontColor(Colors.Teal.Darken2);

                column.Item()
                    .AlignCenter()
                    .Text($"Generated On : {DateTime.Now:dd MMM yyyy hh:mm tt}")
                    .FontSize(10);

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
                    page.Size(PageSizes.A4);
                    page.Margin(20);

                    var reportName = $"BillingReport-{DateTime.UtcNow.ToString("yyyy-MM-dd")}";
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
                            
                            header.Cell().Element(HeaderCell).Text("Id").Bold().FontColor(Colors.White);
                            
                            header.Cell().Element(HeaderCell).Text("Date").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Patient").Bold().FontColor(Colors.White);

                            header.Cell().Element(HeaderCell).Text("Doctor").Bold().FontColor(Colors.White);

          
                            header.Cell().Element(HeaderCell).Text("Total").Bold().FontColor(Colors.White);
                            header.Cell().Element(HeaderCell).Text("Status").Bold().FontColor(Colors.White);


                        });

                        // Data
                        foreach (var item in bills)
                        {
                            table.Cell().Element(BodyCell).Text(item.BillId.ToString());
                            table.Cell().Element(BodyCell).Text(item.AppointmentDate.ToString("dd-MM-yyyy"));
                            table.Cell().Element(BodyCell).Text(item.PatientName);
                            table.Cell().Element(BodyCell).Text(item.DoctorName);
                           
                            table.Cell().Element(BodyCell).Text(item.TotalAmount.ToString());
                            table.Cell().Element(BodyCell).Text(item.PaymentStatus.ToString());

                        }
                    });

                    // FOOTER


                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

    }
}
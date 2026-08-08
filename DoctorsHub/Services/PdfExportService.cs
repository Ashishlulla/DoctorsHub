using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;
using DoctorsHub.Application.DTOs.Reports.PatientsReport;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

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
        public byte[] ExportAppointmentsPdf(List<AppointmentReportDto> appointments, AppointmentReportFilteredDto filter)
        {

            var fromDate = filter.FromDate == default
                ? appointments.Min(a => a.AppointmentDate)
                : filter.FromDate;

            var toDate = filter.ToDate == default
                ? appointments.Max(a => a.AppointmentDate)
                : filter.ToDate;

            

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    var reportName = $"AppointmentsReport-{DateTime.UtcNow:yyyy-MM-dd}";

                    // Header
                    BuildHeader(page, reportName);

                    // Content
                    page.Content().PaddingTop(20).Column(column =>
                    {
                        // Applied Filters
                        column.Item().PaddingBottom(15).Column(filters =>
                        {
                            filters.Item()
                                .Text("Applied Filters")
                                .Bold()
                                .FontSize(12);

                            filters.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem()
                             .Text($"From Date : {fromDate:dd-MMM-yyyy}");

                                            row.RelativeItem()
                                                .Text($"To Date : {toDate:dd-MMM-yyyy}");
                            });

                            filters.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Doctor : {filter.DoctorName ?? "All"}");

                                row.RelativeItem()
                                    .Text($"Patient : {filter.PatientName ?? "All"}");
                            });

                            filters.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Status : {filter.Status?.ToString() ?? "All"}");

                                row.RelativeItem()
                                    .Text($"Total Records : {appointments.Count}");
                            });

                            filters.Item()
                                .PaddingTop(5)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten2);
                        });

                        // Table
                        column.Item().Table(table =>
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
                    });

                    // Footer
                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

        public byte[] ExportBillingPdf(List<BillingReportDto> bills, BillingReportFilterDto filter)
        {

            var fromDate = filter.FromDate == default
               ? bills.Min(a => a.BillDate)
               : filter.FromDate;

            var toDate = filter.ToDate == default
                ? bills.Max(a => a.BillDate)
                : filter.ToDate;



            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);

                    var reportName = $"BillingReport-{DateTime.UtcNow:yyyy-MM-dd}";

                    // Header
                    BuildHeader(page, reportName);

                    // Content
                    page.Content().PaddingTop(20).Column(column =>
                    {
                        // Applied Filters
                        column.Item().PaddingBottom(15).Column(filters =>
                        {
                            filters.Item()
                                .Text("Applied Filters")
                                .Bold()
                                .FontSize(12);

                            filters.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem()
                             .Text($"From Date : {fromDate:dd-MMM-yyyy}");

                                row.RelativeItem()
                                    .Text($"To Date : {toDate:dd-MMM-yyyy}");
                            });

                            filters.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Doctor : {filter.DoctorName}");

                                row.RelativeItem()
                                    .Text($"Patient : {filter.PatientName}");
                            });

                            filters.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Status : {filter.PaymentStatus?.ToString() ?? "All"}");

                                row.RelativeItem()
                                    .Text($"Total Records : {bills.Count}");
                            });

                            filters.Item()
                                .PaddingTop(5)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten2);
                        });

                        // Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                for (int  i = 0;  i < 10;  i++)
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

                                table.Cell().Element(BodyCell).Text(item.BillDate.ToString("dd-MM-yy"));

                                table.Cell().Element(BodyCell).Text(item.DoctorName);

                                table.Cell().Element(BodyCell).Text(item.PatientName);

                                table.Cell().Element(BodyCell).Text(item.ConsultationFee.ToString());

                                table.Cell().Element(BodyCell).Text(item.AdditionalCharges.ToString());

                                table.Cell().Element(BodyCell).Text(item.Discount.ToString());

                                table.Cell().Element(BodyCell).Text(item.TotalAmount.ToString());

                                table.Cell().Element(BodyCell).Text(item.PaymentStatus.ToString());
                            }
                        });
                    });

                    // Footer
                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

        public byte[] ExportDoctorsPdf(List<DoctorsReportDto> doctors, DoctorsReportFilteredDto filter)
        {

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    var reportName = $"DoctorReport-{DateTime.UtcNow:yyyy-MM-dd}";

                    // Header
                    BuildHeader(page, reportName);

                    // Content
                    page.Content().PaddingTop(20).Column(column =>
                    {
                        // Applied Filters
                        column.Item().PaddingBottom(15).Column(filters =>
                        {
                            filters.Item()
                                .Text("Applied Filters")
                                .Bold()
                                .FontSize(12);

                            filters.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Qualification : {filter.Qualification ?? "All"}");

                                row.RelativeItem()
                                    .Text($"Specialization : {filter.SpecializationName ?? "All"}");
                            });

                            filters.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Total Records : {doctors.Count}");
                            });

                            filters.Item()
                                .PaddingTop(5)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten2);
                        });

                        // Table
                        column.Item().Table(table =>
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
                                header.Cell().Element(HeaderCell).Text("Fee").Bold().FontColor(Colors.White);
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
                    });

                    // Footer
                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

        public byte[] ExportPatientsPdf(List<PatientsReportDto> patients, PatientsReportFilteredDto filter)
        {

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    var reportName = $"PatientsReport-{DateTime.UtcNow:yyyy-MM-dd}";

                    // Header
                    BuildHeader(page, reportName);

                    // Content
                    page.Content().PaddingTop(20).Column(column =>
                    {
                        // Applied Filters
                        column.Item().PaddingBottom(15).Column(filters =>
                        {
                            filters.Item()
                                .Text("Applied Filters")
                                .Bold()
                                .FontSize(12);

                            filters.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Patient Name : {filter.PatientName ?? "All"}");

                                row.RelativeItem()
                                    .Text($"Gender : {filter.Gender ?? "All"}");
                            });

                            filters.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Blood Group : {filter.BloodGroup ?? "All"}");

                                row.RelativeItem()
                                    .Text($"Total Records : {patients.Count}");
                            });

                            filters.Item()
                                .PaddingTop(5)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten2);
                        });

                        // Table
                        column.Item().Table(table =>
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
                                header.Cell().Element(HeaderCell).Text("Phone").Bold().FontColor(Colors.White);
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
                                table.Cell().Element(BodyCell).Text(item.DateOfBirth.ToString("dd-MMM-yy"));
                                table.Cell().Element(BodyCell).Text(item.Age.ToString());
                                table.Cell().Element(BodyCell).Text(item.Gender);
                                table.Cell().Element(BodyCell).Text(item.Address);
                            }
                        });
                    });

                    // Footer
                    BuildFooter(page);
                });
            }).GeneratePdf();
        }

        public byte[] GenerateBillPdf(BillDto bill)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // Header
                    BuildHeader(page, $"Bill-{bill.Id}");

                    // Content
                    page.Content()
                        .PaddingTop(20)
                        .Column(column =>
                        {
                            // Bill Information
                            column.Item()
                                .PaddingBottom(15)
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Column(left =>
                                        {
                                            left.Item()
                                                .Text("BILL TO")
                                                .Bold()
                                                .FontSize(11);

                                            left.Item()
                                                .PaddingTop(5)
                                                .Text(bill.PatientName)
                                                .FontSize(12);
                                        });

                                    row.RelativeItem()
                                        .Column(right =>
                                        {
                                            right.Item()
                                                .AlignRight()
                                                .Text($"Bill No : {bill.Id}")
                                                .Bold();

                                            right.Item()
                                                .AlignRight()
                                                .PaddingTop(5)
                                                .Text($"Bill Date : {bill.BillDate:dd-MMM-yyyy}");
                                        });
                                });

                            // Doctor / Appointment
                            column.Item()
                                .PaddingVertical(10)
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(10)
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Text($"Doctor : {bill.DoctorName}");

                                    row.RelativeItem()
                                        .AlignRight()
                                        .Text($"Appointment ID : {bill.AppointmentId}");
                                });

                            // Charges
                            column.Item()
                                .PaddingTop(15)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(1);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell()
                                            .Element(HeaderCell)
                                            .Text("Description")
                                            .Bold()
                                            .FontColor(Colors.White);

                                        header.Cell()
                                            .Element(HeaderCell)
                                            .AlignRight()
                                            .Text("Amount")
                                            .Bold()
                                            .FontColor(Colors.White);
                                    });

                                    table.Cell()
                                        .Element(BodyCell)
                                        .Text("Consultation Fee");

                                    table.Cell()
                                        .Element(BodyCell)
                                        .AlignRight()
                                        .Text($"₹ {bill.ConsultationFee:N2}");

                                    table.Cell()
                                        .Element(BodyCell)
                                        .Text("Additional Charges");

                                    table.Cell()
                                        .Element(BodyCell)
                                        .AlignRight()
                                        .Text($"₹ {bill.AdditionalCharges:N2}");

                                    table.Cell()
                                        .Element(BodyCell)
                                        .Text("Discount");

                                    table.Cell()
                                        .Element(BodyCell)
                                        .AlignRight()
                                        .Text($"- ₹ {bill.Discount:N2}");

                                    table.Cell()
                                        .ColumnSpan(2)
                                        .PaddingTop(8)
                                        .LineHorizontal(1)
                                        .LineColor(Colors.Grey.Lighten2);

                                    table.Cell()
                                        .ColumnSpan(1)
                                        .PaddingTop(8)
                                        .AlignRight()
                                        .Text("TOTAL")
                                        .Bold()
                                        .FontSize(12);

                                    table.Cell()
                                        .Element(BodyCell)
                                        .AlignRight()
                                        .Text($"₹ {bill.TotalAmount:N2}")
                                        .Bold()
                                        .FontSize(12);
                                });

                            // Payment Status + Authorized Stamp
                            var stampPath = Path.Combine(
                                _environment.WebRootPath,
                                "images",
                                "stamp.png");

                            column.Item()
                                .PaddingTop(20)
                                .Row(row =>
                                {
                                    // Payment Status
                                    row.RelativeItem()
                                        .AlignLeft()
                                        .AlignMiddle()
                                        .Text($"Payment Status : {bill.PaymentStatus}")
                                        .Bold();

                                    // Authorized Stamp
                                    row.RelativeItem()
                                        .AlignRight()
                                        .Column(stamp =>
                                        {
                                            stamp.Item()
                                                .Width(70)
                                                .Height(70)
                                                .Image(stampPath)
                                                
                                                .FitArea();

                                            stamp.Item()
                                                .PaddingTop(0)
                                                .AlignRight()
                                                .Text("Authorized Signature")
                                                .Bold()
                                                .FontSize(10);
                                        });
                                });

                            // Thank You Message
                            column.Item()
                                .PaddingTop(25)
                                .AlignCenter()
                                .Text("Thank you for choosing DoctorsHub")
                                .FontSize(11);
                        });

                    // Footer
                    BuildFooter(page);
                });
            }).GeneratePdf();
        }
    }
}

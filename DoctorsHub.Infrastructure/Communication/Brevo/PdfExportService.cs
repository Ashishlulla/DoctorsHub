using DoctorsHub.Application.DTOs.Billing;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.Extensions.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DoctorsHub.Infrastructure.Communication.Brevo
{
    public class PdfExportService : IPdfExportService
    {
        //Private Feilds
        private readonly IHostEnvironment _environment;

        public PdfExportService(IHostEnvironment environment)
        {
            _environment = environment;
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
                            var stampPath = @"C:\Users\Admin\source\repos\Ashishlulla\DoctorsHub\DoctorsHub\wwwroot\images\stamp.png";

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
                                            if (File.Exists(stampPath))
                                            {
                                                stamp.Item()
                                                    .Width(70)
                                                    .Height(70)
                                                    .Image(stampPath)
                                                    .FitArea();
                                            }

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
            var logoPath = @"C:\Users\Admin\source\repos\Ashishlulla\DoctorsHub\DoctorsHub\wwwroot\images\DoctorsHubLogo.png";

            if (File.Exists(logoPath))
            {
                return File.ReadAllBytes(logoPath);
            }


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
    }
}
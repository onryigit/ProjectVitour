using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.ReservationServices;
using ProjectVitour.Services.TourServices;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace ProjectVitour.Controllers
{
    public class AdminReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITourService _tourService;

        public AdminReservationController(IReservationService reservationService, ITourService tourService)
        {
            _reservationService = reservationService;
            _tourService = tourService;
        }

        public async Task<IActionResult> ReservationList()
        {
            var values = await _reservationService.GetAllReservationAsync();
            ViewBag.Tours = await _tourService.GetAllToursAsync();
            return View(values);
        }

        public async Task<IActionResult> DeleteReservation(string id)
        {
            await _reservationService.DeleteReservationAsync(id);
            return RedirectToAction("ReservationList");
        }

        // --- GERÇEK EXCEL RAPORLAMA ---
        public async Task<IActionResult> ExportToExcel()
        {
            var reservations = await _reservationService.GetAllReservationAsync();
            var tours = await _tourService.GetAllToursAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Rezervasyonlar");
                
                // Başlıklar
                worksheet.Cell(1, 1).Value = "Müşteri Ad Soyad";
                worksheet.Cell(1, 2).Value = "E-Posta";
                worksheet.Cell(1, 3).Value = "Tur Adı";
                worksheet.Cell(1, 4).Value = "Kişi Sayısı";
                worksheet.Cell(1, 5).Value = "Tarih";

                // Stil verme
                var headerRange = worksheet.Range("A1:E1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.Emerald;
                headerRange.Style.Font.FontColor = XLColor.White;

                int row = 2;
                foreach (var item in reservations)
                {
                    var tourName = tours.FirstOrDefault(x => x.TourID == item.TourID)?.Title ?? "Bilinmiyor";
                    worksheet.Cell(row, 1).Value = item.FullName;
                    worksheet.Cell(row, 2).Value = item.Email;
                    worksheet.Cell(row, 3).Value = tourName;
                    worksheet.Cell(row, 4).Value = item.PersonCount;
                    worksheet.Cell(row, 5).Value = item.ReservationDate.ToString("dd.MM.yyyy HH:mm");
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Vitour_Rezervasyonlar.xlsx");
                }
            }
        }

        // --- GERÇEK PDF RAPORLAMA ---
        public async Task<IActionResult> ExportToPdf()
        {
            var reservations = await _reservationService.GetAllReservationAsync();
            var tours = await _tourService.GetAllToursAsync();

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Başlık
                Paragraph header = new Paragraph("Vitour Rezervasyon Raporu")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20);
                document.Add(header);

                document.Add(new Paragraph("\n"));

                // Tablo (5 Kolon)
                Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
                table.AddHeaderCell("Müşteri");
                table.AddHeaderCell("E-Posta");
                table.AddHeaderCell("Tur");
                table.AddHeaderCell("Kişi");
                table.AddHeaderCell("Tarih");

                foreach (var item in reservations)
                {
                    var tourName = tours.FirstOrDefault(x => x.TourID == item.TourID)?.Title ?? "Bilinmiyor";
                    table.AddCell(item.FullName);
                    table.AddCell(item.Email);
                    table.AddCell(tourName);
                    table.AddCell(item.PersonCount.ToString());
                    table.AddCell(item.ReservationDate.ToString("dd.MM.yyyy"));
                }

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", "Vitour_Rezervasyonlar.pdf");
            }
        }
    }
}
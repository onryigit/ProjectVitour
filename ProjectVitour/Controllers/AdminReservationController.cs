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
                
                // Başlık Stili
                var titleRange = worksheet.Range("A1:F1");
                titleRange.Merge().Value = "VİTOUR REZERVASYON RAPORU";
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.FontSize = 16;
                titleRange.Style.Font.FontColor = XLColor.White;
                titleRange.Style.Fill.BackgroundColor = XLColor.Teal;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(1).Height = 30;

                // Sütun Başlıkları
                worksheet.Cell(2, 1).Value = "Müşteri Ad Soyad";
                worksheet.Cell(2, 2).Value = "E-Posta";
                worksheet.Cell(2, 3).Value = "Telefon";
                worksheet.Cell(2, 4).Value = "Tur Adı";
                worksheet.Cell(2, 5).Value = "Kişi Sayısı";
                worksheet.Cell(2, 6).Value = "Kayıt Tarihi";

                var headerRange = worksheet.Range("A2:F2");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Font.FontColor = XLColor.Black;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 3;
                foreach (var item in reservations)
                {
                    var tourName = tours.FirstOrDefault(x => x.TourID?.ToString() == item.TourID?.ToString())?.Title ?? "Bilinmeyen Tur";
                    worksheet.Cell(row, 1).Value = item.FullName;
                    worksheet.Cell(row, 2).Value = item.Email;
                    worksheet.Cell(row, 3).Value = item.Phone ?? "-";
                    worksheet.Cell(row, 4).Value = tourName;
                    worksheet.Cell(row, 5).Value = item.PersonCount;
                    worksheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 6).Value = item.ReservationDate.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Alternatif satır rengi
                    if (row % 2 == 0)
                    {
                        worksheet.Range($"A{row}:F{row}").Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    }
                    row++;
                }

                worksheet.Columns().AdjustToContents();
                worksheet.SheetView.FreezeRows(2);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Vitour_Rezervasyonlar_{DateTime.Now:yyyyMMdd}.xlsx");
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

                // Türkçe Karakter Desteği İçin Font (Arial varsa kullan, yoksa standart cp1254)
                PdfFont turkishFont;
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                if (System.IO.File.Exists(fontPath))
                {
                    turkishFont = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                }
                else
                {
                    turkishFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, "Cp1254", PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);
                }
                
                document.SetFont(turkishFont);

                // Başlık
                Paragraph header = new Paragraph("VİTOUR REZERVASYON RAPORU")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(22)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY);
                document.Add(header);
                
                document.Add(new Paragraph($"Oluşturulma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(10)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY));

                document.Add(new Paragraph("\n"));

                // Tablo (5 Kolon)
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 25, 25, 25, 10, 15 })).UseAllAvailableWidth();
                
                // Tablo Başlıkları
                string[] headers = { "Müşteri Ad Soyad", "E-Posta", "Tur Adı", "Kişi", "Tarih" };
                foreach (var h in headers)
                {
                    Cell cell = new Cell().Add(new Paragraph(h))
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(5);
                    table.AddHeaderCell(cell);
                }

                int rowNum = 0;
                foreach (var item in reservations)
                {
                    rowNum++;
                    var tourName = tours.FirstOrDefault(x => x.TourID?.ToString() == item.TourID?.ToString())?.Title ?? "Bilinmeyen Tur";
                    
                    var bg = rowNum % 2 == 0 ? new iText.Kernel.Colors.DeviceRgb(240, 248, 255) : iText.Kernel.Colors.ColorConstants.WHITE;

                    table.AddCell(new Cell().Add(new Paragraph(item.FullName)).SetBackgroundColor(bg).SetPadding(4));
                    table.AddCell(new Cell().Add(new Paragraph(item.Email)).SetBackgroundColor(bg).SetPadding(4));
                    table.AddCell(new Cell().Add(new Paragraph(tourName)).SetBackgroundColor(bg).SetPadding(4));
                    table.AddCell(new Cell().Add(new Paragraph(item.PersonCount.ToString())).SetBackgroundColor(bg).SetTextAlignment(TextAlignment.CENTER).SetPadding(4));
                    table.AddCell(new Cell().Add(new Paragraph(item.ReservationDate.ToString("dd.MM.yyyy"))).SetBackgroundColor(bg).SetTextAlignment(TextAlignment.CENTER).SetPadding(4));
                }

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", $"Vitour_Rezervasyonlar_{DateTime.Now:yyyyMMdd}.pdf");
            }
        }
    }
}
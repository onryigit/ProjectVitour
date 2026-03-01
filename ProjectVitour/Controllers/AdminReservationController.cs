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

        private static string ResolveReservationCode(string reservationCode, string reservationId)
        {
            if (!string.IsNullOrWhiteSpace(reservationCode))
            {
                return reservationCode;
            }

            if (string.IsNullOrWhiteSpace(reservationId))
            {
                return "-";
            }

            var suffixLength = Math.Min(5, reservationId.Length);
            return "#VIT-" + reservationId.Substring(reservationId.Length - suffixLength).ToUpper();
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

        public async Task<IActionResult> ExportToExcel()
        {
            var reservations = await _reservationService.GetAllReservationAsync();
            var tours = await _tourService.GetAllToursAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Rezervasyonlar");

                var titleRange = worksheet.Range("A1:G1");
                titleRange.Merge().Value = "VITOUR REZERVASYON RAPORU";
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.FontSize = 16;
                titleRange.Style.Font.FontColor = XLColor.White;
                titleRange.Style.Fill.BackgroundColor = XLColor.Teal;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(1).Height = 30;

                worksheet.Cell(2, 1).Value = "Rezervasyon Kodu";
                worksheet.Cell(2, 2).Value = "Musteri Ad Soyad";
                worksheet.Cell(2, 3).Value = "E-Posta";
                worksheet.Cell(2, 4).Value = "Telefon";
                worksheet.Cell(2, 5).Value = "Tur Adi";
                worksheet.Cell(2, 6).Value = "Kisi Sayisi";
                worksheet.Cell(2, 7).Value = "Kayit Tarihi";

                var headerRange = worksheet.Range("A2:G2");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Font.FontColor = XLColor.Black;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 3;
                foreach (var item in reservations)
                {
                    var tourName = tours.FirstOrDefault(x => x.TourID?.ToString() == item.TourID?.ToString())?.Title ?? "Bilinmeyen Tur";
                    worksheet.Cell(row, 1).Value = ResolveReservationCode(item.ReservationCode, item.ReservationID);
                    worksheet.Cell(row, 2).Value = item.FullName;
                    worksheet.Cell(row, 3).Value = item.Email;
                    worksheet.Cell(row, 4).Value = item.Phone ?? "-";
                    worksheet.Cell(row, 5).Value = tourName;
                    worksheet.Cell(row, 6).Value = item.PersonCount;
                    worksheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(row, 7).Value = item.ReservationDate.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    if (row % 2 == 0)
                    {
                        worksheet.Range($"A{row}:G{row}").Style.Fill.BackgroundColor = XLColor.AliceBlue;
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

        public async Task<IActionResult> ExportToPdf()
        {
            var reservations = await _reservationService.GetAllReservationAsync();
            var tours = await _tourService.GetAllToursAsync();

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                PdfFont turkishFont;
                PdfFont turkishBoldFont;
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                string boldFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialbd.ttf");
                if (System.IO.File.Exists(fontPath))
                {
                    turkishFont = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                    turkishBoldFont = System.IO.File.Exists(boldFontPath)
                        ? PdfFontFactory.CreateFont(boldFontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED)
                        : turkishFont;
                }
                else
                {
                    turkishFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, "Cp1254", PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);
                    turkishBoldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD, "Cp1254", PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);
                }

                document.SetFont(turkishFont);

                var header = new Paragraph("VITOUR REZERVASYON RAPORU")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(22)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY)
                    .SetFont(turkishBoldFont);
                document.Add(header);

                document.Add(new Paragraph($"Olusturulma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(10)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY));

                document.Add(new Paragraph("\n"));

                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 18, 22, 22, 20, 8, 10 })).UseAllAvailableWidth();

                string[] headers = { "Rez. Kodu", "Musteri", "E-Posta", "Tur", "Kisi", "Tarih" };
                foreach (var h in headers)
                {
                    Cell cell = new Cell().Add(new Paragraph(h).SetFont(turkishBoldFont))
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
                    var reservationCode = ResolveReservationCode(item.ReservationCode, item.ReservationID);
                    var bg = rowNum % 2 == 0
                        ? new iText.Kernel.Colors.DeviceRgb(240, 248, 255)
                        : iText.Kernel.Colors.ColorConstants.WHITE;

                    table.AddCell(new Cell().Add(new Paragraph(reservationCode)).SetBackgroundColor(bg).SetPadding(4));
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

        public async Task<IActionResult> ExportToExcelById(string id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();

            var tour = await _tourService.GetTourByIdAsync(reservation.TourID);
            var tourName = tour?.Title ?? "Bilinmeyen Tur";

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Rezervasyon Detayi");

                var titleRange = worksheet.Range("A1:B1");
                titleRange.Merge().Value = "VITOUR REZERVASYON DETAYI";
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.FontSize = 14;
                titleRange.Style.Font.FontColor = XLColor.White;
                titleRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a6b4a");
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(1).Height = 25;

                worksheet.Cell(2, 1).Value = "Rezervasyon Kodu:";
                worksheet.Cell(2, 2).Value = ResolveReservationCode(reservation.ReservationCode, reservation.ReservationID);

                worksheet.Cell(3, 1).Value = "Musteri Ad Soyad:";
                worksheet.Cell(3, 2).Value = reservation.FullName;

                worksheet.Cell(4, 1).Value = "E-Posta:";
                worksheet.Cell(4, 2).Value = reservation.Email;

                worksheet.Cell(5, 1).Value = "Telefon:";
                worksheet.Cell(5, 2).Value = reservation.Phone ?? "-";

                worksheet.Cell(6, 1).Value = "Tur Adi:";
                worksheet.Cell(6, 2).Value = tourName;

                worksheet.Cell(7, 1).Value = "Kisi Sayisi:";
                worksheet.Cell(7, 2).Value = reservation.PersonCount;

                worksheet.Cell(8, 1).Value = "Kayit Tarihi:";
                worksheet.Cell(8, 2).Value = reservation.ReservationDate.ToString("dd.MM.yyyy HH:mm");

                var headerRange = worksheet.Range("A2:A8");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Rezervasyon_{reservation.FullName.Replace(" ", "_")}.xlsx");
                }
            }
        }

        public async Task<IActionResult> ExportToPdfById(string id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();

            var tour = await _tourService.GetTourByIdAsync(reservation.TourID);
            var tourName = tour?.Title ?? "Bilinmeyen Tur";

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                PdfFont turkishFont;
                PdfFont turkishBoldFont;
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                string boldFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialbd.ttf");
                if (System.IO.File.Exists(fontPath))
                {
                    turkishFont = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                    turkishBoldFont = System.IO.File.Exists(boldFontPath)
                        ? PdfFontFactory.CreateFont(boldFontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED)
                        : turkishFont;
                }
                else
                {
                    turkishFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, "Cp1254", PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);
                    turkishBoldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD, "Cp1254", PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);
                }

                document.SetFont(turkishFont);

                var header = new Paragraph("VITOUR REZERVASYON DETAYI")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY)
                    .SetFont(turkishBoldFont);
                document.Add(header);

                document.Add(new Paragraph($"Olusturulma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(10)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY));

                document.Add(new Paragraph("\n"));

                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 })).UseAllAvailableWidth();

                void AddRow(string label, string value)
                {
                    var labelParagraph = new Paragraph(label).SetFont(turkishBoldFont);
                    var labelCell = new Cell().Add(labelParagraph).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetPadding(5);
                    table.AddCell(labelCell);
                    table.AddCell(new Cell().Add(new Paragraph(value)).SetPadding(5));
                }

                AddRow("Rezervasyon Kodu:", ResolveReservationCode(reservation.ReservationCode, reservation.ReservationID));
                AddRow("Musteri Ad Soyad:", reservation.FullName);
                AddRow("E-Posta:", reservation.Email);
                AddRow("Telefon:", reservation.Phone ?? "-");
                AddRow("Tur Adi:", tourName);
                AddRow("Kisi Sayisi:", reservation.PersonCount.ToString());
                AddRow("Kayit Tarihi:", reservation.ReservationDate.ToString("dd.MM.yyyy HH:mm"));

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", $"Rezervasyon_{reservation.FullName.Replace(" ", "_")}.pdf");
            }
        }
    }
}
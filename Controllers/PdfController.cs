using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using gestionReservas.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace gestionReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PdfController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("reporte-semanalPDF")]
        public async Task<IActionResult> GenerarReporteSemanal()
        {
            var hoy = DateTime.Today;
            var hace7dias = hoy.AddDays(-7);

            var reservas = await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.Usuario)
                .Where(r => r.Fecha >= hace7dias && r.Fecha <= hoy)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text($"Reporte de Reservas – Últimos 7 Días")
                        .FontSize(18)
                        .SemiBold().FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(10)
                        .Column(col =>
                        {
                            col.Item().Text($"Fecha de generación: {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}")
                                .FontSize(10).FontColor(Colors.Grey.Darken1);

                            // ✅ Línea separadora corregida (sin SpacingBottom encadenado inválidamente)
                            col.Item().Element(e =>
                            {
                                e.LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                                return e;
                            });
                            col.Item().Height(10); // Espaciado visual tras la línea

                            if (reservas.Any())
                            {
                                col.Item().Table(table =>
                                {
                                    // Columnas
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(2); // Fecha
                                        columns.RelativeColumn(3); // Usuario
                                        columns.RelativeColumn(3); // Cancha
                                        columns.RelativeColumn(2); // Hora inicio
                                        columns.RelativeColumn(2); // Hora fin
                                        columns.RelativeColumn(2); // Estado
                                    });

                                    // Encabezado
                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("Fecha").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Usuario").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Cancha").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Inicio").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Fin").SemiBold();
                                        header.Cell().Element(CellStyle).Text("Estado").SemiBold();

                                        static IContainer CellStyle(IContainer container) =>
                                            container.DefaultTextStyle(x => x.SemiBold())
                                                     .Padding(4)
                                                     .Background(Colors.Grey.Lighten3)
                                                     .BorderBottom(1);
                                    });

                                    // Datos
                                    foreach (var r in reservas)
                                    {
                                        table.Cell().Element(CellData).Text(r.Fecha.ToString("dd/MM/yyyy"));
                                        table.Cell().Element(CellData).Text(r.Usuario.Nombre);
                                        table.Cell().Element(CellData).Text(r.Cancha.Nombre);
                                        table.Cell().Element(CellData).Text(r.HoraInicio.ToString(@"hh\:mm"));
                                        table.Cell().Element(CellData).Text(r.HoraFin.ToString(@"hh\:mm"));
                                        table.Cell().Element(CellData).Text(r.Estado);

                                        static IContainer CellData(IContainer container) =>
                                            container.Padding(4).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                    }
                                });
                            }
                            else
                            {
                                col.Item().Text("No se encontraron reservas en los últimos 7 días.")
                                          .Italic().FontColor(Colors.Grey.Darken1);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text("Sistema de Reservas de Canchas – SharpSoft")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken2);
                });
            }).GeneratePdf(stream);

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream.ToArray(), "application/pdf", $"ReporteSemanal_{hoy:yyyyMMdd}.pdf");
        }
    }
}

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using gestionReservas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace gestionReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WordController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("reporte-semanalWRD")]
        public async Task<IActionResult> GenerarRankingClientes()
        {
            var hoy = DateTime.Today;
            var hace7dias = hoy.AddDays(-7);

            // Traer reservas con usuario
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Where(r => r.Fecha >= hace7dias && r.Fecha <= hoy)
                .ToListAsync();

            // Agrupar por usuario y contar
            var topClientes = reservas
                .GroupBy(r => new { r.Usuario.Nombre, r.Usuario.Documento })
                .Select(g => new
                {
                    Nombre = g.Key.Nombre,
                    Documento = g.Key.Documento,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(10)
                .ToList();

            // Crear el documento en memoria
            using var stream = new MemoryStream();
            using (var wordDoc = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Título
                var titulo = new Paragraph(new Run(new Text("Top 10 Clientes Activos – Últimos 7 Días")))
                {
                    ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center })
                };
                body.AppendChild(titulo);

                // Fecha de generación
                body.AppendChild(new Paragraph(new Run(new Text($"Fecha de generación: {DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}"))));
                body.AppendChild(new Paragraph(new Run(new Text($"Periodo analizado: {hace7dias:dd/MM/yyyy} a {hoy:dd/MM/yyyy}"))));
                body.AppendChild(new Paragraph(new Run(new Text("")))); // Línea en blanco

                // Tabla
                var table = new Table();

                // Encabezado de tabla
                var headerRow = new TableRow();
                headerRow.Append(
                    CreateCell("#"),
                    CreateCell("Nombre del Cliente"),
                    CreateCell("Documento"),
                    CreateCell("Número de Reservas")
                );
                table.Append(headerRow);

                // Filas con datos
                for (int i = 0; i < topClientes.Count; i++)
                {
                    var cliente = topClientes[i];
                    var row = new TableRow();
                    row.Append(
                        CreateCell((i + 1).ToString()),
                        CreateCell(cliente.Nombre),
                        CreateCell(cliente.Documento),
                        CreateCell(cliente.Cantidad.ToString())
                    );
                    table.Append(row);
                }

                body.Append(table);

                // Pie de página
                body.AppendChild(new Paragraph(new Run(new Text(""))));
                body.AppendChild(new Paragraph(new Run(new Text("Sistema de Reservas de Canchas")))
                {
                    ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center })
                });
            }

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Top10Clientes_{hoy:yyyyMMdd}.docx");
        }

        private TableCell CreateCell(string text)
        {
            var cell = new TableCell();
            cell.Append(new Paragraph(new Run(new Text(text))));
            return cell;
        }
    }
}

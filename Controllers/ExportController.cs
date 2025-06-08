using ClosedXML.Excel;
using gestionReservas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("reporte-mensual")]
        public async Task<IActionResult> GenerarExcelMensual()
        {
            var hoy = DateTime.Today;
            var hace7dias = hoy.AddDays(-7);

            var reservas = await _context.Reservas
                .Include(r => r.Cancha)
                .Where(r => r.Fecha >= hace7dias && r.Fecha <= hoy)
                .ToListAsync();


            var resumen = reservas
                .GroupBy(r => r.Cancha.Nombre)
                .Select(g => new
                {
                    Cancha = g.Key,
                    TotalReservas = g.Count(),
                    TotalHoras = g.Sum(r => (r.HoraFin - r.HoraInicio).TotalHours),
                    IngresoEstimado = g.Sum(r => (decimal)(r.HoraFin - r.HoraInicio).TotalHours * r.Cancha.PrecioPorHora)
                })
                .ToList();

            using var workbook = new XLWorkbook();
            var hoja = workbook.Worksheets.Add("Resumen Mensual");

            hoja.Cell(1, 1).Value = "Cancha";
            hoja.Cell(1, 2).Value = "Total de Reservas";
            hoja.Cell(1, 3).Value = "Total de Horas";
            hoja.Cell(1, 4).Value = "Ingreso Estimado ($)";

            for (int i = 0; i < resumen.Count; i++)
            {
                hoja.Cell(i + 2, 1).Value = resumen[i].Cancha;
                hoja.Cell(i + 2, 2).Value = resumen[i].TotalReservas;
                hoja.Cell(i + 2, 3).Value = resumen[i].TotalHoras;
                hoja.Cell(i + 2, 4).Value = resumen[i].IngresoEstimado;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var nombreArchivo = $"ReporteUltimaSemana_{hace7dias:yyyyMMdd}_a_{hoy:yyyyMMdd}.xlsx";

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                nombreArchivo);
        }
    }
}

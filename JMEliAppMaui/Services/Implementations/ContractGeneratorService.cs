using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.Services.Implementations
{
    /// <summary>
    /// Genera contratos escolares a partir de plantillas HTML.
    /// Llena placeholders con datos del alumno, tutor y ciclo.
    /// El HTML generado puede visualizarse en WebView o convertirse a PDF.
    /// </summary>
    public class ContractGeneratorService : IContractGeneratorService
    {
        private static readonly Dictionary<string, string> Templates = new()
        {
            ["Inscripcion"] = InscripcionTemplate,
            ["Reinscripcion"] = ReinscripcionTemplate,
            ["Baja"] = BajaTemplate
        };

        public List<string> GetAvailableTemplates()
        {
            return Templates.Keys.ToList();
        }

        public async Task<string> GenerateContractHtmlAsync(
            StudentModel student, ClientModel client, CycleModel cycle, string contractType)
        {
            if (!Templates.TryGetValue(contractType, out var template))
            {
                throw new ArgumentException($"Template '{contractType}' not found. Available: {string.Join(", ", Templates.Keys)}");
            }

            var html = ReplacePlaceholders(template, student, client, cycle);
            return await Task.FromResult(html);
        }

        public async Task<string> GenerateAndSaveContractAsync(
            StudentModel student, ClientModel client, CycleModel cycle, string contractType)
        {
            var html = await GenerateContractHtmlAsync(student, client, cycle, contractType);

            var fileName = $"Contrato_{contractType}_{student.FullName?.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            await File.WriteAllTextAsync(filePath, html);

            return filePath;
        }

        private static string ReplacePlaceholders(string template, StudentModel student, ClientModel client, CycleModel cycle)
        {
            return template
                // Student
                .Replace("{{StudentFullName}}", student.FullName ?? "")
                .Replace("{{StudentGrade}}", student.Grade ?? "")
                .Replace("{{StudentLevel}}", student.Level ?? "")
                .Replace("{{StudentClave}}", student.Clave ?? "")
                .Replace("{{Tuition}}", student.Tuition ?? "")
                .Replace("{{MonthlyPayment}}", student.MonthlyPayment ?? student.Tuition ?? "")
                // Client (Tutor)
                .Replace("{{ClientFullName}}", client.FullName ?? "")
                .Replace("{{ClientEmail}}", client.Email ?? "")
                .Replace("{{ClientPhone}}", client.Phone ?? "")
                .Replace("{{ClientAddress}}", client.Address ?? "")
                .Replace("{{ClientRelationship}}", client.Relationship ?? "")
                .Replace("{{ClientIdNumber}}", client.Id ?? "")
                // Cycle
                .Replace("{{CycleName}}", cycle.Name ?? "")
                .Replace("{{CycleStartDate}}", cycle.StartDate ?? "")
                .Replace("{{CycleEndDate}}", cycle.EndDate ?? "")
                // Meta
                .Replace("{{CurrentDate}}", DateTime.Now.ToString("dd/MM/yyyy"))
                .Replace("{{SchoolName}}", "Joan Miró")
                .Replace("{{ContractId}}", Guid.NewGuid().ToString("N")[..8].ToUpper());
        }

        #region Templates

        private const string InscripcionTemplate = @"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Contrato de Inscripción - {{SchoolName}}</title>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; margin: 40px; line-height: 1.6; color: #333; }
        .header { text-align: center; border-bottom: 2px solid #4a148c; padding-bottom: 20px; margin-bottom: 30px; }
        .header h1 { color: #4a148c; margin: 0; }
        .header h2 { color: #7b1fa2; margin: 5px 0; font-weight: normal; }
        .contract-id { color: #999; font-size: 12px; }
        .section { margin: 20px 0; }
        .section h3 { color: #4a148c; border-bottom: 1px solid #e0e0e0; padding-bottom: 5px; }
        .data-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 10px; }
        .data-item { padding: 8px; background: #f5f5f5; border-radius: 4px; }
        .data-item label { font-weight: bold; display: block; font-size: 11px; color: #666; text-transform: uppercase; }
        .data-item span { font-size: 14px; }
        .clause { margin: 10px 0; padding: 10px; border-left: 3px solid #7b1fa2; background: #fafafa; }
        .signatures { display: grid; grid-template-columns: 1fr 1fr; gap: 40px; margin-top: 60px; text-align: center; }
        .signature-line { border-top: 1px solid #333; padding-top: 10px; margin-top: 60px; }
        .footer { margin-top: 40px; text-align: center; font-size: 11px; color: #999; border-top: 1px solid #e0e0e0; padding-top: 10px; }
    </style>
</head>
<body>
    <div class=""header"">
        <h1>{{SchoolName}}</h1>
        <h2>Contrato de Inscripción</h2>
        <p class=""contract-id"">Folio: {{ContractId}} | Fecha: {{CurrentDate}}</p>
    </div>

    <div class=""section"">
        <h3>Datos del Alumno</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Nombre completo</label><span>{{StudentFullName}}</span></div>
            <div class=""data-item""><label>Clave</label><span>{{StudentClave}}</span></div>
            <div class=""data-item""><label>Nivel</label><span>{{StudentLevel}}</span></div>
            <div class=""data-item""><label>Grado</label><span>{{StudentGrade}}</span></div>
        </div>
    </div>

    <div class=""section"">
        <h3>Datos del Tutor / Responsable</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Nombre completo</label><span>{{ClientFullName}}</span></div>
            <div class=""data-item""><label>Parentesco</label><span>{{ClientRelationship}}</span></div>
            <div class=""data-item""><label>Teléfono</label><span>{{ClientPhone}}</span></div>
            <div class=""data-item""><label>Email</label><span>{{ClientEmail}}</span></div>
            <div class=""data-item""><label>Domicilio</label><span>{{ClientAddress}}</span></div>
        </div>
    </div>

    <div class=""section"">
        <h3>Ciclo Escolar</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Ciclo</label><span>{{CycleName}}</span></div>
            <div class=""data-item""><label>Inicio</label><span>{{CycleStartDate}}</span></div>
            <div class=""data-item""><label>Fin</label><span>{{CycleEndDate}}</span></div>
            <div class=""data-item""><label>Colegiatura mensual</label><span>${{Tuition}}</span></div>
        </div>
    </div>

    <div class=""section"">
        <h3>Cláusulas</h3>
        <div class=""clause"">
            <strong>PRIMERA.</strong> El tutor se compromete a inscribir al alumno en el ciclo escolar {{CycleName}} 
            y a cubrir puntualmente la colegiatura mensual de ${{Tuition}} MXN.
        </div>
        <div class=""clause"">
            <strong>SEGUNDA.</strong> El pago deberá realizarse dentro de los primeros 10 días naturales de cada mes.
            En caso de retraso se aplicará un recargo del 10%.
        </div>
        <div class=""clause"">
            <strong>TERCERA.</strong> El tutor se compromete a respetar el reglamento interno de la institución, 
            los horarios establecidos y las disposiciones de seguridad.
        </div>
        <div class=""clause"">
            <strong>CUARTA.</strong> Solo las personas registradas como autorizadas podrán recoger al alumno. 
            Cualquier cambio deberá notificarse por escrito con anticipación.
        </div>
        <div class=""clause"">
            <strong>QUINTA.</strong> La baja del alumno deberá solicitarse por escrito con al menos 15 días de anticipación. 
            No se realizarán devoluciones de mensualidades ya cubiertas.
        </div>
    </div>

    <div class=""signatures"">
        <div>
            <div class=""signature-line"">
                <p><strong>{{ClientFullName}}</strong></p>
                <p>Tutor / Responsable</p>
            </div>
        </div>
        <div>
            <div class=""signature-line"">
                <p><strong>Dirección</strong></p>
                <p>{{SchoolName}}</p>
            </div>
        </div>
    </div>

    <div class=""footer"">
        <p>Este contrato es válido para el ciclo {{CycleName}} ({{CycleStartDate}} - {{CycleEndDate}})</p>
        <p>{{SchoolName}} — Documento generado el {{CurrentDate}}</p>
    </div>
</body>
</html>";

        private const string ReinscripcionTemplate = @"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Contrato de Reinscripción - {{SchoolName}}</title>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; margin: 40px; line-height: 1.6; color: #333; }
        .header { text-align: center; border-bottom: 2px solid #1565c0; padding-bottom: 20px; margin-bottom: 30px; }
        .header h1 { color: #1565c0; margin: 0; }
        .header h2 { color: #1976d2; margin: 5px 0; font-weight: normal; }
        .contract-id { color: #999; font-size: 12px; }
        .section { margin: 20px 0; }
        .section h3 { color: #1565c0; border-bottom: 1px solid #e0e0e0; padding-bottom: 5px; }
        .data-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 10px; }
        .data-item { padding: 8px; background: #f5f5f5; border-radius: 4px; }
        .data-item label { font-weight: bold; display: block; font-size: 11px; color: #666; text-transform: uppercase; }
        .data-item span { font-size: 14px; }
        .clause { margin: 10px 0; padding: 10px; border-left: 3px solid #1976d2; background: #fafafa; }
        .signatures { display: grid; grid-template-columns: 1fr 1fr; gap: 40px; margin-top: 60px; text-align: center; }
        .signature-line { border-top: 1px solid #333; padding-top: 10px; margin-top: 60px; }
        .footer { margin-top: 40px; text-align: center; font-size: 11px; color: #999; }
    </style>
</head>
<body>
    <div class=""header"">
        <h1>{{SchoolName}}</h1>
        <h2>Contrato de Reinscripción</h2>
        <p class=""contract-id"">Folio: {{ContractId}} | Fecha: {{CurrentDate}}</p>
    </div>

    <div class=""section"">
        <h3>Datos del Alumno</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Nombre</label><span>{{StudentFullName}}</span></div>
            <div class=""data-item""><label>Clave</label><span>{{StudentClave}}</span></div>
            <div class=""data-item""><label>Nivel</label><span>{{StudentLevel}}</span></div>
            <div class=""data-item""><label>Grado</label><span>{{StudentGrade}}</span></div>
        </div>
    </div>

    <div class=""section"">
        <h3>Tutor</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Nombre</label><span>{{ClientFullName}}</span></div>
            <div class=""data-item""><label>Teléfono</label><span>{{ClientPhone}}</span></div>
            <div class=""data-item""><label>Email</label><span>{{ClientEmail}}</span></div>
        </div>
    </div>

    <div class=""section"">
        <h3>Nuevo Ciclo Escolar</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Ciclo</label><span>{{CycleName}}</span></div>
            <div class=""data-item""><label>Período</label><span>{{CycleStartDate}} - {{CycleEndDate}}</span></div>
            <div class=""data-item""><label>Colegiatura</label><span>${{Tuition}} MXN/mes</span></div>
        </div>
    </div>

    <div class=""section"">
        <h3>Acuerdo de Reinscripción</h3>
        <div class=""clause"">
            Se renueva el compromiso educativo para el ciclo {{CycleName}} bajo las mismas condiciones 
            del contrato original, actualizando la colegiatura a ${{Tuition}} MXN mensuales.
        </div>
    </div>

    <div class=""signatures"">
        <div><div class=""signature-line""><p><strong>{{ClientFullName}}</strong></p><p>Tutor</p></div></div>
        <div><div class=""signature-line""><p><strong>Dirección</strong></p><p>{{SchoolName}}</p></div></div>
    </div>

    <div class=""footer"">
        <p>{{SchoolName}} — Reinscripción {{CycleName}} — {{CurrentDate}}</p>
    </div>
</body>
</html>";

        private const string BajaTemplate = @"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Formato de Baja - {{SchoolName}}</title>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; margin: 40px; line-height: 1.6; color: #333; }
        .header { text-align: center; border-bottom: 2px solid #c62828; padding-bottom: 20px; margin-bottom: 30px; }
        .header h1 { color: #c62828; margin: 0; }
        .header h2 { color: #e53935; margin: 5px 0; font-weight: normal; }
        .contract-id { color: #999; font-size: 12px; }
        .section { margin: 20px 0; }
        .section h3 { color: #c62828; border-bottom: 1px solid #e0e0e0; padding-bottom: 5px; }
        .data-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 10px; }
        .data-item { padding: 8px; background: #f5f5f5; border-radius: 4px; }
        .data-item label { font-weight: bold; display: block; font-size: 11px; color: #666; text-transform: uppercase; }
        .data-item span { font-size: 14px; }
        .notice { padding: 15px; background: #fff3e0; border: 1px solid #ff9800; border-radius: 4px; margin: 15px 0; }
        .signatures { display: grid; grid-template-columns: 1fr 1fr; gap: 40px; margin-top: 60px; text-align: center; }
        .signature-line { border-top: 1px solid #333; padding-top: 10px; margin-top: 60px; }
        .footer { margin-top: 40px; text-align: center; font-size: 11px; color: #999; }
    </style>
</head>
<body>
    <div class=""header"">
        <h1>{{SchoolName}}</h1>
        <h2>Solicitud de Baja</h2>
        <p class=""contract-id"">Folio: {{ContractId}} | Fecha: {{CurrentDate}}</p>
    </div>

    <div class=""section"">
        <h3>Alumno</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Nombre</label><span>{{StudentFullName}}</span></div>
            <div class=""data-item""><label>Clave</label><span>{{StudentClave}}</span></div>
            <div class=""data-item""><label>Nivel/Grado</label><span>{{StudentLevel}} - {{StudentGrade}}</span></div>
        </div>
    </div>

    <div class=""section"">
        <h3>Tutor Solicitante</h3>
        <div class=""data-grid"">
            <div class=""data-item""><label>Nombre</label><span>{{ClientFullName}}</span></div>
            <div class=""data-item""><label>Teléfono</label><span>{{ClientPhone}}</span></div>
        </div>
    </div>

    <div class=""notice"">
        <strong>⚠️ Aviso:</strong> La baja es efectiva a partir de la fecha de este documento. 
        No se realizan devoluciones de mensualidades ya cubiertas. El expediente del alumno 
        se conserva por un período de 5 años.
    </div>

    <div class=""signatures"">
        <div><div class=""signature-line""><p><strong>{{ClientFullName}}</strong></p><p>Tutor</p></div></div>
        <div><div class=""signature-line""><p><strong>Dirección</strong></p><p>{{SchoolName}}</p></div></div>
    </div>

    <div class=""footer"">
        <p>{{SchoolName}} — Baja procesada {{CurrentDate}}</p>
    </div>
</body>
</html>";

        #endregion
    }
}

using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.Services.Implementations
{
    public class ContractGeneratorService : IContractGeneratorService
    {
        private static readonly Dictionary<string, string> Templates = new()
        {
            ["Inscripcion"] = FichaInscripcionTemplate,
            ["ContratoEscolar"] = ContratoServiciosTemplate,
            ["Reinscripcion"] = FichaInscripcionTemplate,
            ["Baja"] = BajaTemplate
        };

        public List<string> GetAvailableTemplates() => Templates.Keys.ToList();

        public async Task<string> GenerateContractHtmlAsync(StudentModel student, ClientModel client, CycleModel cycle, string contractType)
        {
            if (!Templates.TryGetValue(contractType, out var template))
                template = Templates["Inscripcion"];

            var html = ReplacePlaceholders(template, student, client, cycle);
            return await Task.FromResult(html);
        }

        public async Task<string> GenerateAndSaveContractAsync(StudentModel student, ClientModel client, CycleModel cycle, string contractType)
        {
            var html = await GenerateContractHtmlAsync(student, client, cycle, contractType);
            var fileName = $"Contrato_{contractType}_{student.FullName?.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.html";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllTextAsync(filePath, html);
            return filePath;
        }

        private static string ReplacePlaceholders(string template, StudentModel student, ClientModel client, CycleModel cycle)
        {
            return template
                .Replace("{{StudentFullName}}", student.FullName ?? "")
                .Replace("{{StudentGrade}}", student.Grade ?? "")
                .Replace("{{StudentLevel}}", student.Level ?? "")
                .Replace("{{StudentDOB}}", student.DateOfBirth?.ToString("dd/MM/yyyy") ?? "")
                .Replace("{{StudentReligion}}", student.Religion ?? "")
                .Replace("{{StudentAddress}}", student.Address ?? "")
                .Replace("{{StudentPhone}}", student.Phone ?? "")
                .Replace("{{StudentAllergies}}", student.Allergies ?? "Ninguna")
                .Replace("{{StudentBloodType}}", student.BloodType ?? "")
                .Replace("{{StudentPreviousSchool}}", student.PreviousSchool ?? "")
                .Replace("{{StudentCURP}}", student.CURP ?? "")
                .Replace("{{StudentObservations}}", student.Observations ?? "")
                .Replace("{{ClientFullName}}", client.FullName ?? "")
                .Replace("{{ClientEmail}}", client.Email ?? "")
                .Replace("{{ClientPhone}}", client.Phone ?? "")
                .Replace("{{ClientAddress}}", client.Address ?? "")
                .Replace("{{ClientOccupation}}", client.Occupation ?? "")
                .Replace("{{ClientWork}}", client.Work ?? "")
                .Replace("{{ClientWorkPhone}}", client.WorkPhone ?? "")
                .Replace("{{ClientINE}}", client.INE ?? "")
                .Replace("{{ClientCURP}}", client.CURP ?? "")
                .Replace("{{ClientRelationship}}", client.Relationship ?? "")
                .Replace("{{ClientEmergencyPhone}}", client.EmergencyPhone ?? client.Phone ?? "")
                .Replace("{{CycleName}}", cycle.Name ?? "")
                .Replace("{{CycleStartDate}}", cycle.StartDate ?? "")
                .Replace("{{CycleEndDate}}", cycle.EndDate ?? "")
                .Replace("{{Tuition}}", student.Tuition ?? "")
                .Replace("{{CurrentDate}}", DateTime.Now.ToString("dd/MM/yyyy"))
                .Replace("{{SchoolName}}", "Colegio Joan Miró")
                .Replace("{{SchoolKey}}", "07PJN0312G / 07PPR0585N")
                .Replace("{{SchoolAddress}}", "Periférico Sur Poniente No. 2115-A, Col. Penipak, Tuxtla Gutiérrez, Chiapas")
                .Replace("{{ContractId}}", Guid.NewGuid().ToString("N")[..8].ToUpper());
        }

        #region Templates

        private const string ContratoServiciosTemplate = @"<!DOCTYPE html><html lang='es'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>Contrato de Servicios Escolares</title><style>body{font-family:'Segoe UI',Arial,sans-serif;margin:30px;line-height:1.7;color:#333;font-size:13px}h1{text-align:center;color:#4a148c;font-size:18px}h2{color:#4a148c;font-size:14px;border-bottom:1px solid #e0e0e0;padding-bottom:4px}.header{text-align:center;margin-bottom:20px}.clause{margin:10px 0;text-align:justify}.data{font-weight:bold;color:#1565c0;border-bottom:1px dotted #999}.signatures{display:grid;grid-template-columns:1fr 1fr;gap:40px;margin-top:60px;text-align:center}.sig-line{border-top:1px solid #333;padding-top:8px;margin-top:50px}.footer{margin-top:30px;text-align:center;font-size:10px;color:#999}</style></head><body>
<div class='header'><h1>PRESTACIÓN DE SERVICIOS</h1><h2>CONTRATO</h2></div>
<p class='clause'>Que celebran por una parte el <strong>{{SchoolName}}</strong>, representada en este acto por la C. Mtra. Elizabeth Pereyra Thomas y por la otra el (la) Sr.(a) <span class='data'>{{ClientFullName}}</span> en su carácter de padre o tutor al tenor de las siguientes declaraciones y cláusulas.</p>
<h2>DECLARACIONES</h2>
<p class='clause'>1.- Para efectos de mayor brevedad en lo sucesivo se denominará al {{SchoolName}}, como El Colegio y a él (la) Sr.(a), como padre o tutor.</p>
<p class='clause'>2.- El Colegio se declara estar legalmente establecida ante la Secretaría de Educación, con clave <span class='data'>{{SchoolKey}}</span>, con domicilio en {{SchoolAddress}}.</p>
<p class='clause'>3.- ""EL PADRE O TUTOR"" declara que es una persona física con capacidad jurídica y con domicilio particular en <span class='data'>{{ClientAddress}}</span>, que ha inscrito a <span class='data'>{{StudentFullName}}</span> como alumno(a) de El Colegio para el ciclo escolar <span class='data'>{{CycleName}}</span>, en el grado <span class='data'>{{StudentGrade}}</span>, de la fase <span class='data'>{{StudentLevel}}</span> y que tiene la solvencia económica suficiente para comprometerse a cubrir los pagos por concepto de inscripción, colegiaturas y los eventos socio-culturales que el colegio organiza.</p>
<h2>CLÁUSULAS</h2>
<p class='clause'><strong>Primera.</strong> EL COLEGIO se compromete a prestar en sus instalaciones el servicio de educación en el nivel correspondiente de acuerdo a los programas y planes de estudio autorizados por la Secretaría de Educación.</p>
<p class='clause'><strong>Segunda.</strong> EL COLEGIO se compromete a mantener debidamente informado al padre o tutor del desempeño académico del alumno(a) a través de la entrega de boletines, reuniones con padres de familia y juntas generales.</p>
<p class='clause'><strong>Tercera.</strong> EL PADRE O TUTOR se obliga a respetar y cumplir las indicaciones que EL COLEGIO estipule en su reglamento académico-disciplinario y administrativo.</p>
<p class='clause'><strong>Cuarta.</strong> EL PADRE O TUTOR al inscribir a su hijo(a) es consciente de que EL COLEGIO comparte el compromiso de formación integral del alumno, reconociendo la co-responsabilidad en dicha formación.</p>
<p class='clause'><strong>Quinta.</strong> El costo de los servicios educativos: la colegiatura mensual es de $<span class='data'>{{Tuition}}</span> MXN y deberá ser cubierta dentro de los primeros 10 días naturales del mes. En caso de retraso se aplicará un recargo del 10%.</p>
<p class='clause'><strong>Sexta.</strong> La baja del alumno deberá solicitarse por escrito con al menos 15 días de anticipación. No se realizarán devoluciones de mensualidades ya cubiertas.</p>
<p class='clause'><strong>Séptima.</strong> Solo las personas registradas como autorizadas podrán recoger al alumno. Cualquier cambio deberá notificarse por escrito.</p>
<p class='clause'><strong>Octava.</strong> INE del tutor: <span class='data'>{{ClientINE}}</span></p>
<p class='clause'>Leído que fue el presente contrato y enteradas las partes de su contenido y alcance legal, lo firman de conformidad en Tuxtla Gutiérrez, Chiapas a <span class='data'>{{CurrentDate}}</span>.</p>
<div class='signatures'><div><div class='sig-line'><strong>{{ClientFullName}}</strong><br>Padre / Tutor</div></div><div><div class='sig-line'><strong>Mtra. Elizabeth Pereyra Thomas</strong><br>{{SchoolName}}</div></div></div>
<div class='footer'>{{SchoolName}} — Contrato Ciclo {{CycleName}} — Folio: {{ContractId}}</div></body></html>";

        private const string FichaInscripcionTemplate = @"<!DOCTYPE html><html lang='es'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>Ficha de Inscripción</title><style>body{font-family:'Segoe UI',Arial,sans-serif;margin:30px;line-height:1.6;color:#333;font-size:12px}h1{text-align:center;color:#4a148c;font-size:18px}h2{color:#1565c0;font-size:13px;margin-top:16px}.row{display:grid;grid-template-columns:1fr 1fr;gap:10px;margin:4px 0}.field{margin:4px 0}.label{font-size:10px;color:#666;text-transform:uppercase}.value{font-weight:bold;border-bottom:1px dotted #ccc;min-height:18px;padding:2px 0}.header{text-align:center;border-bottom:2px solid #4a148c;padding-bottom:10px;margin-bottom:20px}.sig-line{border-top:1px solid #333;padding-top:8px;margin-top:50px;text-align:center}.footer{margin-top:20px;font-size:9px;color:#999;text-align:center}</style></head><body>
<div class='header'><h1>FICHA DE INSCRIPCIÓN — {{StudentLevel}}</h1><p>Fecha: {{CurrentDate}} | Grado: {{StudentGrade}} | Ciclo: {{CycleName}}</p></div>
<h2>DATOS DEL ALUMNO(A)</h2>
<div class='row'><div class='field'><div class='label'>Nombre completo</div><div class='value'>{{StudentFullName}}</div></div><div class='field'><div class='label'>CURP</div><div class='value'>{{StudentCURP}}</div></div></div>
<div class='row'><div class='field'><div class='label'>Fecha de nacimiento</div><div class='value'>{{StudentDOB}}</div></div><div class='field'><div class='label'>Religión</div><div class='value'>{{StudentReligion}}</div></div></div>
<div class='row'><div class='field'><div class='label'>Dirección</div><div class='value'>{{StudentAddress}}</div></div><div class='field'><div class='label'>Teléfono</div><div class='value'>{{StudentPhone}}</div></div></div>
<h2>DATOS DEL PADRE/TUTOR</h2>
<div class='row'><div class='field'><div class='label'>Nombre</div><div class='value'>{{ClientFullName}}</div></div><div class='field'><div class='label'>Parentesco</div><div class='value'>{{ClientRelationship}}</div></div></div>
<div class='row'><div class='field'><div class='label'>Ocupación</div><div class='value'>{{ClientOccupation}}</div></div><div class='field'><div class='label'>Lugar de trabajo</div><div class='value'>{{ClientWork}}</div></div></div>
<div class='row'><div class='field'><div class='label'>Teléfono</div><div class='value'>{{ClientPhone}}</div></div><div class='field'><div class='label'>Tel. trabajo</div><div class='value'>{{ClientWorkPhone}}</div></div></div>
<div class='row'><div class='field'><div class='label'>Email</div><div class='value'>{{ClientEmail}}</div></div><div class='field'><div class='label'>INE</div><div class='value'>{{ClientINE}}</div></div></div>
<div class='row'><div class='field'><div class='label'>Dirección</div><div class='value'>{{ClientAddress}}</div></div><div class='field'><div class='label'>CURP</div><div class='value'>{{ClientCURP}}</div></div></div>
<h2>DATOS MÉDICOS</h2>
<div class='row'><div class='field'><div class='label'>Alergias</div><div class='value'>{{StudentAllergies}}</div></div><div class='field'><div class='label'>Tipo de sangre</div><div class='value'>{{StudentBloodType}}</div></div></div>
<div class='field'><div class='label'>Observaciones</div><div class='value'>{{StudentObservations}}</div></div>
<h2>ESCUELA DE PROCEDENCIA</h2>
<div class='field'><div class='label'>Escuela anterior</div><div class='value'>{{StudentPreviousSchool}}</div></div>
<h2>CONTACTO DE EMERGENCIA</h2>
<div class='row'><div class='field'><div class='label'>Teléfono emergencia</div><div class='value'>{{ClientEmergencyPhone}}</div></div><div class='field'><div class='label'>Email</div><div class='value'>{{ClientEmail}}</div></div></div>
<div class='sig-line'><strong>{{ClientFullName}}</strong><br>Nombre y firma del Padre o Tutor</div>
<div class='footer'>{{SchoolName}} — Ficha de Inscripción {{CycleName}} — {{CurrentDate}}</div></body></html>";

        private const string BajaTemplate = @"<!DOCTYPE html><html lang='es'><head><meta charset='UTF-8'><title>Solicitud de Baja</title><style>body{font-family:'Segoe UI',Arial,sans-serif;margin:40px;line-height:1.6;color:#333}.header{text-align:center;border-bottom:2px solid #c62828;padding-bottom:20px}h1{color:#c62828}.data{font-weight:bold;color:#1565c0}.notice{padding:15px;background:#fff3e0;border:1px solid #ff9800;border-radius:4px;margin:15px 0}.sig-line{border-top:1px solid #333;padding-top:10px;margin-top:60px;text-align:center}</style></head><body><div class='header'><h1>{{SchoolName}}</h1><h2>Solicitud de Baja</h2><p>Fecha: {{CurrentDate}}</p></div><p>Alumno: <span class='data'>{{StudentFullName}}</span> | Grado: <span class='data'>{{StudentGrade}}</span> | Nivel: <span class='data'>{{StudentLevel}}</span></p><p>Tutor: <span class='data'>{{ClientFullName}}</span> | Tel: <span class='data'>{{ClientPhone}}</span></p><div class='notice'><strong>⚠️ Aviso:</strong> La baja es efectiva a partir de la fecha de este documento. No se realizan devoluciones de mensualidades ya cubiertas.</div><div class='sig-line'><strong>{{ClientFullName}}</strong><br>Tutor</div></body></html>";

        #endregion
    }
}

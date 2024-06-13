using iText.IO.Image;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using Photos;
using Foundation;
using JMEliAppMaui.Services;
using QuickLook;
using UIKit;


namespace JMEliAppMaui.Platforms
{
    public class Autorizados
    {
        public string? Nombre { get; set; }
        public string? Parentezco { get; set; }
        public string? Telefono { get; set; }
        public bool Autorizado { get; set; }
        public Autorizados() { Autorizado = false; }

    }
    public class FileService : IFileService
    {
        public bool NeedsPermission()
        {
            try
            {
                var status = PHPhotoLibrary.AuthorizationStatus;
                if (status == PHAuthorizationStatus.Denied
                    || status == PHAuthorizationStatus.NotDetermined
                    || status == PHAuthorizationStatus.Restricted)
                {
                    // Permission is denied or restricted
                    return true;
                }
                // Permission is granted //or not determined 
                return false;


            }
            catch (Exception ex)
            {
                // Handle exception if any
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public void RequestPermision()
        {
            PHPhotoLibrary.RequestAuthorization(status =>
            {
                if (status == PHAuthorizationStatus.Authorized)
                {
                    // Permission granted
                    Console.WriteLine("Photo library permission granted.");
                }
                else
                {
                    // Permission denied or restricted
                    Console.WriteLine("Photo library permission denied or restricted.");
                }
            }); ;
        }

        public async Task<string> CreatePdfAsync(ContractModel contract, Stream sign)
        {
            string fileName = GetFileName(contract.Url);
            string mainDir = FileSystem.Current.AppDataDirectory;
            string newDir = mainDir.Replace(@"AppData\Local\Packages\com.etho.jmeliappmaui_9zz4h110yvjzm\LocalState", "").Trim();
            var filePath = Path.Combine(newDir, "Downloads", fileName);

            using (PdfWriter writer = new PdfWriter(filePath))
            {
                iText.Layout.Element.Image? imageSign = null;
                if (sign != null)
                {
                    var stream = await ConvertImageSourceToStreamAsync(sign);
                    imageSign = new iText.Layout.Element.Image(ImageDataFactory
                        .Create(stream))
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                }
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, pdf.GetDefaultPageSize(), false);

                //inicio de documento
                Paragraph inscripcion = new Paragraph("FICHA DE INSCRIPCIÓN")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(12);
                document.Add(inscripcion);
                Paragraph space = new Paragraph("");
                document.Add(space);
                var bytes = await ConvertImageSourceToStreamAsync("user_icon_pdf.png");//images MUST GO IN TO RAW FOLDER as MAUI ASSET!!
                ImageData data = ImageDataFactory.Create(bytes);
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(data)
                                                       .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
                Paragraph photo = new Paragraph();
                photo.Add(image.ScaleToFit(120, 120));
                document.ShowTextAligned(photo, 530, 790, 1, iText.Layout.Properties.TextAlignment.RIGHT, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                Paragraph inscripcionBloque = new Paragraph("Fecha de inscripción:  ")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                    .SetFontSize(12);
                inscripcionBloque.Add(new Text("Fecha aqui").SetUnderline());
                inscripcionBloque.Add("\n");
                inscripcionBloque.Add("Horario: ");
                inscripcionBloque.Add(new Text("Turno aqui").SetUnderline());
                document.Add(inscripcionBloque);
                document.Add(space);
                //Datos del alumno
                Paragraph datos = new Paragraph("DATOS DEL ALUMNO")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(12);
                document.Add(datos);
                document.Add(space);

                Paragraph datosBloque = new Paragraph("Nombre del alumno:  ")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                   .SetFontSize(12);
                datosBloque.Add(new Text("nombre aqui").SetUnderline());
                document.Add(datosBloque);
                document.Add(space);

                Paragraph domicilio = new Paragraph("Domicilio particular:  ")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                   .SetFontSize(12);
                domicilio.Add(new Text("domicilio aqui").SetUnderline());
                document.Add(domicilio);
                document.Add(space);

                Paragraph fechanacimiento = new Paragraph("Fecha de nacimiento:  ")
                  .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                  .SetFontSize(12);
                fechanacimiento.Add(new Text("nacimiento aqui").SetUnderline());
                fechanacimiento.Add(new iText.Layout.Element.Tab());
                fechanacimiento.Add(new iText.Layout.Element.Tab());
                fechanacimiento.Add("CURP:  ");
                fechanacimiento.Add(new Text("CURP aqui").SetUnderline());
                document.Add(fechanacimiento);
                document.Add(space);

                Paragraph alergias = new Paragraph("Alergias (medicamentos o alimentos):  ")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                   .SetFontSize(12);
                alergias.Add(new Text("Alergias aqui").SetUnderline());
                document.Add(alergias);
                document.Add(space);

                Paragraph sangre = new Paragraph("Tipo de sangre:  ")
                  .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                  .SetFontSize(12);
                sangre.Add(new Text("sangre aqui").SetUnderline());
                document.Add(sangre);
                document.Add(space);
                //datos del padre
                Paragraph datosPadre = new Paragraph("DATOS DEL PADRE")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFontSize(12);
                document.Add(datosPadre);
                document.Add(space);

                Paragraph nombrePadre = new Paragraph("Nombre:  ")
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                 .SetFontSize(12);
                nombrePadre.Add(new Text("Nombre padre aqui").SetUnderline());
                document.Add(nombrePadre);
                document.Add(space);

                Paragraph escolaridadPadre = new Paragraph("Escolaridad:  ")
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                 .SetFontSize(12);
                escolaridadPadre.Add(new Text("escolaridad padre aqui").SetUnderline());
                escolaridadPadre.Add(new iText.Layout.Element.Tab());
                escolaridadPadre.Add(new iText.Layout.Element.Tab());
                escolaridadPadre.Add("Tel. Celular:  ");
                escolaridadPadre.Add(new Text("celular aqui").SetUnderline());
                document.Add(escolaridadPadre);
                document.Add(space);

                Paragraph ocupacionPadre = new Paragraph("Ocupacion:  ")
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                 .SetFontSize(12);
                ocupacionPadre.Add(new Text("ocupacion padre aqui").SetUnderline());
                document.Add(ocupacionPadre);
                document.Add(space);

                Paragraph institucionPadre = new Paragraph("Institucion donde trabaja:  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                institucionPadre.Add(new Text("institucion padre aqui").SetUnderline());
                document.Add(institucionPadre);
                document.Add(space);

                Paragraph oficinaPadre = new Paragraph("Tel. de oficina:  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                oficinaPadre.Add(new Text("oficina padre aqui").SetUnderline());
                document.Add(oficinaPadre);
                document.Add(space);
                //datos de la madre

                Paragraph datosMadre = new Paragraph("DATOS DE LA MADRE")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFontSize(12);
                document.Add(datosMadre);
                document.Add(space);

                Paragraph nombreMadre = new Paragraph("Nombre:  ")
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                 .SetFontSize(12);
                nombreMadre.Add(new Text("Nombre madre aqui").SetUnderline());
                document.Add(nombrePadre);
                document.Add(space);

                Paragraph escolaridadMadre = new Paragraph("Escolaridad:  ")
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                 .SetFontSize(12);
                escolaridadMadre.Add(new Text("escolaridad madre aqui").SetUnderline());
                escolaridadMadre.Add(new iText.Layout.Element.Tab());
                escolaridadMadre.Add(new iText.Layout.Element.Tab());
                escolaridadMadre.Add("Tel. Celular:  ");
                escolaridadMadre.Add(new Text("celular aqui").SetUnderline());
                document.Add(escolaridadMadre);
                document.Add(space);

                Paragraph ocupacionMadre = new Paragraph("Ocupacion:  ")
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                 .SetFontSize(12);
                ocupacionMadre.Add(new Text("ocupacion madre aqui").SetUnderline());
                document.Add(ocupacionMadre);
                document.Add(space);

                Paragraph institucionMadre = new Paragraph("Institucion donde trabaja:  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                institucionMadre.Add(new Text("institucion madre aqui").SetUnderline());
                document.Add(institucionMadre);
                document.Add(space);

                Paragraph oficinaMadre = new Paragraph("Tel. de oficina:  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                oficinaMadre.Add(new Text("oficina madre aqui").SetUnderline());
                document.Add(oficinaMadre);
                document.Add(space);

                Paragraph tutor = new Paragraph("Tutor  ")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
               .SetFontSize(12);
                string otrotutor = "madre";
                string result = GetTutor(otrotutor);
                tutor.Add(new Text(result));
                if (result.Equals("Madre()    Padre ()       Otro:  "))
                {
                    tutor.Add(new Text(otrotutor).SetUnderline());
                }
                document.Add(tutor);
                document.Add(space);

                Paragraph email = new Paragraph("E-mail:  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                email.Add(new Text("email aqui").SetUnderline());
                document.Add(email);
                document.Add(space);
                //pagina 2
                Paragraph header2 = new Paragraph("Relación de las personas que autoriza para recoger al alumno")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                document.Add(header2);
                document.Add(space);
                //tabla
                List<Autorizados> aut = new List<Autorizados>();
                Autorizados uno = new Autorizados
                {
                    Nombre = "Perengano Rodriguez Pereyra",
                    Parentezco = "Developer",
                    Telefono = "1561561689",
                    Autorizado = true
                };
                aut.Add(uno);
                Autorizados dos = new Autorizados
                {
                    Nombre = "Sutano Lopez Burguete",
                    Parentezco = "Abuelo",
                    Telefono = "1561561689",
                    Autorizado = false
                };
                aut.Add(dos);
                Autorizados tres = new Autorizados
                {
                    Nombre = "Fulano Espinoza Ruiz",
                    Parentezco = "Padrino",
                    Telefono = "1561561689",
                    Autorizado = true
                };
                aut.Add(tres);

                Table autorizados = new Table(4);
                autorizados.SetWidth(500);
                autorizados.AddCell(new Paragraph("Nombre")
                          .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                          .SetFontSize(12));
                autorizados.AddCell(new Paragraph("Parentesco")
                           .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                           .SetFontSize(12));
                autorizados.AddCell(new Paragraph("Telefono")
                           .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                           .SetFontSize(12));
                autorizados.AddCell(new Paragraph("Firma de autorizado")
                           .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                           .SetFontSize(12));

                if (aut.Count > 0)
                {
                    foreach (var autorizado in aut)
                    {
                        autorizados.AddCell(new Paragraph(autorizado.Nombre)
                             .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                             .SetFontSize(12)
                             .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));
                        autorizados.AddCell(new Paragraph(autorizado.Parentezco)
                                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                   .SetFontSize(12)
                                   .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));
                        autorizados.AddCell(new Paragraph(autorizado.Telefono)
                                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                   .SetFontSize(12)
                                   .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));
                        if (autorizado.Autorizado)
                        {
                            if (imageSign != null)
                            {
                                autorizados.AddCell(imageSign);
                            }
                            else
                            {
                                autorizados.AddCell(new Paragraph("")
                                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                                       .SetFontSize(12));
                            }
                        }
                        else
                        {
                            if (imageSign != null)
                            {
                                autorizados.AddCell(new Paragraph("Pendiente de autorizar")
                                      .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                      .SetFontSize(12));
                            }
                            else
                            {
                                autorizados.AddCell(new Paragraph(""));
                            }
                        }
                    }
                }
                document.Add(autorizados);
                document.Add(space);

                //termina tabla
                Paragraph accidente = new Paragraph("En caso de accidente avisar a:  ")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
               .SetFontSize(12);
                accidente.Add(new Text("Contacto de emergencia aqui").SetUnderline());
                document.Add(accidente);
                document.Add(space);
                //informacion general

                Paragraph general = new Paragraph("INFORMACIÓN GENERAL")
                  .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                  .SetFontSize(12);
                document.Add(general);
                document.Add(space);

                Paragraph sobrenombre = new Paragraph("Utiliza sobrenombre?  ")
                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                 .SetFontSize(12);
                sobrenombre.Add(new Text("sobrenombre aqui").SetUnderline());
                document.Add(sobrenombre);
                document.Add(space);

                Paragraph entrenado = new Paragraph("¿Esta entrenando para ir al baño?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                entrenado.Add(new Text("entrenado aqui").SetUnderline());
                document.Add(entrenado);
                document.Add(space);

                Paragraph idioma = new Paragraph("¿Qué idioma se habla en casa?  ")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
               .SetFontSize(12);
                idioma.Add(new Text("idioma aqui").SetUnderline());
                document.Add(idioma);
                document.Add(space);

                Paragraph evidencia = new Paragraph("¿Hay alguna evidencia de desarrollo tardío?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                evidencia.Add(new Text("evidencia aqui").SetUnderline());
                document.Add(evidencia);
                document.Add(space);

                Paragraph especifico = new Paragraph("Especifíquelo  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                especifico.Add(new Text("especificar aqui").SetUnderline());
                document.Add(especifico);
                document.Add(space);

                Paragraph fobia = new Paragraph("¿Tiene alguna fobia en especial?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                fobia.Add(new Text("fobia aqui").SetUnderline());
                document.Add(fobia);
                document.Add(space);

                Paragraph comer = new Paragraph("¿come solo, usa cuchara, toma en taza?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                comer.Add(new Text("especificar aqui").SetUnderline());
                document.Add(comer);
                document.Add(space);

                Paragraph palabras = new Paragraph("¿Dice algunas palabras?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                palabras.Add(new Text("especificar aqui").SetUnderline());
                palabras.Add(new iText.Layout.Element.Tab());
                palabras.Add("¿Cuáles?  ");
                palabras.Add(new Text("palabras aqui").SetUnderline());
                document.Add(palabras);
                document.Add(space);

                Paragraph bath = new Paragraph("¿A qué hora se baña?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                bath.Add(new Text("hora aqui").SetUnderline());
                document.Add(bath);
                document.Add(space);

                Paragraph duerme = new Paragraph("¿A qué hora se duerme?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                duerme.Add(new Text("hora aqui").SetUnderline());
                document.Add(duerme);
                document.Add(space);

                Paragraph despierta = new Paragraph("¿A qué hora se despierta?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                despierta.Add(new Text("hora aqui").SetUnderline());
                document.Add(despierta);
                document.Add(space);

                Paragraph siesta = new Paragraph("¿A qué hora toma su siesta?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                siesta.Add(new Text("hora aqui").SetUnderline());
                document.Add(siesta);
                document.Add(space);

                Paragraph desayuno = new Paragraph("¿A qué hora toma su desayuno?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                desayuno.Add(new Text("hora aqui").SetUnderline());
                document.Add(desayuno);
                document.Add(space);

                Paragraph come = new Paragraph("¿Qué come a medio día?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                come.Add(new Text("comida aqui").SetUnderline());
                document.Add(come);
                document.Add(space);

                Paragraph comida = new Paragraph("¿A qué hora toma la comida?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                comida.Add(new Text("hora aqui").SetUnderline());
                document.Add(comida);
                document.Add(space);

                Paragraph control = new Paragraph("¿Cómo es su control muscular?  ")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
                .SetFontSize(12);
                control.Add(new Text("especificar aqui").SetUnderline());
                document.Add(control);
                document.Add(space);

                Paragraph observaciones = new Paragraph("OBSERVACIONES")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED)
               .SetFontSize(12);
                observaciones.Add("\n");
                observaciones.Add(new Text("observaciones aqui").SetUnderline());
                document.Add(observaciones);
                document.Add(space);

                if (imageSign != null)
                {
                    document.Add(imageSign);
                    Paragraph footer = new Paragraph(new Text("Firmado el " + DateTime.Now.ToShortDateString()).SetBold());
                    for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                    {
                        document.ShowTextAligned(footer, 50, 10, i, iText.Layout.Properties.TextAlignment.LEFT, iText.Layout.Properties.VerticalAlignment.BOTTOM, 0);
                    }
                }

                Paragraph nombrepapa = new Paragraph("Perenganito papa de fulanito")
                  .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                  .SetFontSize(15);
                LineSeparator ls = new LineSeparator(new SolidLine()).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                ls.SetWidth(200);
                document.Add(ls);
                document.Add(nombrepapa);



                document.Close();
            }
            return filePath;
        }

        public bool FileExists(string ContractModelUrl)
        {
            string filename = GetFileName(ContractModelUrl);
            if (!string.IsNullOrEmpty(filename))
            {
                string internalPath = GetFilePath(filename);

                if (File.Exists(internalPath))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetWebviewUrl(string ContractModelUrl)
        {
            string fileName = GetFileName(ContractModelUrl);

            if (!string.IsNullOrEmpty(fileName))
            {
                return GetFilePath(fileName);
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> UpdatePdfAsync(ContractModel contract, Stream sign)
        {
            string fileName = GetFileName(contract.Url);
            string mainDir = FileSystem.Current.AppDataDirectory;
            string newDir = mainDir.Replace(@"AppData\Local\Packages\com.etho.jmeliappmaui_9zz4h110yvjzm\LocalState", "").Trim();
            var filePath = Path.Combine(newDir, "Downloads", fileName);

            HttpClient httpClient = new HttpClient();
            var content = await httpClient.GetAsync(contract.Url);
            var stream = new MemoryStream(await content.Content.ReadAsByteArrayAsync());
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                iText.Layout.Element.Image? imageSign = null;
                if (sign != null)
                {
                    var streamSign = await ConvertImageSourceToStreamAsync(sign);
                    imageSign = new iText.Layout.Element.Image(ImageDataFactory
                        .Create(streamSign))
                        .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                }
                PdfDocument pdf = new PdfDocument(new PdfReader(stream), writer);
                Document document = new Document(pdf);

                Paragraph footer = new Paragraph();
                footer.Add(imageSign);
                document.ShowTextAligned(footer, 530, 790, 1, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.MIDDLE, 0);
                document.Close();
            }

            return filePath;
        }

        static string GetFileName(string ContractModelUrl)
        {
            string fileName = string.Empty;
            var result = ContractModelUrl.Split("?alt");
            if (result != null && result.Count() > 1)
            {
                fileName = result[0].Replace("https://firebasestorage.googleapis.com/v0/b/joanmiroapp.appspot.com/o/StudentContrat%2F", "").Trim();
            }
            return fileName;
        }

        static string GetFilePath(string filename)
        {
            var downloadsPath = NSSearchPath.GetDirectories(NSSearchPathDirectory.DownloadsDirectory, NSSearchPathDomain.User, true).FirstOrDefault();
            var personal = Environment.GetFolderPath(Environment.SpecialFolder.Personal);


            // Construct the full path to the PDF file
            var pdfFileName = filename;
            var fulldownloadpath = Path.Combine(downloadsPath, pdfFileName);
            // Check if the file exists
            if (!File.Exists(fulldownloadpath))
            {
                Console.WriteLine($"PDF file '{pdfFileName}' not found at '{fulldownloadpath}'.");

            }


            var Personalpath = Path.Combine(personal, pdfFileName);
            // Check if the file exists
            if (!File.Exists(Personalpath))
            {
                Console.WriteLine($"PDF file '{pdfFileName}' not found at '{Personalpath}'.");

            }

            var icloudDriveDownloadpath = "/private/var/mobile/Library/Mobile Documents/com~apple~CloudDocs/Downloads/";





            var f1 = File.Exists(fulldownloadpath) || File.Exists(Path.Combine(icloudDriveDownloadpath, filename));

            //var fileUrl = new NSUrl(pdfFilePath, false);
            //var request = new NSUrlRequest(fileUrl);
            //await webView.LoadRequestAsync(request);
            return Path.Combine(fulldownloadpath, filename);
        }
        private async Task<byte[]> ConvertImageSourceToStreamAsync(Stream sign)
        {
            using var ms = new MemoryStream();

            await sign.CopyToAsync(ms);
            return ms.ToArray();
        }

        private async Task<byte[]> ConvertImageSourceToStreamAsync(string imageName)
        {
            using var ms = new MemoryStream();
            using (var stream = await FileSystem.OpenAppPackageFileAsync(imageName))
                await stream.CopyToAsync(ms);
            return ms.ToArray();
        }
        private string GetTutor(string tutor)
        {
            switch (tutor)
            {
                case "padre": return "Madre()    Padre (X)       Otro:  ";
                case "madre": return "Madre(X)    Padre ()       Otro:  ";
                default: return "Madre()    Padre ()       Otro:  ";
            }
        }

        public Task<string> SaveAndView(string fileName, MemoryStream stream)
        {
            return Task.FromResult(string.Empty);
        }

        public async Task SaveAndView(string fileName, MemoryStream stream, OpenOption context, string contentType = "application/pdf")
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, fileName);

            FileStream fileStream = File.Open(filePath, FileMode.Create);
            stream.Position = 0;

            await stream.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            fileStream.Close();

            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (currentController.PresentedViewController != null)
                currentController = currentController.PresentedViewController;

            UIView currentView = currentController.View;
            QLPreviewController qLPreview = new QLPreviewController();
            QLPreviewItem item = new QLPreviewItemBundle(fileName, filePath);
            qLPreview.DataSource = new PreviewControllerDS(item);
            currentController.PresentViewController(qLPreview, true, null);
        }

    }
    public class PreviewControllerDS : QLPreviewControllerDataSource
    {
        private QLPreviewItem _item;
        public PreviewControllerDS(QLPreviewItem item)
        {
            _item = item;
        }


        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return 1;
        }

        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return _item;
        }
    }
    public class QLPreviewItemBundle : QLPreviewItem
    {
        private string _fileName, _filePath;
        public QLPreviewItemBundle(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        public override string PreviewItemTitle => _fileName;
        public override NSUrl PreviewItemUrl
        {
            get
            {
                var documents = NSBundle.MainBundle.BundlePath;
                var lib = Path.Combine(documents, _filePath);
                var url = NSUrl.FromFilename(lib);
                return url;
            }
        }
    }

    public class QLPreviewItemFileSystem : QLPreviewItem
    {
        string _fileName, _filePath;
        public QLPreviewItemFileSystem(string fileName, string filePath)
        {
            _fileName = fileName;
            _filePath = filePath;
        }

        public override string PreviewItemTitle => _fileName;

        public override NSUrl PreviewItemUrl => NSUrl.FromString(_filePath);

    }
}

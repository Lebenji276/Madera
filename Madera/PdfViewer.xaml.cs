using GemBox.Pdf;
using System.Windows;
using System.Windows.Xps.Packaging;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Syncfusion.Pdf.Grid;
using System.Data;
using System.IO;
using Madera.Classe;

namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Window
    {
        private XpsDocument xpsDocument;

        public PdfViewer(Devis devis, Client client)
        {
            InitializeComponent();
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            GeneratePdf(devis, client);

            string currentDir = Directory.GetCurrentDirectory();
            string[] currentDirSplitted = currentDir.Split("\\bin");
            var filename = devis.nomProjet + devis.dateDevis.Trim().Replace("/", "_").Replace(":", "");
            var fullPath = currentDirSplitted[0] + "\\pdf\\" + filename + ".pdf";

            using (var document = GemBox.Pdf.PdfDocument.Load(fullPath))
            {
                this.xpsDocument = document.ConvertToXpsDocument(GemBox.Pdf.SaveOptions.Xps);
                this.DocumentViewer.Document = this.xpsDocument.GetFixedDocumentSequence();
            }
            
            this.Show();
        }

        public void GeneratePdf(Devis devis, Client client)
        {
            using (Syncfusion.Pdf.PdfDocument document = new Syncfusion.Pdf.PdfDocument())
            {
                //Add a page to the document
                Syncfusion.Pdf.PdfPage page = document.Pages.Add();

                //Create PDF graphics for a page
                PdfGraphics graphics = page.Graphics;
                //Set the standard font
                PdfFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 40, (PdfFontStyle)1);
                PdfFont fontTitleSecond = new PdfStandardFont(PdfFontFamily.Helvetica, 16, (PdfFontStyle)1);
                PdfFont fontText = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

                //Draw the text
                graphics.DrawString("Devis", fontTitle, PdfBrushes.Black, new PointF(350, 20));
                PdfBitmap image = new PdfBitmap("../../../madera.png");
                graphics.DrawImage(image, 0, 0);
                graphics.DrawString("80 avenue Edmund Halley", fontText, PdfBrushes.Black, new PointF(0, 60));
                graphics.DrawString("76800 Saint-Etienne-du-Rouvray", fontText, PdfBrushes.Black, new PointF(0, 80));
                graphics.DrawString("02 32 81 85 60", fontText, PdfBrushes.Black, new PointF(0, 100));

                graphics.DrawString("Reference : " + devis.referenceProjet, fontText, PdfBrushes.Black, new PointF(0, 200));
                graphics.DrawString("Date : " + devis.dateDevis, fontText, PdfBrushes.Black, new PointF(0, 220));
                graphics.DrawString("Numeros Client : " + client.phone, fontText, PdfBrushes.Black, new PointF(0, 240));

                graphics.DrawString(client.first_name, fontTitleSecond, PdfBrushes.Black, new PointF(330, 130));
                graphics.DrawString("Adresse : " + client.address, fontText, PdfBrushes.Black, new PointF(330, 160));
                graphics.DrawString("CP Ville : " + client.postal_code + ", "+ client.city, fontText, PdfBrushes.Black, new PointF(330, 180));

                graphics.DrawString("Intitule : " + devis.nomProjet, fontText, PdfBrushes.Black, new PointF(0, 300));

                //Create a PdfGrid.
                PdfGrid pdfGrid = new PdfGrid();
                //Create a DataTable.
                DataTable dataTable = new DataTable();
                //Add columns to the DataTable
                dataTable.Columns.Add("Quantite");
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Prix Unitaire HT");
                dataTable.Columns.Add("Prix Total HT");

                var modules = Module.getModulesByDevis(devis.modules);

                if (modules.Length == 0)
                {
                    dataTable.Rows.Add("",
                        "",
                        "",
                        "");
                }

                foreach (var module in modules)
                {
                    dataTable.Rows.Add("1", 
                        module.nomModule + (module.nomGamme != "" ? " / " + module.nomGamme : ""), 
                        "100.54 euros", 
                        "402.16 euros");
                }

                //Assign data source.
                pdfGrid.DataSource = dataTable;
                //Draw grid to the page of PDF document.
                pdfGrid.Draw(page, new PointF(0, 340));

                PdfGrid pdfGrid2 = new PdfGrid();
                //Create a DataTable.
                DataTable dataTable2 = new DataTable();

                dataTable2.Columns.Add("Total Hors Taxe");
                dataTable2.Columns.Add("TVA a 20%");
                dataTable2.Columns.Add("Total TTC");

                dataTable2.Rows.Add("1206.48 euros", "241.296 euros", "1 447.776");

                pdfGrid2.DataSource = dataTable2;
                int startDraw = dataTable.Rows.Count * 22 + 340;
                pdfGrid2.Draw(page, new PointF(300, startDraw));

                graphics.DrawString("Nous restons a votre disposition pour toute information complementaire.", fontText, PdfBrushes.Black, new PointF(0, startDraw + 80));
                graphics.DrawString("Si ce devis vous convient, veuillez nous le retourner signe precede de la mention :\n\"BON POUR COMMANDE ET EXECUTION DEVIS\"", fontText, PdfBrushes.Black, new PointF(0, startDraw + 100));

                graphics.DrawString("Date :", fontText, PdfBrushes.Black, new PointF(0, startDraw + 140));
                graphics.DrawString("Signature :", fontText, PdfBrushes.Black, new PointF(330, startDraw + 140));

                //Save the document
                string currentDir = Directory.GetCurrentDirectory();
                string[] currentDirSplitted = currentDir.Split("\\bin");
                var filename = devis.nomProjet + devis.dateDevis.Trim().Replace("/", "_").Replace(":", "");
                var fullPath = currentDirSplitted[0] + "\\pdf\\" + filename + ".pdf";

                document.Save(fullPath);
            }
        }
    }
}

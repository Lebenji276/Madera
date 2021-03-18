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

namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Window
    {
        private XpsDocument xpsDocument;

        public PdfViewer()
        {
            InitializeComponent();
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            GeneratePdf();
            using (var document = GemBox.Pdf.PdfDocument.Load("D:\\Scolarité\\Cesi\\Projets\\Livrable_3\\Client_Lourd\\Madera\\pdf\\Output.pdf"))
            {
                this.xpsDocument = document.ConvertToXpsDocument(GemBox.Pdf.SaveOptions.Xps);
                this.DocumentViewer.Document = this.xpsDocument.GetFixedDocumentSequence();
            }
        }

        public void GeneratePdf()
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
                graphics.DrawString("Adresse", fontText, PdfBrushes.Black, new PointF(0, 60));
                graphics.DrawString("CP Ville", fontText, PdfBrushes.Black, new PointF(0, 80));
                graphics.DrawString("Telephone", fontText, PdfBrushes.Black, new PointF(0, 100));

                graphics.DrawString("Reference :", fontText, PdfBrushes.Black, new PointF(0, 200));
                graphics.DrawString("Date :", fontText, PdfBrushes.Black, new PointF(0, 220));
                graphics.DrawString("Numeros Client :", fontText, PdfBrushes.Black, new PointF(0, 240));

                graphics.DrawString("Nom Client", fontTitleSecond, PdfBrushes.Black, new PointF(330, 130));
                graphics.DrawString("Adresse :", fontText, PdfBrushes.Black, new PointF(330, 160));
                graphics.DrawString("CP Ville :", fontText, PdfBrushes.Black, new PointF(330, 180));

                graphics.DrawString("Intitule : Description du projet et/ou Produit facture", fontText, PdfBrushes.Black, new PointF(0, 300));

                //Create a PdfGrid.
                PdfGrid pdfGrid = new PdfGrid();
                //Create a DataTable.
                DataTable dataTable = new DataTable();
                //Add columns to the DataTable
                dataTable.Columns.Add("Quantite");
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Prix Unitaire HT");
                dataTable.Columns.Add("Prix Total HT");

                dataTable.Rows.Add("4", "Mur en Bois", "100.54 euros" ,"402.16 euros" );
                dataTable.Rows.Add("4", "Mur en Bois", "100.54 euros", "402.16 euros");
                dataTable.Rows.Add("4", "Mur en Bois", "100.54 euros", "402.16 euros");
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
                document.Save("D:\\Scolarité\\Cesi\\Projets\\Livrable_3\\Client_Lourd\\Madera\\pdf\\Output.pdf");
            }
        }
    }
}

using Tabula.Extractors;
using Tabula;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
//using Spectre.Console;

namespace Pdf_to_Anki_cards;

internal class PdfReader
{
    public List<Table> ReadPdfTables(string path)
    {
        
        string pdfPath = @"C:\Users\itscr\source\repos\PDF-to-Anki-cards\Pdf-to-Anki-cards\Pdf-to-Anki-cards\PDFs\058.pdf";

        //string csvPath = @"/CSVoutput/output.csv";
        //"C:\Users\itscr\source\repos\PDF-to-Anki-cards\Pdf-to-Anki-cards\Pdf-to-Anki-cards\058.pdf"

        using (PdfDocument document = PdfDocument.Open(pdfPath, new ParsingOptions() { ClipPaths = true }))
        {
            ObjectExtractor oe = new ObjectExtractor(document);
            List<Table> tables = new List<Table>();
            var pageCount = document.NumberOfPages;

            for (int i = 1; i < pageCount; i++)
            {
                PageArea page = oe.Extract(i);

                IExtractionAlgorithm ea = new SpreadsheetExtractionAlgorithm();
                tables.AddRange(ea.Extract(page));
            }

            // Only return tables if there are any in the list
            if (tables.Count > 0)
            {
                return tables;
            }
            else
            {
                return null;
            }
        }
    }

}

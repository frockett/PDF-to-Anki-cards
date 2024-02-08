using Tabula.Extractors;
using Tabula;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Text;
//using Spectre.Console;

namespace Pdf_to_Anki_cards;

internal class PdfReader
{
    public List<Table>? ReadPdfTables(string path)
    {
        using (PdfDocument document = PdfDocument.Open(path, new ParsingOptions() { ClipPaths = true }))
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

            //SanitizeTables(tables);

            // Only return tables if there are any in the list
            return tables.Count > 0 ? tables : null;
        }
    }

    private void SanitizeTables(List<Table> tables)
    {
        foreach (var table in tables)
        {
            foreach (var row in table.Rows)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    // Assuming 'Text' is a property that holds the cell's text.
                    // You'll need to adjust this based on the actual data structure.
                    //row[i].Text = SanitizeText(row[i].Text);
                    row[i].SetTextElements(SanitizeText(row[i]));
                    
                }
            }
        }
    }

    private List<TextChunk> SanitizeText(Cell cell)
    {
        List<TextChunk> textChunks = new List<TextChunk>();
        foreach(var chunk in textChunks)
        {
            if(chunk.GetText() == "'")
            {
                //textChunks.IndexOf(chunk) = "''";
            }
        }

        throw new NotImplementedException();
    }
}

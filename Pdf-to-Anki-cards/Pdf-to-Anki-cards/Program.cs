using UglyToad.PdfPig;


namespace Pdf_to_Anki_cards;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        string pdfPath = @"C:\Users\itscr\source\repos\PDF-to-Anki-cards\Pdf-to-Anki-cards\Pdf-to-Anki-cards\PDFs\058.pdf";
        //string csvPath = @"/CSVoutput/output.csv";
        //"C:\Users\itscr\source\repos\PDF-to-Anki-cards\Pdf-to-Anki-cards\Pdf-to-Anki-cards\058.pdf"


        
        using var document = PdfDocument.Open(pdfPath);
        //using var writer = new StreamWriter(csvPath, false);

        foreach (var page in document.GetPages())
        {
            var chineseColumnMaxX = 150;
            var pinyinColumnMaxX = 250;

            var tableStartYFirstPage = 650;
            var tableStartY = 770;

            var tableEndYLastPage = 430;
            var tableEndY = 80;

            foreach (var word in page.GetWords())
            {
                var text = word.Text;
                var position = word.Letters.First().Location;

                string category;
                if (position.X < chineseColumnMaxX)
                {
                    category = "Chinese";
                }
                else if (position.X < pinyinColumnMaxX)
                {
                    category = "Pinyin";
                }
                else
                {
                    category = "English";
                }

                Console.WriteLine($"{category}: {text} || Location: {position.X}, {position.Y}");

            }
        }
    }
}

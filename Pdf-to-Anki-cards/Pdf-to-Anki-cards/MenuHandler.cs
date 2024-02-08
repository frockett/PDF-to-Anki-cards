using Spectre.Console;
using Tabula;

namespace Pdf_to_Anki_cards;

internal class MenuHandler
{
    
    private readonly PdfReader reader;
    private readonly NoteBuilder noteBuilder;
    internal MenuHandler(PdfReader reader, NoteBuilder noteBuilder) 
    { 
        this.reader = reader;
        this.noteBuilder = noteBuilder;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public async Task ShowMainMenu()
    {
        List<Tabula.Table>? tables = new();

        // Get the file path
        string pdfPath = AnsiConsole.Ask<string>("Paste full path to PDF: ");
        string cleanPdfPath = pdfPath.Trim('"');


        if (File.Exists(cleanPdfPath))
        {
            // File exists, proceed with processing
            tables = reader.ReadPdfTables(cleanPdfPath);
        }
        else
        {
            // File does not exist, warn the user
            Console.WriteLine("File does not exist. Please check the file path and try again.");
            return;
        }



        // Call for the document to be read
        //tables = reader.ReadPdfTables(pdfPath);

        if (tables == null || tables.Count == 0)
        {
            AnsiConsole.Markup("Either your document has no tables or there was an error parsing them. Try a different document or algorithm.");
            return;
        }

        // Get the number of columns in the first table
        int columnCount = tables.FirstOrDefault().ColumnCount;


        
        // Cache cells as variable
        var rows = tables.FirstOrDefault().Rows;
        var cells = tables.FirstOrDefault().Cells;
        


        // Display contents of first row so that user can then label them for deck creation.
        AnsiConsole.WriteLine($"There are {columnCount} columns in this table.");

        for ( int i = 0; i < columnCount; i++ )
        {
            AnsiConsole.WriteLine($"Column {i + 1}: {cells.ElementAt(i)}");
        }

        // Get user input for deck name, then generate note types and deckId
        string deckName = AnsiConsole.Ask<string>("What would you like to name your deck? ");
        char[] invalidChars = Path.GetInvalidFileNameChars();

        while (deckName.Any(c => invalidChars.Contains(c)))
        {
            // Filter out unprintable characters and put them in a new char array, then join them with spaces in-between
            char[] printableChars = invalidChars.Where(c => char.IsControl(c) == false).ToArray();
            string displayString = string.Join(" ", printableChars);

            AnsiConsole.WriteLine($"The filename contains invalid characters such as {displayString}");
            deckName = AnsiConsole.Ask<string>($"What would you like to name your deck? ");
        }

        string deckNameWithExtension = Path.ChangeExtension(deckName, ".apkg");
        long noteTypeId = noteBuilder.GenerateNoteTypes();
        long deckId = noteBuilder.GenerateDeck(deckName);

        // Check if error code was returned
        if (deckId == -1)
        {
            AnsiConsole.MarkupLine("An error occurred and the deck was not created successfully. Restart and try again.");
            return;
        }
        
        foreach (var row in rows)
        {
            Cell[] currentRow = row.ToArray();
            string[] cellStrings = currentRow.Select(cell => cell.GetText()).ToArray();
            noteBuilder.GenerateNotes(deckId, noteTypeId, cellStrings);
        }

        await noteBuilder.WriteCollection(deckNameWithExtension);
    }
}

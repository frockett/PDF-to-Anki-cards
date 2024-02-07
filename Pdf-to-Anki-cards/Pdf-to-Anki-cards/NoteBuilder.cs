using AnkiNet;
using System.IO.Compression;

namespace Pdf_to_Anki_cards;

internal class NoteBuilder
{
    private readonly AnkiCollection ocollection;
    private AnkiNoteType noteType;
    AnkiCollection collection = new AnkiCollection();
    public NoteBuilder(AnkiCollection collection) 
    { 
        ocollection = collection;
    }

    public void GenerateNotes(long deckId, long noteTypeId, string[] fields)
    {
        collection.CreateNote(deckId, noteTypeId, "TestCN", "TestPy","TestEn");
    }
    public async Task WriteCollection()
    {
        collection.TryGetDeckByName("test", out var deck);
        //List<AnkiNote> notes = deck.Cards.ToList();
        //foreach (var note in notes)
        //{
        //    Console.WriteLine(note.Fields.ToString());
       // }
        //Console.WriteLine(deck.Cards.ToString());
        await AnkiFileWriter.WriteToFileAsync("Test1.apkg", collection);
    }

    public long GenerateNoteTypes()
    {
        AnkiCardType[] cardTypes = GenerateCardTypes();

        noteType = new AnkiNoteType(
            "Basic",
            cardTypes,
            new[] { "Chinese", "Pinyin", "English" }
        );

        var noteTypeId = collection.CreateNoteType(noteType);
        return noteTypeId;
    }

    private AnkiCardType[] GenerateCardTypes()
    {
        AnkiCardType[] cardTypes = new[]
        {
            new AnkiCardType(
                "Forwards",
                0,
                "{{Chinese}}<br/>{{Pinyin}}",
                "{{English}}"
            ),
            new AnkiCardType(
                "Backwards",
                1,
                "{{English}}",
                "{{Chinese}}<br/>{{Pinyin}}"
            )
        };

        return cardTypes;
    }

    public long GenerateDeck(string deckName = "test")
    {
        var deckId = collection.CreateDeck(deckName);

        bool deckExists = collection.TryGetDeckById(deckId, out var deck);

        if (deckExists)
        {
            return deckId;
        }
        else
        {
            return -1;
        }
    }
    
}

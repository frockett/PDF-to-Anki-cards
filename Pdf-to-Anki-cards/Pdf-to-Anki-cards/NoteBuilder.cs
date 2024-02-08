using AnkiNet;
using System.IO.Compression;

namespace Pdf_to_Anki_cards;

internal class NoteBuilder
{
    private readonly AnkiCollection collection;
    private AnkiNoteType noteType;
    //AnkiCollection collection = new AnkiCollection();
    public NoteBuilder(AnkiCollection collection) 
    {
        this.collection = collection;
    }

    public void GenerateNotes(long deckId, long noteTypeId, string[] fields)
    {
        collection.CreateNote(deckId, noteTypeId, fields);
    }

    public async Task WriteCollection()
    {
        collection.TryGetDeckByName("test", out var deck);
      
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

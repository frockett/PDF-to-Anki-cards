
using AnkiNet;
using Pdf_to_Anki_cards;

var collection = new AnkiCollection();
NoteBuilder noteBuilder = new NoteBuilder(collection);
PdfReader reader = new PdfReader();
MenuHandler menuHandler = new MenuHandler(reader, noteBuilder);

menuHandler.ShowMainMenu();
noteBuilder.WriteCollection();
﻿
using AnkiNet;
using Pdf_to_Anki_cards;

var collection = new AnkiCollection();
NoteBuilder noteBuilder = new NoteBuilder(collection);
PdfReader reader = new PdfReader();
MenuHandler menuHandler = new MenuHandler(reader, noteBuilder);

await menuHandler.ShowMainMenu();
//noteBuilder.WriteCollection();
//await noteBuilder.TestMethod();
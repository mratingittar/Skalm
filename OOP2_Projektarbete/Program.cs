﻿// See https://aka.ms/new-console-template for more information

using Skalm;

Console.Title = Globals.G_GAME_TITLE;
Console.CursorVisible = Globals.G_DISPLAY_CURSOR;

GameManager game = new GameManager();
game.Start();
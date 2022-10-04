// See https://aka.ms/new-console-template for more information

using OOP2_Projektarbete;

Console.Title = Globals.G_GAME_TITLE;
Console.CursorVisible = Globals.G_DISPLAY_CURSOR;

GameManager game = new GameManager();
game.DisplayManager.HudMsgBox.DisplayMessage("1+ damage taken");
Console.ReadKey();
game.DisplayManager.HudMsgBox.DisplayMessage("");
Console.ReadKey();
game.DisplayManager.HudMsgBox.DisplayMessage("You are overcome by a sense of doom... \nAll is lost, for reasons completely unknown and impossible to stop. \nYou did absolutely nothing to avoid disaster... And frankly, pouting has always been the way of the great scholars of all time.");
Console.ReadKey();
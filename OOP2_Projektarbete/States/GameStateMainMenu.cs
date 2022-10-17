﻿using Skalm.Animation;
using Skalm.Input;
using Skalm.Menu;
using Skalm.Structs;

namespace Skalm.States
{
    internal class GameStateMainMenu : GameStateBase
    {
        private Animator fireAnimator;
        private bool everyOtherFrame = true;

        // CONSTRUCTOR I
        public GameStateMainMenu(GameManager gameManager) : base(gameManager)
        {
            fireAnimator = gameManager.FireAnimator;
        }

        #region StateBasics

        // ENTER STATE
        public override void Enter()
        {
            menuManager.LoadMenu(menuManager.mainMenu);
            soundManager.PlayMusic(soundManager.Tracks.Find(song => song.soundName == "Video Dungeon Crawl"));

            // INPUT EVENT SUBSCRIBE
            inputManager.OnInputMove += MoveInput;
            inputManager.OnInputCommand += CommandInput;

            // MENU EVENT SUBSCRIBE
            menuManager.mainMenu.onMenuExecution += MenuExecution;
        }

        // EXIT STATE
        public override void Exit()
        {
            menuManager.UnloadMenu();

            // INPUT EVENT UNSUBSCRIBE
            inputManager.OnInputMove -= MoveInput;
            inputManager.OnInputCommand -= CommandInput;

            // MENU EVENT UNSUBSCRIBE
            menuManager.mainMenu.onMenuExecution -= MenuExecution;
        }

        // UPDATE LOGIC
        public override void UpdateLogic()
        {

        }

        // UPDATE DISPLAY
        public override void UpdateDisplay()
        {
            if (everyOtherFrame)
            {
                fireAnimator.AnimatedBraziers();
                everyOtherFrame = false;
            }
            else
                everyOtherFrame = true;
        }

        #endregion

        #region Methods

        // METHOD MOVE INPUT
        private void MoveInput(Vector2Int direction)
        {
            menuManager.TraverseMenu(direction);
        }

        // METHOD COMMAND INPUT
        private void CommandInput(InputCommands command)
        {
            menuManager.ExecuteMenu(command);
        }

        // METHOD EXECUTE MENU INDEX
        private void MenuExecution(Page menuPage, string item)
        {
            switch (menuPage)
            {
                case Page.MainMenu:
                    if (item == "Exit")
                        Environment.Exit(0);
                    break;

                case Page.NewGame:
                    if (item == "Enter Name")
                    {
                        EraseRow(menuManager.ActiveMenu.PageStartRow + 5);
                        (bool nameOK, string nameReturned) = EnterName(menuManager.ActiveMenu.PageStartRow + 5);
                        Console.CursorVisible = false;
                        if (nameOK)
                            gameManager.PlayerName = nameReturned;
                        else
                            EraseRow(Console.CursorTop);
                    }

                    if (item == "Start New Game")
                    {
                        gameManager.NewGame = true;
                        gameManager.stateMachine.ChangeState(GameStates.GameStatePlaying);
                    }
                    break;

                case Page.Options:
                    if (item == "Toggle Beep")
                        soundManager.player.SFXEnabled = !soundManager.player.SFXEnabled;
                    break;

                case Page.Music:
                    soundManager.PlayMusic(soundManager.Tracks.Find(sound => sound.soundName == item));
                    menuManager.ActiveMenu.ReloadPage();
                    break;

                case Page.InputMethod:
                    inputManager.SetInputMethod(inputManager.Inputs.Find(input => input.GetType().Name == item)!);
                    break;
            }
        }

        // METHOD NAME ENTERING
        private (bool, string) EnterName(int height)
        {
            Console.SetCursorPosition(displayManager.windowInfo.WindowWidth / 2, height);
            Console.CursorVisible = true;
            ConsoleKeyInfo cki;
            string name = "";
            while (true)
            {
                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Enter)
                    break;

                else if (cki.Key == ConsoleKey.Escape)
                    return (false, name);

                else if (cki.Key == ConsoleKey.Backspace && name.Length > 0)
                {
                    name = name.Substring(0, name.Length - 1);
                    EraseRow(height);
                }

                else
                    name += cki.KeyChar;

                displayManager.printer.PrintCenteredInWindow(name, height);
            }

            if (name.Length == 0)
                return (false, name);
            else
                return (true, name);
        }

        private void EraseRow(int row)
        {
            displayManager.eraser.EraseLinesFromTo(row, row);
        }
        #endregion
    }
}

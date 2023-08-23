using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace BattleshipGame.API.Services
{
    public class MessageService : IMessageService
    {
        public string PlayerNotFoundMessage()
        {
            string message = "Sorry! This user doesn't exist. Check the id or username you've entered and try again.";

            return message;
        }

        public string UserCreatingError()
        {
            string message = "Sorry! Error occurred when creating a new player. Check if you've passed all required data." +
                " Make sure this name is available. You can try with another one.";

            return message;
        }

        public string Delete()
        {
            string message = "Player successfully deleted.";

            return message;
        }

        public string StartGameError()
        {
            string message = "Sorry! Game can't be started because of wrong data. Insert 1 as a key and your player name" +
                " to start the game.";

            return message;
        }

        public string StartGameSuccess(string opponent)
        {
            string message = ($"Game created successfully! Your opponent is {opponent}");

            return message;
        }

        public string GameBoardNotFound()
        {
            string message = "Sorry! Game board for this player name wasn't found. Make sure you've started the game" +
                " and passed correct player name.";

            return message;
        }

        public string ShootError(string opponent, string key)
        {
            string message = "";

            if (key == "0")
            {
                message = "Sorry! Something gone wrong. Check if you've passed valid player name and coordinates and" +
                    " try again.";
            }
            else if (key == "1")
            {
                message = $"You've taken your shoot. Now it's {opponent} turn.";
            }
            else if (key == "2")
            {
                message = "Sorry! You can pass maximum 5 pair of coordinates. Make sure your format is correct and try again.";
            }

            return message;
        }

        public string ShotSuccess(int quantity, List<string> hitShipsCoords)
        {
            string message = "";

            if (hitShipsCoords.Count > 1)
            {
                message = $"Shoot successful! You've hit {quantity} field(s). Let's take another shoot! Hit ships coordinates" +
                    $" are displayed below.";
            }

            if (hitShipsCoords.Count == 1)
            {
                message = $"Shoot successful! You've hit {quantity} field(s). Hit ship was at (X,Y): " +
                    $"({hitShipsCoords[0]}). Let's take another shoot!";
            }

            return message;
        }

        public string ShotMissed()
        {
            string message = $"You didn't hit any enemy ships, try again in the next round!";

            return message;
        }
    }
}

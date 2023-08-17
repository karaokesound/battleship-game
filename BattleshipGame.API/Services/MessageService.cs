namespace BattleshipGame.API.Services
{
    public class MessageService : IMessageService
    {
        public string PlayerNotFoundMessage()
        {
            string message = "Sorry! This user doesn't exist. Check the id you've entered and try again.";

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

        public string GameBoardNotFound()
        {
            string message = "Sorry! Game board for this player name wasn't found. Check if you've passed correct player" +
                " and try again.";

            return message;
        }

    }
}

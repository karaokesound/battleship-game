namespace BattleshipGame.API.Services
{
    public interface IMessageService
    {
        string PlayerNotFoundMessage();

        string UserCreatingError();

        string Delete();
    }
}

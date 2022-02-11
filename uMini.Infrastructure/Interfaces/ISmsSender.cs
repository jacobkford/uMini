namespace uMini.Infrastructure.Interfaces;

public interface ISmsSender
{
    Task SendSmsAsync(string number, string message);
}
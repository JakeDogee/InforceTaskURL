namespace InforceTask.Application.Interfaces;

public interface IUrlShorteningService
{
    string ShortenUrl(string originalUrl);
}
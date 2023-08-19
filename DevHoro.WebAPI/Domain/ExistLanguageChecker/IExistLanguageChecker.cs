namespace DevHoro.WebAPI.Domain.ExistLanguageChecker;

public interface IExistLanguageChecker
{
    Task<bool> Check(string language);  
}
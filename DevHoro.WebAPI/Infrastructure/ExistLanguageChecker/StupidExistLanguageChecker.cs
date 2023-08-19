using DevHoro.WebAPI.Domain.ExistLanguageChecker;

namespace DevHoro.WebAPI.Infrastructure.ExistLanguageChecker;

public class StupidExistLanguageChecker : IExistLanguageChecker
{
    private readonly IEnumerable<string> _languages = new List<string>()
    {
        "python",
        "c",
        "c++",
        "java",
        "c#",
        "javascript",
        "visual",
        "sql",
        "assembly",
        "php",
        "scratch",
        "go",
        "matlab",
        "fortran",
        "cobol",
        "r",
        "ruby",
        "swift",
        "rust",
        "julia",
        "sas",
        "classic",
        "delphi",
        "ada",
        "prolog",
        "visual basic",
        "kotlin",
        "perl",
        "objective",
        "lisp",
        "scala",
        "haskell",
        "d",
        "lua",
        "dart",
        "logo",
        "gams",
        "vbscript",
        "scheme",
        "transact",
        "cfml",
        "pl",
        "abap",
        "solidity",
        "typescript",
        "f",
        "powershell",
        "forth",
        "bash",
        "x++"
    };

    public async Task<bool> Check(string language)
        => await Task.FromResult(_languages.Contains(language.ToLower()));
}
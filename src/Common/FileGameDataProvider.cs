using System.IO;
using System.Threading.Tasks;

namespace QuestViva.Common;

public class FileGameDataProvider(string filename): IGameDataProvider
{
    public Task<GameData?> GetData()
    {
        var stream = File.OpenRead(filename);
        var gameId = Path.GetFileName(filename);
        return Task.FromResult<GameData?>(new GameData(stream, gameId, filename, this));
    }

    public virtual Stream? GetAdjacentFile(string adjacentFilename) => null;
}

public class FileDirectoryGameDataProvider(string filename): FileGameDataProvider(filename)
{
    private readonly string? _parentDirectory = Path.GetDirectoryName(filename);

    public override Stream? GetAdjacentFile(string adjacentFilename)
    {
        if (_parentDirectory == null)
        {
            return null;
        }
            
        var adjacentFilePath = Path.Combine(_parentDirectory, adjacentFilename);
        if (File.Exists(adjacentFilePath))
        {
            return File.OpenRead(adjacentFilePath);
        }
        return null;
    }
}
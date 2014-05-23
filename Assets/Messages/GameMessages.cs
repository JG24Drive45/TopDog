

public class LoadLevelMessage
{
    public string LevelToLoad;

    public LoadLevelMessage(string level)
    {
        LevelToLoad = level;
    }


}


public class DestroyLevelMessage
{
    public string LevelToLoad;

    public DestroyLevelMessage(string level)
    {
        LevelToLoad = level;
    }
}

public class LevelIsDestroyedMessage
{
}
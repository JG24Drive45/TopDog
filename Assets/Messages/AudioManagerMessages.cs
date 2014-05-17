
public class PauseSoundMessage
{
    public string SoundName { get; set; }
    public PauseSoundMessage(string soundName)
    {
        SoundName = soundName;
    }
}

public class StopSoundMessage
{
    public string SoundName { get; set; }
    public StopSoundMessage(string soundName)
    {
        SoundName = soundName;
    }
}

public class ResumeSoundMessage
{
    public string SoundName { get; set; }
    public ResumeSoundMessage(string soundName)
    {
        SoundName = soundName;
    }
}

public class PlaySoundMessage
{
    public string SoundName { get; set; }
    public bool LoopAble { get; set; }
    public int FadeInDuration { get; set; }

    public PlaySoundMessage(string soundName, bool loop)
    {
        SoundName = soundName;
        LoopAble = loop;
    }

    public PlaySoundMessage(string soundName, bool loop, int duration)
    {
        SoundName = soundName;
        LoopAble = loop;
        FadeInDuration = duration;
    }
}

public class StopSoundLoopMessage
{
    public string SoundName { get; set; }
    public int FadeDuration { get; set; }

    public StopSoundLoopMessage(string soundName)
    {
        SoundName = soundName;
    }

    public StopSoundLoopMessage(string soundName, int duration)
    {
        SoundName = soundName;
        FadeDuration = duration;
    }
}


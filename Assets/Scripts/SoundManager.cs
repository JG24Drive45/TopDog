using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundEffects
{
    public AudioClip AudioClip { get; set; }
    public int Instances { get; set; }
    public float Volume { get; set; }

    public SoundEffects(AudioClip soundEffect, int instances)
    {
        AudioClip = soundEffect;
        Instances = instances;
        Volume = 1;
    }

    public SoundEffects(AudioClip soundEffect, int instances, float vol)
    {
        AudioClip = soundEffect;
        Instances = instances;
        Volume = vol;
    }
};

public class SoundEffectInstances
{
    public List<GameObject> SoundEffects { get; set; }
    public int Instances { get; set; }

    public SoundEffectInstances(int instances)
    {
        SoundEffects = new List<GameObject>();
        Instances = instances;
    }

    internal void PlaySound(AudioClip audioclip, float volume, float pitch, float pan)
    {
        if (SoundEffects.Count < Instances)
        {
            for (int i = 0; i < Instances; ++i)
            {
                GameObject apObject = new GameObject();
                apObject.transform.position = Vector3.zero;
                apObject.AddComponent<AudioSource>();
                AudioSource apAudio = apObject.GetComponent<AudioSource>();
                apAudio.playOnAwake = false;
                apAudio.rolloffMode = AudioRolloffMode.Linear;
                apAudio.loop = false;
                apAudio.clip = audioclip;
                apAudio.volume = volume;
                apAudio.pitch = pitch;
                apAudio.pan = pan;
                if (i == 0)
                    apAudio.Play();
                apObject.SetActive(true);
                SoundEffects.Add(apObject);
            }
        }
        else
        {
            for (int i = 0; i < SoundEffects.Count; ++i)
            {
                AudioSource temp = SoundEffects[i].GetComponent<AudioSource>();
                if (!temp.isPlaying)
                {
                    temp.Play();
                    break;
                }
            }
        }
    }

    internal void PlaySound(AudioClip audioclip, float volume, float pitch, float pan, bool looping)
    {
        if (SoundEffects.Count < Instances)
        {
            for (int i = 0; i < Instances; ++i)
            {
                GameObject apObject = new GameObject();
                apObject.transform.position = Vector3.zero;
                apObject.AddComponent<AudioSource>();
                AudioSource apAudio = apObject.GetComponent<AudioSource>();

                apAudio.playOnAwake = false;
                apAudio.rolloffMode = AudioRolloffMode.Linear;
                apAudio.loop = looping;
                apAudio.clip = audioclip;
                apAudio.volume = volume;
                apAudio.pitch = pitch;
                apAudio.pan = pan;
                if (i == 0)
                    apAudio.Play();
                apObject.SetActive(true);
                SoundEffects.Add(apObject);
            }
        }
        else
        {
            for (int i = 0; i < SoundEffects.Count; ++i)
            {
                AudioSource temp = SoundEffects[i].GetComponent<AudioSource>();
                if (!temp.isPlaying)
                {
                    temp.Play();
                    break;
                }
            }
        }
    }
};

public class LoopingSoundEfectInstances
{
    public SoundEffectInstances SoundEffectInstance { get; set; }
    public SoundManager.MusicFadeEffect MusicFadeEffect { get; set; }
}

public class SoundManager : MonoBehaviour,
    IHandle<StopSoundMessage>,
    IHandle<StopSoundLoopMessage>,
    IHandle<PlaySoundMessage>,
    IHandle<PauseSoundMessage>,
    IHandle<ResumeSoundMessage>
{
    public Dictionary<string, SoundEffects> m_Sounds = new Dictionary<string, SoundEffects>();
    private Dictionary<string, SoundEffectInstances> m_PlayingSounds = new Dictionary<string, SoundEffectInstances>();
    private Dictionary<string, LoopingSoundEfectInstances> m_PlayingSoundLoop = new Dictionary<string, LoopingSoundEfectInstances>();
    EventAggregator m_EventAggregator;
    private static float volume = 1.0f;
    public static float SoundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    void Start()
    {
        AudioListener.volume = 1.0f;
        GameEventAggregator.GameMessenger.Subscribe(this);
    }

    void Unload()
    {
        GameEventAggregator.GameMessenger.Unsubscribe(this);
        if (m_EventAggregator != null)
            m_EventAggregator.Unsubscribe(this);
    }

    public void LoadSound(string soundName, AudioClip clip)
    {
        if (m_Sounds.ContainsKey(soundName))
        {
            throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
        }
        m_Sounds.Add(soundName, new SoundEffects(clip, 1));
    }

    public void LoadSound(string soundName, AudioClip clip, float vol)
    {
        if (m_Sounds.ContainsKey(soundName))
        {
            throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
        }
        m_Sounds.Add(soundName, new SoundEffects(clip, 1, vol));
    }

    public void LoadSound(string soundName, AudioClip clip, int instances)
    {
        if (m_Sounds.ContainsKey(soundName))
        {
            throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
        }

        m_Sounds.Add(soundName, new SoundEffects(clip, instances));
    }

    public void LoadSound(string soundName, string soundPath)
    {
        if (m_Sounds.ContainsKey(soundName))
        {
            throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
        }
        m_Sounds.Add(soundName, new SoundEffects(Resources.Load<AudioClip>(soundPath), 1));
    }

    public void LoadSound(string soundName, string soundPath, float vol)
    {
        if (m_Sounds.ContainsKey(soundName))
        {
            throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
        }

        m_Sounds.Add(soundName, new SoundEffects(Resources.Load<AudioClip>(soundPath), 1, vol));
    }

    public void LoadSound(string soundName, string soundPath, int instances)
    {
        if (m_Sounds.ContainsKey(soundName))
        {
            throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
        }

        m_Sounds.Add(soundName, new SoundEffects(Resources.Load<AudioClip>(soundPath), instances));
    }

    public void PlaySound(string soundName, float volume, float pitch, float pan)
    {
        SoundEffects sound;
        if (!m_Sounds.TryGetValue(soundName, out sound))
        {
            Debug.Log("could not find sound name: " + soundName);
        }
        else
        {
            if (!m_PlayingSounds.ContainsKey(soundName))
            {
                m_PlayingSounds[soundName] = new SoundEffectInstances(sound.Instances);
                m_PlayingSounds[soundName].PlaySound(sound.AudioClip, sound.Volume, pitch, pan);
            }
            else
            {
                m_PlayingSounds[soundName].Instances = sound.Instances;
                m_PlayingSounds[soundName].PlaySound(sound.AudioClip, sound.Volume, pitch, pan);
            }
        }
    }

    public void PlaySound(string soundName)
    {
        PlaySound(soundName, 1.0f, 1.0f, 0.0f);
    }

    public void PlaySound(string soundName, float volume)
    {
        PlaySound(soundName, volume, 0.0f, 0.0f);
    }

    public void PlaySoundLooped(string soundName, int durations)
    {
        SoundEffects sound;


        if (m_Sounds.TryGetValue(soundName, out sound))
        {
            if (m_PlayingSoundLoop.ContainsKey(soundName))
            {
                m_PlayingSoundLoop[soundName].SoundEffectInstance.PlaySound(sound.AudioClip, sound.Volume, 1.0f, 0.0f, true);
                m_PlayingSoundLoop[soundName].MusicFadeEffect = new MusicFadeEffect(sound.Volume, 1, durations);
            }
            else
            {
                m_PlayingSoundLoop[soundName] = new LoopingSoundEfectInstances();
                m_PlayingSoundLoop[soundName].SoundEffectInstance = new SoundEffectInstances(sound.Instances);
                m_PlayingSoundLoop[soundName].SoundEffectInstance.PlaySound(sound.AudioClip, sound.Volume, 1.0f, 0.0f, true);
                m_PlayingSoundLoop[soundName].MusicFadeEffect = new MusicFadeEffect(sound.Volume, 1, durations);
            }
        }
    }

    public void PlaySoundLooped(string soundName)
    {

        SoundEffects sound;

        if (m_Sounds.TryGetValue(soundName, out sound))
        {
            if (m_PlayingSoundLoop.ContainsKey(soundName))
            {
                m_PlayingSoundLoop[soundName].SoundEffectInstance.PlaySound(sound.AudioClip, sound.Volume, 1.0f, 0.0f, true);
            }
            else
            {
                m_PlayingSoundLoop[soundName] = new LoopingSoundEfectInstances();
                m_PlayingSoundLoop[soundName].SoundEffectInstance = new SoundEffectInstances(sound.Instances);
                m_PlayingSoundLoop[soundName].SoundEffectInstance.PlaySound(sound.AudioClip, sound.Volume, 1.0f, 0.0f, true);
            }
        }

    }

    public void StopSoundLoop(string soundName)
    {
        if (m_PlayingSoundLoop.ContainsKey(soundName))
        {
            m_PlayingSoundLoop[soundName].SoundEffectInstance.SoundEffects[0].audio.Stop();
        }
    }

    public void StopSoundLoop(string soundName, int duration)
    {
        if (m_PlayingSoundLoop.ContainsKey(soundName))
        {
            m_PlayingSoundLoop[soundName].MusicFadeEffect = new MusicFadeEffect(m_PlayingSoundLoop[soundName].SoundEffectInstance.SoundEffects[0].audio.volume, 0, duration);
        }
    }

    public void StopSound(string soundName)
    {
        if (m_PlayingSounds.ContainsKey(soundName))
        {
            foreach (GameObject effect in m_PlayingSounds[soundName].SoundEffects)
            {
                effect.audio.Stop();                  
            }
        }
    }

    public class MusicFadeEffect
    {
        public float SourceVolume;
        public float TargetVolume;

        private float _time;
        private float _duration;

        public MusicFadeEffect(float sourceVolume, float targetVolume, float duration)
        {
            SourceVolume = sourceVolume;
            TargetVolume = targetVolume;
            _time = 0.0f;
            _duration = duration;
        }

        public bool Update(float time)
        {
            _time += time;

            if (_time >= _duration)
            {
                _time = _duration;
                return true;
            }

            return false;
        }

        public float GetVolume()
        {
            return Mathf.Lerp(SourceVolume, TargetVolume, _time / _duration);
        }
    }

    public void PauseSound(string soundName)
    {
        if (m_PlayingSounds.ContainsKey(soundName))
        {
            foreach (GameObject effect in m_PlayingSounds[soundName].SoundEffects)
            {
                effect.audio.Pause();
            }
        }
    }

    public void ResumeSound(string soundName)
    {
        if (m_PlayingSounds.ContainsKey(soundName))
        {
            foreach (GameObject effect in m_PlayingSounds[soundName].SoundEffects)
            {
                float time = effect.audio.time;
                effect.audio.Play();
                effect.audio.time = time;
            }
        }
    }

    public void StopAllSounds()
    {
        foreach (SoundEffectInstances instance in m_PlayingSounds.Values)
        {
            foreach (GameObject se in instance.SoundEffects)
            {
                se.audio.Stop();
            }
        }
    }

    public void Update()
    {
        List<string> removeLoopingInstances = new List<string>();
        foreach (KeyValuePair<String, LoopingSoundEfectInstances> keyPair in m_PlayingSoundLoop)
        {
            if (m_PlayingSoundLoop.Count > 1)
            {
                int x = 5;
            }

            if (keyPair.Value.MusicFadeEffect != null)
            {
                keyPair.Value.MusicFadeEffect.Update(Time.deltaTime);
                keyPair.Value.SoundEffectInstance.SoundEffects[0].audio.volume = keyPair.Value.MusicFadeEffect.GetVolume();

                if (keyPair.Value.SoundEffectInstance.SoundEffects[0].audio.volume <= 0)
                {
                    removeLoopingInstances.Add(keyPair.Key);
                }
                else if (keyPair.Value.MusicFadeEffect.TargetVolume == 1 && keyPair.Value.SoundEffectInstance.SoundEffects[0].audio.volume == 1)
                {
                    keyPair.Value.MusicFadeEffect = null;
                }
            }
        }

        for (int i = 0; i < removeLoopingInstances.Count; i++)
        {
            m_PlayingSoundLoop[removeLoopingInstances[i]].SoundEffectInstance.SoundEffects[0].audio.Stop();
        }

    }
    public void Handle(PlaySoundMessage message)
    {

        Debug.Log("PlaySound Message");
        if (message.LoopAble)
        {
            if (message.FadeInDuration > 0)
            {
                PlaySoundLooped(message.SoundName, message.FadeInDuration);
            }
            else
            {
                PlaySoundLooped(message.SoundName);
            }
        }
        else
        {
            PlaySound(message.SoundName);
        }
    }

    public void Handle(StopSoundMessage message)
    {
        StopSound(message.SoundName);
    }

    public void Handle(StopSoundLoopMessage message)
    {
        if (message.FadeDuration > 0)
        {
            StopSoundLoop(message.SoundName, message.FadeDuration);
        }
        else
        {
            StopSoundLoop(message.SoundName);
        }
    }

    public void Handle(PauseSoundMessage message)
    {
        PauseSound(message.SoundName);
    }
    public void Handle(ResumeSoundMessage message)
    {
        ResumeSound(message.SoundName);
    }
}

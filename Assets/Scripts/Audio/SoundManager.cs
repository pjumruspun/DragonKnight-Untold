using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    private static Dictionary<SFXName, SFX> soundCache = new Dictionary<SFXName, SFX>();

    public static void Play(SFXName name, bool critVariation = false)
    {
        if (name == SFXName.None)
        {
            return;
        }

        if (critVariation)
        {
            Play(SFXName.CriticalHit);
        }

        if (soundCache.TryGetValue(name, out SFX sound))
        {
            sound.Source.Play();
        }
        else
        {
            Debug.LogAssertion($"[ERROR] AudioManager.Play: soundCache missed: {name}");
        }
    }

    public static void MuteAll()
    {
        foreach (SFX sound in SoundRepository.AllSounds)
        {
            sound.Source.volume = 0;
        }
    }

    public static void UnmuteAll()
    {
        foreach (SFX sound in SoundRepository.AllSounds)
        {
            sound.Source.volume = sound.Volume;
        }
    }

    private void Start()
    {
        foreach (SFX sound in SoundRepository.AllSounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.loop = sound.Loop;
            soundCache[sound.Name] = sound;
        }

        // Play BGM?
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRepository : MonoSingleton<SoundRepository>
{
    public static IEnumerable<SFX> AllSounds => Instance.sounds;

    [SerializeField]
    private List<SFX> sounds = new List<SFX>();
}

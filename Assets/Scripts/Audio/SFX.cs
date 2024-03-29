using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SFX", menuName = "Roguelite/SFX")]
public class SFX : ScriptableObject
{
    public SFXName Name => name;
    public AudioClip Clip => clip;
    public float Volume => volume;
    public bool Loop => loop;

    public AudioSource Source { get; set; }

    [SerializeField]
    private SFXName name = default;

    [SerializeField]
    private AudioClip clip = default;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float volume = 1;

    [SerializeField]
    private bool loop = false;
}
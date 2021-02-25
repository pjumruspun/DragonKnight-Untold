using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigContainer : MonoSingleton<ConfigContainer>
{
    public PlayerConfig GetPlayerConfig => playerConfig;

    [SerializeField]
    private PlayerConfig playerConfig;
}

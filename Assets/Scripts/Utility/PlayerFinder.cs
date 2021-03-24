using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder
{
    private const string playerTag = "Player";

    public static GameObject FindByTag()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
        {
            throw new System.NullReferenceException($"Cannot find player's GameObject with tag '{playerTag}'");
        }

        return player;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder
{
    public static GameObject FindByTag()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.Player);
        if (player == null)
        {
            throw new System.NullReferenceException($"Cannot find player's GameObject with tag '{Tags.Player}'");
        }

        return player;
    }
}

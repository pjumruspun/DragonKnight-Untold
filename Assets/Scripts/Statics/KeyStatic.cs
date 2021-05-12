using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStatic
{
    public static int numberOfKeys = 0;
    public const int maxKeyCount = 5;

    public static bool AddKey()
    {
        if (numberOfKeys < maxKeyCount)
        {
            ++numberOfKeys;
            return true;
        }

        return false;
    }
}

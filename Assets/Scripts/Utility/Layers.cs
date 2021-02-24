using UnityEngine;

public static class Layers
{
    public static LayerMask PlayerLayer => 1 << playerLayerIndex;
    public static LayerMask GroundLayer => 1 << groundLayerIndex;
    public static LayerMask EnemyLayer => 1 << enemyLayerIndex;
    private const int playerLayerIndex = 6;
    private const int groundLayerIndex = 7;
    private const int enemyLayerIndex = 8;
}

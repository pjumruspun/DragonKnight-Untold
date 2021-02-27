using UnityEngine;

public static class Layers
{
    public static LayerMask PlayerLayer => 1 << playerLayerIndex;
    public static LayerMask GroundLayer => 1 << groundLayerIndex;
    public static LayerMask EnemyLayer => 1 << enemyLayerIndex;
    public static LayerMask PlayerAttackLayer => 1 << playerAttackLayerIndex;
    public static LayerMask EnemyProjectileLayer => 1 << enemyProjectileLayerIndex;
    public const int playerLayerIndex = 6;
    public const int groundLayerIndex = 7;
    public const int enemyLayerIndex = 8;
    public const int playerAttackLayerIndex = 9;
    public const int enemyProjectileLayerIndex = 10;
}

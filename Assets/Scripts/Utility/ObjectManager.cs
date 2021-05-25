using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoSingleton<ObjectManager>
{
    public ObjectPool SwordWaves;
    public ObjectPool FloatingDamage;
    public ObjectPool Arrows;
    public ObjectPool UltimateArrows;
    public ObjectPool ChargedArrows;
    public ObjectPool ItemPickups;
    public ObjectPool Keys;
    public ObjectPool HealthPotion;
    public ObjectPool PerkUpgradePotion;
    public ObjectPool SlashEffect;
    public ObjectPool Explosion;
    public ObjectPool CurveRoute;

    [Header("Floating Damage")]
    [SerializeField]
    private GameObject floatingDamagePrefab;
    [SerializeField]
    private int floatingDamagePoolSize = 20;


    [Header("Sword Waves")]
    [SerializeField]
    private GameObject swordWavePrefab;
    [SerializeField]
    private int swordWavePoolSize = 5;

    [Header("Arrows")]
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private int arrowPoolSize = 30;

    [Header("Ultimate Arrows")]
    [SerializeField]
    private GameObject ultArrowPrefab;
    [SerializeField]
    private int ultArrowPoolSize = 5;

    [Header("Charged Arrows")]
    [SerializeField]
    private GameObject chargedArrowsPrefab;
    [SerializeField]
    private int chargedArrowsPoolSize = 3;

    [Header("Item Pickups")]
    [SerializeField]
    private GameObject itemPickupPrefab;
    [SerializeField]
    private int itemPickupPoolSize = 10;

    [Header("Keys")]
    [SerializeField]
    private GameObject keyPrefab;
    [SerializeField]
    private int keyPoolsize = 10;

    [Header("Health Potion")]
    [SerializeField]
    private GameObject healthPotionPrefab;
    [SerializeField]
    private int healthPotionPoolsize = 10;

    [Header("Health Potion")]
    [SerializeField]
    private GameObject perkUpgradePotionPrefab;
    [SerializeField]
    private int perkUpgradePotionPoolsize = 2;

    [Header("Slash Effect")]
    [SerializeField]
    private GameObject slashEffectPrefab;
    [SerializeField]
    private int slashEffectPoolsize = 5;
    [SerializeField]
    private float slashEffectRandomAngle = 20.0f;
    [SerializeField]
    private float slashEffectDestroyDuration = 0.5f;

    [Header("Explosion Effect")]
    [SerializeField]
    private GameObject explosionEffectPrefab;
    [SerializeField]
    private int explosionEffectPoolsize = 5;

    [Header("Curve Route")]
    [SerializeField]
    private GameObject curveRoutePrefab;
    [SerializeField]
    private int curveRoutePoolSize = 10;

    public void SpawnSlashingEffect(Vector3 spawnPosition)
    {
        float rotationZ = Random.Range(-slashEffectRandomAngle, slashEffectRandomAngle);
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        GameObject slashEffect = ObjectManager.Instance.SlashEffect.SpawnObject(spawnPosition, rotation);
        CoroutineUtility.ExecDelay(() => ObjectManager.Instance.SlashEffect.ReturnObject(slashEffect), slashEffectDestroyDuration);
    }

    private void Start()
    {
        FloatingDamage = new ObjectPool(floatingDamagePrefab, floatingDamagePoolSize);
        SwordWaves = new ObjectPool(swordWavePrefab, swordWavePoolSize);
        Arrows = new ObjectPool(arrowPrefab, arrowPoolSize);
        UltimateArrows = new ObjectPool(ultArrowPrefab, ultArrowPoolSize);
        ItemPickups = new ObjectPool(itemPickupPrefab, itemPickupPoolSize);
        Keys = new ObjectPool(keyPrefab, keyPoolsize);
        HealthPotion = new ObjectPool(healthPotionPrefab, healthPotionPoolsize);
        PerkUpgradePotion = new ObjectPool(perkUpgradePotionPrefab, perkUpgradePotionPoolsize);
        SlashEffect = new ObjectPool(slashEffectPrefab, slashEffectPoolsize);
        Explosion = new ObjectPool(explosionEffectPrefab, explosionEffectPoolsize);
        ChargedArrows = new ObjectPool(chargedArrowsPrefab, chargedArrowsPoolSize);
        CurveRoute = new ObjectPool(curveRoutePrefab, curveRoutePoolSize);
    }
}

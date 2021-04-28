using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoSingleton<ObjectManager>
{
    public ObjectPool SwordWaves;
    public ObjectPool FloatingDamage;
    public ObjectPool CritFloatingDamage;
    public ObjectPool Arrows;
    public ObjectPool ChargedArrows;
    public ObjectPool ItemPickups;
    public ObjectPool Keys;
    public ObjectPool SlashEffect;
    public ObjectPool CurveRoute;

    [Header("Floating Damage")]
    [SerializeField]
    private GameObject floatingDamagePrefab;
    [SerializeField]
    private int floatingDamagePoolSize = 20; // How many floating damage can there be at time?

    [Header("Crit Floating Damage")]
    [SerializeField]
    private GameObject critFloatingDamagePrefab;
    [SerializeField]
    private int critFloatingDamagePoolSize = 20;

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

    [Header("Slash Effect")]
    [SerializeField]
    private GameObject slashEffectPrefab;
    [SerializeField]
    private int slashEffectPoolsize = 5;
    [SerializeField]
    private float slashEffectRandomAngle = 20.0f;
    [SerializeField]
    private float slashEffectDestroyDuration = 0.5f;

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
        CritFloatingDamage = new ObjectPool(critFloatingDamagePrefab, critFloatingDamagePoolSize);
        SwordWaves = new ObjectPool(swordWavePrefab, swordWavePoolSize);
        Arrows = new ObjectPool(arrowPrefab, arrowPoolSize);
        ItemPickups = new ObjectPool(itemPickupPrefab, itemPickupPoolSize);
        Keys = new ObjectPool(keyPrefab, keyPoolsize);
        SlashEffect = new ObjectPool(slashEffectPrefab, slashEffectPoolsize);
        ChargedArrows = new ObjectPool(chargedArrowsPrefab, chargedArrowsPoolSize);
        CurveRoute = new ObjectPool(curveRoutePrefab, curveRoutePoolSize);
    }
}

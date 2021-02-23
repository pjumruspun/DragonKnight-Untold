using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public HashSet<Collider2D> HitColliders => hitColliders;
    private HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        hitColliders.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        hitColliders.Remove(other);
    }
}

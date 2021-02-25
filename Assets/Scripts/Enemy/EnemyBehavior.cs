using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : StateMachineBehaviour
{
    // Cached speed
    protected float cachedActualSpeed;


    // Other stuff
    protected Animator animator;
    protected Transform transform;
    protected Rigidbody2D rigidbody2D;
    protected Transform player;
    protected Enemy enemy;

    // Coroutine for finding player
    protected Coroutine findingPlayerCoroutine;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        rigidbody2D = animator.gameObject.GetComponent<Rigidbody2D>();
        transform = animator.gameObject.transform;
        player = PlayerAbilities.Instance.gameObject.transform;
        enemy = transform.gameObject.GetComponent<Enemy>();

        // Cache once on enter
        cachedActualSpeed = enemy.EnemyBaseSpeed + Random.Range(-enemy.RandomSpeedFactor, enemy.RandomSpeedFactor);

        // We make the AI repeatedly looking for players
        findingPlayerCoroutine = CoroutineUtility.Instance.CreateCoroutine(RepeatedlyLookForPlayer());
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.AdjustRotation();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CoroutineUtility.Instance.KillCoroutine(findingPlayerCoroutine);
    }

    protected void LookForPlayer()
    {
        // Right now the AI will attempt to find players no matter where the player is
        // We could optimize by checking if player distance is in chasing range or not
        // Before we perform this whole function

        Vector3 position = transform.position; // The enemy's position
        float distance = enemy.ChasingRange; // Chasing ranging is looking range
        Vector3 direction = player.position - position;

        LayerMask raycastLayer;
        if (enemy.CanSeeThroughWalls)
        {
            // Will only raycast onto player layer
            raycastLayer = Layers.PlayerLayer;
        }
        else
        {
            // Enemy vision blocks by the wall
            // So need to also raycast on walls (ground) as well
            raycastLayer = Layers.PlayerLayer | Layers.GroundLayer;
        }

        Debug.DrawRay(position, direction.normalized * distance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, raycastLayer);

        // Hit something and that "something" is the player
        enemy.ShouldChase = hit && IsPlayer(hit.collider.gameObject);
    }

    private IEnumerator RepeatedlyLookForPlayer()
    {
        while (true)
        {
            LookForPlayer();
            yield return new WaitForSeconds(enemy.ChasingInterval);
        }
    }

    private bool IsPlayer(GameObject gameObject)
    {
        return gameObject.TryGetComponent<PlayerAbilities>(out _);
    }


}

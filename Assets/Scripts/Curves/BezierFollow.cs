using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BezierConfig
{
    public float speed;
    public bool rotateAlongPath;

    public BezierConfig(float speed = 1.0f, bool rotateAlongPath = false)
    {
        this.speed = speed;
        this.rotateAlongPath = rotateAlongPath;
    }
}

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes = new Transform[1];
    private CurveRoute curveRoute;
    private int routeToGo;
    private float tParam;
    private Vector2 objectPosition;
    private BezierConfig bezierConfig;
    private Coroutine movingCoroutine;

    public void SetRoute(CurveRoute route)
    {
        curveRoute = route;
        routes[0] = route.transform;
    }

    public void Activate(BezierConfig bezierConfig)
    {
        routeToGo = 0;
        tParam = 0f;
        this.bezierConfig = bezierConfig;
        if (movingCoroutine == null)
        {
            movingCoroutine = StartCoroutine(GoByTheRoute(routeToGo));
        }
        else
        {
            throw new System.InvalidOperationException("Tries to call Activate() in BezierFollow when coroutine already existed");
        }
    }

    // Create thread to handle follow's movement
    private IEnumerator GoByTheRoute(int routeNum)
    {
        Vector2 p0 = routes[routeNum].GetChild(0).position;
        Vector2 p1 = routes[routeNum].GetChild(1).position;
        Vector2 p2 = routes[routeNum].GetChild(2).position;
        Vector2 p3 = routes[routeNum].GetChild(3).position;

        Vector3 lastPosition = transform.position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * bezierConfig.speed;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            // Move

            transform.position = objectPosition;
            Vector3 newPosition = transform.position;

            // Rotate along velocity
            if (bezierConfig.rotateAlongPath)
            {
                Vector2 velocityVector = newPosition - lastPosition;
                float angle = Mathf.Atan2(velocityVector.y, velocityVector.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // Set last position
            lastPosition = newPosition;

            // Do this frame by frame
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        Deactivate();
    }

    private void Deactivate()
    {
        StopCoroutine(movingCoroutine);
        movingCoroutine = null;
        gameObject.SetActive(false);
    }
}

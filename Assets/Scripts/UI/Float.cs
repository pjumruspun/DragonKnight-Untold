using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMesh))]
public class Float : MonoBehaviour
{
    [SerializeField]
    private float floatingSpeed = 0.5f;
    private TextMesh textMesh;

    public void SetColor(Color color)
    {
        Debug.Log(textMesh);
        textMesh.color = color;
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + Vector3.up * floatingSpeed * Time.fixedDeltaTime;
    }
}

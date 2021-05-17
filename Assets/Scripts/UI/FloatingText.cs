using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshPro))]
public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private float floatingSpeed = 0.5f;

    [SerializeField]
    private TextMeshPro textMesh;

    private float originalSize;
    private Color originalColor;

    public void SetText(string text)
    {
        EnsureAvailability();
        textMesh.text = text;
    }

    public void SetColor(Color color)
    {
        EnsureAvailability();
        textMesh.color = color;
    }

    public void SetSize(int size)
    {
        EnsureAvailability();
        textMesh.fontSize = size;
    }

    private void Awake()
    {
        EnsureAvailability();
        originalSize = textMesh.fontSize;
        originalColor = textMesh.color;
    }

    private void OnDisable()
    {
        CoroutineUtility.ExecDelay(() =>
        {
            EnsureAvailability();
            textMesh.fontSize = originalSize;
            textMesh.color = originalColor;
        }, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + Vector3.up * floatingSpeed * Time.fixedDeltaTime;
    }

    private void EnsureAvailability()
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }
    }
}

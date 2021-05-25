using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
[RequireComponent(typeof(RectTransform))]
public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI header;

    [SerializeField]
    private TextMeshProUGUI content;

    [SerializeField]
    private LayoutElement layoutElement;

    [SerializeField]
    private int characterWrapLimit;

    private RectTransform rectTransform;

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            this.header.gameObject.SetActive(false);
        }
        else
        {
            this.header.gameObject.SetActive(true);
            this.header.text = header;
        }

        this.content.text = content;
        UpdateTooltipSize();
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            UpdateTooltipSize();
        }

        Vector2 mousePosition = Input.mousePosition;

        // Change pivot depends on the location
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);

        // Move along cursor
        transform.position = mousePosition;
    }

    private void UpdateTooltipSize()
    {
        int headerLength = header.text.Length;
        int contentLength = content.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
    }
}

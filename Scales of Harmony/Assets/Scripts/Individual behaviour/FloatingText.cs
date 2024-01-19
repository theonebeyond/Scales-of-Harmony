using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    private TextMeshProUGUI textMesh; // For TextMeshPro
    // private Text textComponent; // Uncomment for standard UI Text

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        // textComponent = GetComponent<Text>(); // Uncomment for standard UI Text
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1.0f - (elapsedTime / fadeDuration));

            // For TextMeshPro
            Color color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;

            // For standard UI Text, uncomment the following lines
            // Color color = textComponent.color;
            // color.a = alpha;
            // textComponent.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }
}

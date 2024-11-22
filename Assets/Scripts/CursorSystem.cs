using UnityEngine;
using System.Collections;

public class CursorSystem : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot = new Vector2(16, 16);
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [Header("Click Effect Settings")]
    [SerializeField] private GameObject circleEffectPrefab;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float startScale = 0.1f;
    [SerializeField] private float endScale = 2f;
    [SerializeField] private Color startColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private Color endColor = new Color(1f, 1f, 1f, 0f);
    [SerializeField] private float zOffset = 0f; // Para controlar a profundidade do efeito

    private void Start()
    {
        // Configura o cursor
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateClickEffect();
        }
    }

    private void CreateClickEffect()
    {
        // Converte a posição do mouse para posição no mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = zOffset; // Define a profundidade do efeito

        // Cria o efeito
        GameObject effect = Instantiate(circleEffectPrefab, mousePos, Quaternion.identity);
        SpriteRenderer spriteRenderer = effect.GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = startColor;
            effect.transform.localScale = Vector3.one * startScale;
            StartCoroutine(AnimateCircle(effect, spriteRenderer));
        }
    }

    private IEnumerator AnimateCircle(GameObject circle, SpriteRenderer renderer)
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            
            // Usa uma curva suave para a animação
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);
            
            // Anima o tamanho
            float currentScale = Mathf.Lerp(startScale, endScale, smoothT);
            circle.transform.localScale = Vector3.one * currentScale;
            
            // Anima a cor/transparência
            renderer.color = Color.Lerp(startColor, endColor, smoothT);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(circle);
    }
}
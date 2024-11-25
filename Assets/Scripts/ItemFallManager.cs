using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFallManager : MonoBehaviour
{
  
 [Header("Configurações")]
    [SerializeField] private float fallSpeed        = 200f; // Velocidade em pixels por segundo
    [SerializeField] private float targetY          = -160f;  // Posição Y final (ajuste conforme necessário)
    
    private RectTransform rectTransform;
    private bool hasLanded = false;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Debug.Log($"Item UI iniciado em: {rectTransform.anchoredPosition}");
    }
    
    void Update()
    {
        if (!hasLanded)
        {
            // Move o item para baixo
            Vector2 position = rectTransform.anchoredPosition;
            position.y -= fallSpeed * Time.deltaTime;
            rectTransform.anchoredPosition = position;
            
            // Verifica se chegou na posição alvo
            if (position.y <= targetY)
            {
                hasLanded = true;
                position.y = targetY;
                rectTransform.anchoredPosition = position;
                Debug.Log("Item chegou ao destino!");
            }
        }
    }
    
    // Método para spawnar um item UI que cai
    public static ItemFallManager SpawnFallingItem(GameObject prefab, Vector2 startPosition, Transform parent)
    {
        GameObject item = Instantiate(prefab, parent);
        RectTransform rt = item.GetComponent<RectTransform>();
        rt.anchoredPosition = startPosition;
        
        ItemFallManager fallManager = item.AddComponent<ItemFallManager>();
        return fallManager;
    }
}

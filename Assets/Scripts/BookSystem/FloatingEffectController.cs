using UnityEngine;
using System.Collections;


public class FloatingEffectController : MonoBehaviour
{
    [Header("Configurações de Flutuação")]
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float rotationAmount = 5f;

    private Vector3 startPosition;
    private float randomOffset;
    private bool isDragging = false;
    private DragItemScript dragScript;

    void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
        dragScript = GetComponent<DragItemScript>();
        StartCoroutine(FloatingMovement());
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
        if (!dragging)
        {
            // Atualiza a posição inicial quando o drag terminar
            startPosition = transform.position;
        }
    }

    IEnumerator FloatingMovement()
    {
        while (true)
        {
            if (!isDragging)
            {
                float y = Mathf.Sin((Time.time + randomOffset) * frequency) * amplitude;
                float rotation = Mathf.Sin((Time.time + randomOffset) * frequency) * rotationAmount;
                
                transform.position = startPosition + new Vector3(0f, y, 0f);
                transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
            yield return null;
        }
    }
}
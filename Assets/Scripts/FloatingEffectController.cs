using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffectController : MonoBehaviour
{
    [Header("Configurações de Flutuação")]
    [SerializeField] private float amplitude = 0.5f; // Quanto o livro move pra cima/baixo
    [SerializeField] private float frequency = 1f;   // Velocidade do movimento
    [SerializeField] private float rotationAmount = 5f; // Quanto o livro gira

    private Vector3 startPosition;
    private float randomOffset;

    void Start()
    {
        startPosition = transform.position;
        // Offset aleatório para que cada livro flutue diferente
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
        StartCoroutine(FloatingMovement());
    }

    IEnumerator FloatingMovement()
    {
        while (true)
        {
            // Calcula o movimento vertical
            float y = Mathf.Sin((Time.time + randomOffset) * frequency) * amplitude;
            // Calcula a rotação suave
            float rotation = Mathf.Sin((Time.time + randomOffset) * frequency) * rotationAmount;

            // Aplica o movimento
            transform.position = startPosition + new Vector3(0f, y, 0f);
            transform.rotation = Quaternion.Euler(0, 0, rotation);

            yield return null;
        }
    }
}

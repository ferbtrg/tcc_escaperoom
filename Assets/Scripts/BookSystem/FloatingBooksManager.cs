using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBooksManager : MonoBehaviour
{
    private Vector3[]       _originalPositions;
    private Transform[]     _books;

    void Start()
    {
        // Pega todos os livros flutuantes
        _books = GetComponentsInChildren<Transform>();
        _originalPositions = new Vector3[_books.Length];
        
        // Guarda as posições originais
        for (int i = 0; i < _books.Length; i++)
            _originalPositions[i] = _books[i].position;
    }

    public void OnZoomIn()
    {
        // Move os livros para fora da tela ou os esconde
        foreach (Transform book in _books)
        {
            book.gameObject.SetActive(false);
        }
    }

    public void OnZoomOut()
    {
        // Restaura os livros para suas posições originais
        for (int i = 0; i < _books.Length; i++)
        {
            _books[i].gameObject.SetActive(true);
            _books[i].position = _originalPositions[i];
        }
    }
}

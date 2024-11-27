using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookGroupController : MonoBehaviour
{
    [SerializeField] private GameObject _firstBook;
    [SerializeField] private GameObject _secBook;
    [SerializeField] private GameObject _thirdBook;
    [SerializeField] private GameObject _fourthBook;
    [SerializeField] private GameObject _fifthBook;

    void Start()
    {
        SetBookVisibility();
    }

    public void SetBookVisibility()
    {
        _firstBook.SetActive( false);
        _secBook.SetActive( false );
        _thirdBook.SetActive( false );
        _fourthBook.SetActive( false );
        _fifthBook.SetActive( false );
    }

    public void ShowBook(string color)
    {
        Debug.Log($"Tentando mostrar flor da cor: {color}");

        GameObject obj = GameObject.FindWithTag( color );
        obj.SetActive( true );
    }
}

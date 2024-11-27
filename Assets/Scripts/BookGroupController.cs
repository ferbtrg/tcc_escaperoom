using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookGroupController : MonoBehaviour
{
    [SerializeField] private GameObject _blueBook;
    [SerializeField] private GameObject _purpleBook;
    [SerializeField] private GameObject _blackBook;
    [SerializeField] private GameObject _greenBook;
    [SerializeField] private GameObject _redBook;

    // Start is called before the first frame update

    void Start()
    {
        //SetBookVisibility();
    }

    public void SetBookVisibility()
    {
        // Começa com todas as flores invisíveis
        _blueBook.SetActive( false);
        _purpleBook.SetActive( false );
        _blackBook.SetActive( false );
        _greenBook.SetActive( false );
        _redBook.SetActive( false );
    }

    public void ShowBook(string color)
    {
        Debug.Log($"Tentando mostrar flor da cor: {color}");
       
        switch( color )
        {
            case "Blue":
                if( _blueBook != null)
                    _blueBook.SetActive(true);
                break;

            case "Purple":
                if( _purpleBook != null )
                    _purpleBook.SetActive( true );
                break;

            case "Green":
                if( _greenBook != null )
                    _greenBook.SetActive(true);
                break;

            case "Red":
                if( _redBook != null )
                    _redBook.SetActive(true);
                break;
                
                case "Black":
                if( _blackBook != null )
                    _blackBook.SetActive(true);
                break;
        }
    }
}

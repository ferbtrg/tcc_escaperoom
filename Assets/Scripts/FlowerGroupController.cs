using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGroupController : MonoBehaviour
{
    [SerializeField] private GameObject _blueFlower;
    [SerializeField] private GameObject _purpleFlower;
    [SerializeField] private GameObject _yellowFlower;
    [SerializeField] private GameObject _pinkFlower;

    public bool isClone = false;

    void Start()
    {
        if( isClone )
            return;
        
        SetFlowerVisibility();
    }

    public void SetFlowerVisibility()
    {
        // Começa com todas as flores invisíveis
        _blueFlower.SetActive( false);
        _purpleFlower.SetActive( false );
        _yellowFlower.SetActive( false );
        _pinkFlower.SetActive( false );
    }

    public void ShowFlower(string color)
    {
        Debug.Log($"Tentando mostrar flor da cor: {color}");
       
        switch(color.ToLower())
        {
            case "blue":
                if(_blueFlower != null) {
                    _blueFlower.SetActive(true);
                    Debug.Log("Ativando flor azul");
                }
                break;
            case "purple":
                if(_purpleFlower != null) {
                    _purpleFlower.SetActive(true);
                    Debug.Log("Ativando flor roxa");
                }
                break;
            case "yellow":
                if(_yellowFlower != null) {
                    _yellowFlower.SetActive(true);
                    Debug.Log("Ativando flor amarela");
                }
                break;
            case "pink":
                if(_pinkFlower != null) {
                    _pinkFlower.SetActive(true);
                    Debug.Log("Ativando flor rosa");
                }
                break;
        }

                // Debug do estado atual das flores
        Debug.Log($"Blue ativo? {_blueFlower?.activeSelf}, Existe? {_blueFlower != null}");
        Debug.Log($"Purple ativo? {_purpleFlower?.activeSelf}, Existe? {_purpleFlower != null}");
        Debug.Log($"Yellow ativo? {_yellowFlower?.activeSelf}, Existe? {_yellowFlower != null}");
        Debug.Log($"Pink ativo? {_pinkFlower?.activeSelf}, Existe? {_pinkFlower != null}");
    }

}

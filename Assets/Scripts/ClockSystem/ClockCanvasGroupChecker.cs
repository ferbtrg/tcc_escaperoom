using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClockCanvasGroupChecker : MonoBehaviour
{
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private Image _star;
    // Start is called before the first frame update
    void Start()
    {
        CheckCanvasGroupConditions();
    }

    private void CheckCanvasGroupConditions()
    {
        if( _canvasGroup != null && !_canvasGroup.blocksRaycasts && !_canvasGroup.interactable && _star != null )
           _star.gameObject.SetActive( false );
        else
           _star.gameObject.SetActive( true );
    }
}

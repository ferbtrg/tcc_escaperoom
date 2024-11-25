using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if( string.IsNullOrEmpty( sceneName ) )
            return;

        SceneManager.LoadScene( sceneName );
        Debug.Log( string.Format( "Scene clicked:{0}", sceneName ) );

        bool puzzleComplete     = false;

        var parent      = transform.parent;
        var children    = parent.GetComponentsInChildren<Transform>();


        Transform puzzleCanvas = children.Where(child => child.name == "Canvas").FirstOrDefault();
        if( puzzleCanvas != null )
        {
            var canvasGroup = puzzleCanvas.GetComponent<CanvasGroup>();
            if(canvasGroup != null)
                puzzleComplete = !canvasGroup.interactable;
        }

        StartCoroutine(SetupNewScene( sceneName, puzzleComplete ));
    }

    private IEnumerator SetupNewScene( string sceneName, bool status )
    {
        yield return new WaitForEndOfFrame();

        GameObject sceneRoot = GameObject.Find( sceneName );
        if( sceneRoot != null )
        {
            
            CanvasGroup canvasGroup = sceneRoot.GetComponentInChildren<CanvasGroup>();
            if(canvasGroup != null)
            {
                canvasGroup.interactable        = status;
                canvasGroup.blocksRaycasts      = status;
            }
        }
    }
}

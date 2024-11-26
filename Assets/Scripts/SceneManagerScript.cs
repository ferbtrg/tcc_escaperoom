using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Metadata;

public class SceneScript : MonoBehaviour
{  
    private static bool puzzleStatusToSet = false;
    static string _sceneName = "";
    
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return;

        _sceneName = sceneName;
        
        var parent = transform.parent;
        var children = parent.GetComponentsInChildren<Transform>();
        Transform puzzleCanvas = children.FirstOrDefault(child => child.name == "Canvas");
        
        if (puzzleCanvas != null)
        {
            var canvasGroup = puzzleCanvas.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            { 
                if( sceneName == "Scene1Soma" )
                    puzzleStatusToSet = true;
                else
                    puzzleStatusToSet = !canvasGroup.interactable;
            }
        }

        // Registra o evento só quando vai trocar de cena
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var sceneName   = GameObject.Find(scene.name);
        var children    = sceneName.GetComponentsInChildren<Transform>();
        var canvas = children.FirstOrDefault(child => child.name == "Canvas");
        var canvasGroup = canvas.GetComponent<CanvasGroup>();
        if(canvasGroup != null)
        {
            canvasGroup.interactable = puzzleStatusToSet;
            canvasGroup.blocksRaycasts = puzzleStatusToSet;
        }

        // Remove o evento após configurar a cena
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
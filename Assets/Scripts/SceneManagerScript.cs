using System.Collections;
using System.Collections.Generic;
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
    }
}

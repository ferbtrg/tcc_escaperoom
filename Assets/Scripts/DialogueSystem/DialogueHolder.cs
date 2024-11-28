using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(DialogueSequence());
        }

        private IEnumerator DialogueSequence()
        {
            int totalDialogues = transform.childCount;
            bool finalLine = false;
            for( int i = 0; i < totalDialogues; i++ )
            {
                GameObject currentChild = transform.GetChild(i).gameObject;
                if( currentChild.GetComponent<UnityEngine.UI.Image>() != null )
                    continue;
                
                DeactivateDialogueLines();
                currentChild.SetActive(true);
                
                DialogueLine dialogueLine = currentChild.GetComponent<DialogueLine>();

                if( dialogueLine != null )
                {
                    finalLine = dialogueLine.ToString() == "FIM!";
                    yield return new WaitUntil(() => dialogueLine.Finished);
                    if( i == totalDialogues - 1 && !finalLine )
                    {
                        SceneManager.LoadScene("Scene1Soma");
                        yield break;
                    }
                }
            }
            

            if( !finalLine )
                gameObject.SetActive(false);

            
        }

        private void DeactivateDialogueLines()
        {
            for( int i = 0; i < transform.childCount; i++ )
            { 
                //Deactivate only DialogueLine objects, not images
                GameObject child = transform.GetChild(i).gameObject;
                if( child.GetComponent<DialogueLine>() != null )
                    child.SetActive(false);
            }
        }
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "TetheredClimb"; 

    void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return new WaitForSeconds(15f); 

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);


        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

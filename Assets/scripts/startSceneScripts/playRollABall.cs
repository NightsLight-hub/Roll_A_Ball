using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playRollABall : MonoBehaviour
{
    private AsyncOperation loadGame2AO;
    private void Start()
    {
        loadGame2AO = LoadGame2();
    }
    public void OnClick1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClick2()
    {
        loadGame2AO.allowSceneActivation = true;
    }

    private AsyncOperation LoadGame2()
    {
        var oa = SceneManager.LoadSceneAsync("Game2", LoadSceneMode.Single);
        oa.allowSceneActivation = false;
        oa.completed += (AsyncOperation obj) =>
        {
            Debug.Log("load game2 completed");
        };
        StartCoroutine(printProgress(oa));
        return oa;
    }

    private IEnumerator printProgress(AsyncOperation ao)
    {
        while (!ao.isDone)
        {
            Debug.LogFormat("load progress {0}", ao.progress);
            if (ao.progress >= 0.9f)
            {
                Debug.Log("load game2 set allowSceneActivation to true");
                yield break;
            }
            yield return null;
        }
    }

}

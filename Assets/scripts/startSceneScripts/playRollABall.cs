using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playRollABall : MonoBehaviour
{
    public void OnClick1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClick2()
    {
        SceneManager.LoadScene("Game2");
    }
}

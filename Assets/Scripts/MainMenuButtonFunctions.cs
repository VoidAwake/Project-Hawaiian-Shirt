using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonFunctions : MonoBehaviour
{
    [SerializeField] int buildIndex;

    public void LoadSceneByIndex()
    {
        //if (SceneManager.GetSceneByBuildIndex(buildIndex).IsValid())
        //{
            SceneManager.LoadScene(buildIndex);
        //}
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void DeleteSelf()
    {
        Destroy(this.gameObject);
    }
}

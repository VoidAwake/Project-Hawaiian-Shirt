using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hawaiian.UI.MainMenu
{
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
            Destroy(gameObject);
        }
    }
}

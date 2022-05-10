using UnityEngine;
using UnityEngine.SceneManagement;
using Hawaiian.UI.CharacterSelect;

namespace Hawaiian.UI.MainMenu
{
    public class MainMenuButtonFunctions : MonoBehaviour
    {
        enum ButtonFunction { LoadSceneByIndex, CloseApp }
        [SerializeField] ButtonFunction function;
        [SerializeField] int buildIndex;
        [SerializeField] bool callTransition;

        public void LoadSceneByIndex()
        {
            if (callTransition)
            {
                Transition transition = FindObjectOfType<Transition>();
                if (transition != null)
                {
                    transition.BeginTransition(true, true, buildIndex, false);
                }
                else
                {
                    SceneManager.LoadScene(buildIndex);
                }
            }
            else
            {
                SceneManager.LoadScene(buildIndex);
            }
        }

        public void CloseApp()
        {
            Application.Quit();
        }

        public void DeleteSelf()
        {
            Destroy(gameObject);
        }

        public void CallSerializedFunction()
        {
            switch (function)
            {
                case ButtonFunction.LoadSceneByIndex:
                    LoadSceneByIndex();
                    break;
                case ButtonFunction.CloseApp:
                    CloseApp();
                    break;
                default:
                    break;
            }
        }
    }
}

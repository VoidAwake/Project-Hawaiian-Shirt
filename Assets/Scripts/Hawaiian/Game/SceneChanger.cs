using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/SceneChanger")]
    public class SceneChanger : ScriptableObject
    {
        public event Func<object, EventArgs, Task> ChangingScene;
        
        public async void ChangeScene(SceneReference scene)
        {
            await ChangingScene.Raise(this, EventArgs.Empty);
            
            SceneManager.LoadScene(scene);
        }
    }
}
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hawaiian.Game
{
    [CreateAssetMenu(menuName = "Hawaiian/Managers/SceneChanger")]
    public class SceneChanger : ScriptableObject
    {
        public event Func<object, EventArgs, Task> ChangingScene;
        
        public async Task ChangeScene(SceneReference scene)
        {
            await ChangingScene.Raise(this, EventArgs.Empty);

            await SceneManager.LoadSceneAsync(scene);
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Game
{
    public class ModeManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlayerManager playerManager;
        
        public IModeController modeController;

        public UnityEvent<IModeController> initialised = new();
        
        private void Awake()
        {
            var modeControllerObject = Instantiate(gameManager.CurrentGameMode.modeControllerPrefab, transform);

            modeController = modeControllerObject.GetComponent<IModeController>();
            
            modeController.Initialise(playerManager, gameManager.GameModeSceneReference);
            
            initialised.Invoke(modeController);
        }

        public void SaveScores()
        {
            modeController.SaveScores();
        }
    }
}
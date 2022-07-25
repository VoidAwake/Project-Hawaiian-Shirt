using UnityEngine;
using UnityEngine.Events;

namespace Hawaiian.Game.GameModes
{
    public class ModeManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlayerManager playerManager;
        
        public ModeController<ModeSceneReference> modeController;

        public UnityEvent<ModeController<ModeSceneReference>> initialised = new();
        
        private void Awake()
        {
            var modeControllerObject = Instantiate(gameManager.CurrentGameMode.modeControllerPrefab, transform);

            modeController = modeControllerObject.GetComponent<ModeController<ModeSceneReference>>();
            
            modeController.Initialise(playerManager, gameManager.ModeSceneReference);
            
            initialised.Invoke(modeController);
        }

        public void SaveScores()
        {
            modeController.SaveScores();
        }
    }
}
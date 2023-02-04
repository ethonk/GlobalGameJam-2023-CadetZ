using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Core Events")]
        [SerializeField] private UnityEvent onStart;
        [SerializeField] private UnityEvent onGameOver;

        [Header("Events")]
        [SerializeField] private UnityEvent onKill;

        [Header("Variables")]
        [SerializeField] private GameState gameState = GameState.Start;

        // game manager instance
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; }}
        
        // game states
        public enum GameState { Start, GameOver };

        /// <summary>
        /// Ensures that the Game Manager only ever exists once.
        /// </summary>
        private void Awake()
        {
            // set the instance
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;
            
            // then run the game state
            ChangeGameState(gameState);
        }

        public void ChangeGameState(GameState state)
        {
            // first change the game state
            gameState = state;
            // then run the corresponding event with the game state
            switch (gameState)
            {
                case GameState.Start:
                    onStart.Invoke();
                    break;
                case GameState.GameOver:
                    onGameOver.Invoke();
                    break;
                default:
                    print("State that you've requested to switch to is not implemented.");
                    break;
            }
        }

        public void DebugLog(string msg)
        {
            print(msg);
        }

        public void GameOver()
        {
            UnityEngine.SceneManagement.SceneManager
                .LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void AwardKill()
        {
            onKill.Invoke();
        }
    }
}

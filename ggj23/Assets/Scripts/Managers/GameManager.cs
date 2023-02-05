using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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
        [SerializeField] private GameState gameState = GameState.Menu;
        public int score;

        // game manager instance
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; }}
        
        // game states
        public enum GameState { Menu, Start, GameOver };

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
            
            // if high score don't exist, make the key
            if (!PlayerPrefs.HasKey("HighScore"))
                PlayerPrefs.SetFloat("HighScore", 0);
        }

        private void Update()
        {
            if (gameState != GameState.Menu) return;
            if (Input.GetKeyDown(KeyCode.Space)) ChangeGameState(GameState.Start);
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
            //
            // 1) stop all AI
            //
            List<NavMeshAgent> allAi = new List<NavMeshAgent>(FindObjectsOfType<NavMeshAgent>());
            foreach (NavMeshAgent agent in allAi)
            {
                // start dance
                var newDance = Instantiate(Resources.Load<Transform>("Prefab/Chimp_CollardGreens"),
                    null, true);
                newDance.position = agent.transform.position - new Vector3(0, .55f, 0);
                
                // kill agent
                Destroy(agent.gameObject);
            }
            
            //
            // 2) replace player with dummy
            //
            var plr = FindObjectOfType<PlayerDetails>();
            
            // instantiate dummy player
            var dummy = Instantiate(plr.dummy, null, true);
            dummy.transform.position = plr.transform.position;
            
            // delete real player
            Destroy(plr.gameObject);

            //
            // 3) set high score and restart
            //
            
            // compare high score
            if (score > PlayerPrefs.GetFloat("HighScore"))
                PlayerPrefs.SetFloat("HighScore", score);
            
            // restart scene
            StartCoroutine(Restart());
        }

        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(3f);
            
            UnityEngine.SceneManagement.SceneManager
                .LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void AwardKill()
        {
            score++;
            onKill.Invoke();
            
            // play sound
            SoundManager.Instance.PlaySound("SFX/blood_splat");
            
            int randSound = UnityEngine.Random.Range(0, 4);
            string soundToPlay = "SFX/chimp_death-" + (randSound + 1).ToString();
            SoundManager.Instance.PlaySound(soundToPlay);
            
            
            // use here to check for game over
            
        }
    }
}

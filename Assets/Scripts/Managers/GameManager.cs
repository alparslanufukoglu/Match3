using UnityEngine;

namespace Managers
{
    public enum GameState
    {
        ready,
        busy,
        levelend
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameState currentState;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;
        public event Action OnMainMenuActivated;

        private void Awake()
        {
            Instance = this;
        }
        
        public void MainMenuActivated()
        {
            OnMainMenuActivated?.Invoke();
        }
    }
}

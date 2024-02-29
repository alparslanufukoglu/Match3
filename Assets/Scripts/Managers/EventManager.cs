using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;
        public event Action OnMainMenuActivated;
        public event Action<int> OnPlayButtonClicked;

        private void Awake()
        {
            Instance = this;
        }
        
        public void MainMenuActivated()
        {
            OnMainMenuActivated?.Invoke();
        }
        public void PlayButtonClicked(int index)
        {
            OnPlayButtonClicked?.Invoke(index);
        }
    }
}

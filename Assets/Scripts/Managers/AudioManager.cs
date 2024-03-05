using UnityEngine;
using UnityEngine.UI;
namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public AudioSource backgroundMusic;
        public AudioSource gameOverMusic;
        [SerializeField] private Slider volumeSlider;
        
        public void Awake()
        {
            if (!PlayerPrefs.HasKey("musicVolume"))
            {
                PlayerPrefs.SetFloat("musicVolume",1);
                Load();
            }
            else
            {
                Load();
            }
            Instance = this;
        }
        public void ChangeVolume()
        {
            AudioListener.volume = volumeSlider.value;
            Save();
        }
        private void Save()
        {
            PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        }
        private void Load()
        {
            PlayerPrefs.GetFloat("musicVolume");
        }
    }
}
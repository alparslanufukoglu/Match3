using System;
using TMPro;
using UIKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelCellView : UITableViewCell
{
   public TextMeshProUGUI levelNumberText; 
   public TextMeshProUGUI moveCountText; 
   public TextMeshProUGUI scoreText; 
   public TextMeshProUGUI playButtonText;
   public Image playButtonBackground;
   private readonly int _lockedTextSize = 9;
   private readonly int _playTextSize = 10;
   public Button playButton;
   private Level _level;
   public event Action OnClick;
        
        public void FillCell (Level level)
        {
            _level = level;
            levelNumberText.text = String.Concat("level : ",level.levelNumber);
            moveCountText.text = String.Concat("Move Count : " ,level.moveCount);
            SetScoreText();
            SetButtonStatus();
        }
        public void PlayButtonClicked()
        {
            SceneManager.LoadScene(1,LoadSceneMode.Additive);
            LoadSelectedLevel(_level);
        }

        private void LoadSelectedLevel(Level level)
        {
            LevelManager.Instance.SetCurrentLevel(level);
            OnClick?.Invoke();
        }
        private void SetScoreText()
        {
            if (_level.IsLocked())
            {
                scoreText.text = "Locked Level";
            }
            else if (_level.GetScore() == 0)
            {
                scoreText.text = "No Score";
            }
            else
            {
                scoreText.text = "High Score : " + _level.GetScore();
            }
        }
        private void SetButtonStatus()
        {
            if (_level.IsLocked()) 
            {
                LockButton();
            }
            else
            {
                UnlockButton();
            }
        }
        private void LockButton()
        {
            playButtonText.text = "Locked";
            playButtonText.fontSize = _lockedTextSize;
            playButtonBackground.color = Color.grey;
            playButton.interactable = false;
            playButton.onClick.RemoveListener(PlayButtonClicked);
        }

        private void UnlockButton()
        {
            playButtonText.text = "Play";
            playButtonText.fontSize = _playTextSize;
            playButtonBackground.color = Color.white;
            playButton.interactable = true;
            playButton.onClick.AddListener(PlayButtonClicked);
        }
        public void Clear()
        {
            playButton.onClick.RemoveListener(PlayButtonClicked);
        }
}
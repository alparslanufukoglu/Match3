using System;
using Cysharp.Threading.Tasks;
using Managers;
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
    private readonly int _lockedTextSize = 35;
    private readonly int _playTextSize = 50;
    public Button playButton;
    private Level _level;
    private CameraScaler _cameraScaler;
    public event Action OnClick;
    private void Start()
    {
        if (Camera.main != null) _cameraScaler = Camera.main.GetComponent<CameraScaler>();
    }

    public void FillCell(Level level)
    {
        _level = level;
        levelNumberText.text = String.Concat("level : ", level.levelNumber);
        moveCountText.text = String.Concat("Move Count : ", level.moveCount);
        SetScoreText();
        SetButtonStatus();
    }

    private async void PlayButtonClicked()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        EventManager.Instance.PlayButtonClicked(1);
        await UniTask.Yield(PlayerLoopTiming.LastFixedUpdate);
        LoadSelectedLevel(_level);
    }

    private void LoadSelectedLevel(Level level)
    {
        LevelManager.Instance.SetCurrentLevel(level);
        _cameraScaler.SetCameraPosition(level.gridWidth, level.gridHeight);
        OnClick?.Invoke();
        GridView.Instance.SetLevelUI(level);
        GridManager.Instance.SetGrid(level);
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
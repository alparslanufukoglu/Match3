using System;
using Managers;
using UnityEngine;
public class MainView : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;

    private void Awake()
    {
        EventManager.Instance.OnMainMenuActivated += ActivateMainMenu;
    }

    public void LevelsButtonClicked()
    {
        mainMenu.SetActive(false);
        levelsMenu.SetActive(true);
        LevelTableView.Instance.LoadLevels();
        LevelTableView.Instance.table.ReloadData(0);
        LevelTableView.Instance.table.ScrollToCellAt(0, withMargin: true);
    }
    
    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
    }
    public void DeactivateMainMenu()
    {
        mainMenu.SetActive(false);
    }
    public void ActivateLevelsMenu()
    {
        levelsMenu.SetActive(true);
    }
    public void DeactivateLevelsMenu()
    {
        levelsMenu.SetActive(false);
    }
}

using System;
using Managers;
using UnityEngine;
public class MainView : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;

    private void Start()
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

    private void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
    }
}

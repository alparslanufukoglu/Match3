using System.Net.Mime;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] backgroundImages;

    private void Start()
    {
        EventManager.Instance.OnMainMenuActivated += ActivateMainMenu;
        EventManager.Instance.OnPlayButtonClicked += ChangeBackgroundImage;
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

    private void ChangeBackgroundImage(int index)
    {
        image.sprite = backgroundImages[index];
    }
}

using UnityEngine;

public class MainView : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;
    
    public void LevelsButtonClicked()
    {
        mainMenu.SetActive(false);
        levelsMenu.SetActive(true);
        LevelGridView.Instance.LoadLevels();
        LevelGridView.Instance.table.ReloadData(0);
        LevelGridView.Instance.table.ScrollToCellAt(0, withMargin: true);
        
    }
}

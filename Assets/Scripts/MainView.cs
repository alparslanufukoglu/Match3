using UnityEngine;

public class MainView : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelsMenu;
    
    
    public void LevelsButtonClicked()
    {
        mainMenu.SetActive(false);
        levelsMenu.SetActive(true);
    }
}

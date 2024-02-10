using UIKit;
using UnityEngine;

public class LevelTableView : MonoBehaviour,IUITableViewDataSource, IUITableViewDelegate, IUITableViewMargin
{
    public UITableView table;
    public LevelCellView cellPrefab;
    public GameObject levelsMenu;
    public static LevelTableView Instance;
    private void Awake()
    {
        Instance = this;
        table.dataSource = this;
        table.@delegate = this;
        table.marginDataSource = this;
        if (!PlayerPrefs.HasKey("Level1"))
        {
            PlayerPrefs.SetInt("Level1",0);
        }
    }
    
    public void LoadLevels()
    {
        table.AppendData();
        if(LevelManager.Instance.Levels.Count < 15) LoadLevels();
    }

    public UITableViewCell CellAtIndexInTableView(UITableView tableView, int index)
    {
        return tableView.ReuseOrCreateCell(cellPrefab);
    }

    public int NumberOfCellsInTableView(UITableView tableView)
    {
        return LevelManager.Instance.Levels.Count;
    }

    public float ScalarForCellInTableView(UITableView tableView, int index)
    {
        return 120;
    }

    public float ScalarForUpperMarginInTableView(UITableView tableView, int index)
    {
        return index == 0 ? 100f : 10f;
    }

    public float ScalarForLowerMarginInTableView(UITableView tableView, int index)
    {
        return index == LevelManager.Instance.Levels.Count - 1 ? 100f : 10f;
    }

    public void CellAtIndexInTableViewWillAppear(UITableView tableView, int index)
    {
        var levelCell = tableView.GetLoadedCell<LevelCellView>(index);
        levelCell.OnClick += CellClicked;
        levelCell.FillCell(LevelManager.Instance.Levels[index]);
    }

    public void CellAtIndexInTableViewDidDisappear(UITableView tableView, int index)
    {
        var levelCell = tableView.GetLoadedCell<LevelCellView>(index);
        levelCell.OnClick -= CellClicked;
        levelCell.Clear();
    }
    
    private void CellClicked()
    {
        levelsMenu.SetActive(false);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpGrid : MonoBehaviour
{
    int maxObjectInGrid = 4;

    int curpage = 0;

    [SerializeField] GridLayoutGroup grid;
    [SerializeField] GameObject powerUpPrefab;

    [SerializeField] Button nextPageButton;
    [SerializeField]SelectedPowerUpsManager choosedPowerUps;

    [SerializeField] Card powerUpCard;
    void Start()
    {
        nextPageButton.onClick.AddListener(() =>
        {
            curpage++;
            SetPowerUpsInGrid();
        });
    }

    public void SetPowerUpsInGrid()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }

        int goCont = 0;
        var puList = choosedPowerUps.GetSelectedPowerUps();
        int startIndex = curpage * maxObjectInGrid;
        if (startIndex >= puList.Count) {startIndex = 0; curpage = 0; }
        for (int i = startIndex; i < puList.Count; i++)
        {
            if (goCont < maxObjectInGrid)
            {
                goCont++;
                int puIndex = i;
                var powerUp = GameObject.Instantiate(powerUpPrefab, grid.transform);;
                powerUp.GetComponent<Image>().sprite = puList[puIndex].icon;
                powerUp.GetComponentInChildren<TextMeshProUGUI>().text = puList[puIndex].name;

                
                var button = powerUp.GetComponent<Button>();
                
                button.onClick.AddListener(() =>
                    {
                        if (GameStateManager.instance.GetState() != GameState.Playing && GameStateManager.instance.GetState() != GameState.VisualizingPowerUps) return;
                        GameStateManager.instance.SetState(GameState.VisualizingPowerUps);
                        powerUpCard.SetPowerUp(puList[puIndex], false);
                        powerUpCard.gameObject.SetActive(true);
                    }
                );
                
            }
        }
    }
}

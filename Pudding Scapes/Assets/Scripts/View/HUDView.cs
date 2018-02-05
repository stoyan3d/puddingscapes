using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDView : MonoBehaviour {

    public Text turnText;
    public GameObject gameEndPanel;
    public Text endGameText;
    public Button restartButton;

    public static HUDView instance;

    private string gameWon = "You Win!";
    private string gameLost = "You Died!";

    WorldModel World { get { return WorldController.instance.World; } }
    private int turn;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There should only be one instance of HUDView");
            return;
        }

        instance = this;
    }

    // Use this for initialization
    void Start () {
        World.onTurnUpdateCallback += UpdateUI;

        restartButton.onClick.AddListener(() => RestartGame());

        // Hide all the pop up panels
        gameEndPanel.SetActive(false);
	}

    private void UpdateUI()
    {
        turnText.text = "" + World.Turns;

        if (World.GameWon)
        {
            endGameText.text = gameWon;
            gameEndPanel.SetActive(true);
        }
            
        if (World.GameLost)
        {
            endGameText.text = gameLost;
            gameEndPanel.SetActive(true);
        }
            
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

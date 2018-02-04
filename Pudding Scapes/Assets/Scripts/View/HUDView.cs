using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour {

    public Text turnText;

    public static HUDView instance;

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
	}

    private void UpdateUI()
    {
        turnText.text = "Turn: " + World.Turn;
    }
}

using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public void Awake(){
        Instance = this;
    }

    [SerializeField] private TMP_Text timerText;

    [SerializeField] private TMP_Text winLoseText;
    [SerializeField] private TMP_Text timeTakenText;
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject winMenuCanvas;


    public float timeElapsed{
        get{
            return _timeElapsed;
        }
        set{
            _timeElapsed = value;
            timerText.text = Math.Round(value, 2).ToString();
        }
    }

    private float _timeElapsed;
    // Start is called before the first frame update
    void Start() {
        timerText.text = "0.0";
        timeElapsed = 0f;

        EnableWinMenu(false);
    }

    public void EnableWinMenu(bool on){
        hudCanvas.SetActive(!on);
        winMenuCanvas.SetActive(on);
    }

    public void WinLoseGame(bool win){
        Debug.Log("Win");
        EnableWinMenu(true);

        winLoseText.text = win ? "YOU WON !!!!" : "YOU LOST";
        timeTakenText.text = timeElapsed.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
    }
}

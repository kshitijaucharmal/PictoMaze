using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int breakwalls = 0 ;
    [SerializeField] private int floatground = 0;

    [SerializeField] private TMP_Text raudraCountText;
    [SerializeField] private TMP_Text shantaCountText;

    [SerializeField] private Button raudraButton;
    [SerializeField] private Button shantaButton;

    public static PlayerStats Instance;
    private BallController Player;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        breakwalls = 0; floatground = 0 ;
        raudraButton.interactable = false;
        shantaButton.interactable = false;
    }

    public bool CanUseBreakPowerup() { return breakwalls > 0 && !GameObject.FindWithTag("Player").GetComponent<BallController>().IsBreaking(); }
    public bool CanUseFloatPowerup() { return floatground > 0 && !GameObject.FindWithTag("Player").GetComponent<BallController>().IsFloating(); }
    public void DecreaseBreakPowerupCount() { 
        breakwalls--;
        raudraCountText.text = breakwalls.ToString();
        if(breakwalls <= 0){
            raudraButton.interactable = false;
        }
    }
    public void DecreaseFloatPowerupCount() { 
        floatground--; 
        shantaCountText.text = floatground.ToString();
        if(floatground <= 0){
            shantaButton.interactable = false;
        }
    }
    public void AddBreakPowerup(int count = 1)
    {
        breakwalls += count;
        raudraButton.interactable = true;
        raudraCountText.text = breakwalls.ToString();
    }

    public void AddFloatPowerup(int count = 1)
    {
        floatground+= count;
        shantaButton.interactable = true;
        shantaCountText.text = floatground.ToString();
    }

}

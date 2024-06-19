using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int breakwalls = 0 ;
    [SerializeField] private int floatground = 0;

    [SerializeField] private TMP_Text raudraCountText;
    [SerializeField] private TMP_Text shantaCountText;

    public static PlayerStats Instance;
    private BallController Player;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        breakwalls = 0; floatground = 0 ;
    }

    public bool CanUseBreakPowerup() { return breakwalls > 0 && !GameObject.FindWithTag("Player").GetComponent<BallController>().IsBreaking(); }
    public bool CanUseFloatPowerup() { return floatground > 0 && !GameObject.FindWithTag("Player").GetComponent<BallController>().IsFloating(); }
    public void DecreaseBreakPowerupCount() { breakwalls--; }
    public void DecreaseFloatPowerupCount() { floatground--; }
    public void AddBreakPowerup(int count = 1)
    {
        breakwalls += count;
        raudraCountText.text = breakwalls.ToString();
    }

    public void AddFloatPowerup(int count = 1)
    {
        floatground+= count;
        shantaCountText.text = floatground.ToString();
    }

}

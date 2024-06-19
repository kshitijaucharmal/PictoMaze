using UnityEngine;

public class BreakPowerup : MonoBehaviour,IPowerup {
    public void Collect(){
        transform.parent.gameObject.SetActive(false);
        PlayerStats.Instance.AddBreakPowerup();
    }
    public static void Use() {
        if(!PlayerStats.Instance.CanUseBreakPowerup()) return;
        Debug.Log("Breaking");
        GameObject.FindWithTag("Player").GetComponent<BallController>().StartBreak();
        PlayerStats.Instance.DecreaseBreakPowerupCount();
    }
}

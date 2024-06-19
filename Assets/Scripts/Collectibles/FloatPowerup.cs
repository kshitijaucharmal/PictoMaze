using UnityEngine;

public class FloatPowerup : MonoBehaviour,IPowerup {
    public void Collect() {
        transform.parent.gameObject.SetActive(false);
        PlayerStats.Instance.AddFloatPowerup();
    }
    public static void Use() {
        if(!PlayerStats.Instance.CanUseFloatPowerup()) return;
        Debug.Log("Floating");
        GameObject.FindWithTag("Player").GetComponent<BallController>().StartFloat();
        PlayerStats.Instance.DecreaseFloatPowerupCount();
    }
}
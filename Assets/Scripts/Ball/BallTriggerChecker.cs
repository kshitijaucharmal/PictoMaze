using UnityEngine;

public class BallTriggerChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.W)){
        //     GameManager.Instance.WinLoseGame(true);
        // }
        // if(Input.GetKeyDown(KeyCode.L)){
        //     GameManager.Instance.WinLoseGame(false);
        // }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CellTrigger")
        {
            transform.parent.GetComponent<BallController>().SetCurrentStandingCell(other.transform.parent.gameObject);
        }
        if(other.gameObject.tag == "Powerup")
        {
            other.transform.GetChild(0).GetComponent<IPowerup>().Collect();
        }
        if(other.gameObject.tag == "DieTrigger") {
            // EditorApplication.isPlaying = false;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.WinLoseGame(false);
        }

        if (other.gameObject.tag == "WonTrigger")
        {
            // EditorApplication.isPlaying = false;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.WinLoseGame(true);
        }
    }
}

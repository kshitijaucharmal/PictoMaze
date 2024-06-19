using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallTriggerChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            Debug.Log("Dead");
            // EditorApplication.isPlaying = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (other.gameObject.tag == "WonTrigger")
        {
            Debug.Log("Won");
            // EditorApplication.isPlaying = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

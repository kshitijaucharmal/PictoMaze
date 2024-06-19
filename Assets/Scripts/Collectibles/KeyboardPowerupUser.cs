using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyboardPowerupUser : MonoBehaviour
{
    [SerializeField]private List<GameObject> endgoalPathList;
    [SerializeField] private List<GameObject> highlightedPath;

    void Start()
    {
        MazeGenerator.instance.OnPathGenerated += SetGoalPath;
    }

    private void SetGoalPath(object sender, List<GameObject> e)
    {
        endgoalPathList = e;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && PlayerStats.Instance.CanUseFloatPowerup()) {
            FloatPowerup.Use();
        }
        if (Input.GetKeyDown(KeyCode.G) && PlayerStats.Instance.CanUseBreakPowerup())
        {
            BreakPowerup.Use();
        }

    }

    private void HighlightPath(GameObject currCell)
    {
        int i = endgoalPathList.IndexOf(currCell) + 1;
        List<GameObject>highlightedObjects = new List<GameObject>();
        int maxIndex = i + 5;
        while (i < endgoalPathList.Count || i < maxIndex)
        {
            highlightedObjects.Add(endgoalPathList[i]);
        }
        highlightedPath = highlightedObjects;
        foreach (GameObject obj in highlightedObjects) {
            highlightedObjects[i].GetComponent<MazeCell>().SetHighlight();
        }
    }
}

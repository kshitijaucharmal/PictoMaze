using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{

    [SerializeField] private GameObject breakPowerup;
    [SerializeField] private GameObject floatPowerup;
    private void Start()
    {
        MazeGenerator.instance.OnMazeGenerated += SpawnPowerups;
    }

    private void SpawnPowerups(object sender, System.EventArgs e)
    {
        List<GameObject> powerupLocations = MazeGenerator.instance.GetBacktrackedCells();
        int n = powerupLocations.Count/2;
        for(int i=0;i<n;i++) {
            powerupLocations.Remove(powerupLocations[UnityEngine.Random.Range(0, powerupLocations.Count)]);
        }
        foreach(GameObject powerupLocation in powerupLocations)
        {
            GameObject powerup = powerupLocation.GetComponent<MazeCell>().GetPowerup();
            Vector2 coordinates = MazeGenerator.instance.GetGridCoordinates(powerupLocation.GetComponent<MazeCell>());
            if (coordinates.x > MazeGenerator.instance.GetGridDimensions().x / 2)
            {
                GameObject powerupSubObject = Instantiate(floatPowerup,powerup.transform);
            }
            else
            {
                GameObject powerupSubObject = Instantiate(breakPowerup, powerup.transform);
            }
            powerup.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

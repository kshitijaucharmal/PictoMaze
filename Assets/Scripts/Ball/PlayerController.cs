using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MazeGenerator mazeGenerator;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private bool keyboardTest=false;
    private GameObject ball=null;

    private void Awake(){
        if (SystemInfo.supportsGyroscope){Input.gyro.enabled = true;}
    }

    private void SpawnBall(object sender, EventArgs e)
    {
        
        ball = Instantiate(ballPrefab);
        ball.transform.parent = transform;
        ball.transform.position = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
        if (keyboardTest) { if(ball.TryGetComponent(out BallController ballController)){ ballController.SetKeyboardTest(); } }
    }

    void Start()
    {
        mazeGenerator = MazeGenerator.instance;
        mazeGenerator.OnMazeGenerated += SpawnBall;
    }
}

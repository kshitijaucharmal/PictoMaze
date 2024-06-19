using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float ForceModifier = 30f;
    [SerializeField] private GameObject currentStandingCell;
    private Rigidbody rb;
    private IA_Maze mazeInputActions;
    private Vector3 gyroMover;
    private Vector3 adjustedMover;
    private bool isFloating;
    private bool isBreaking;
    private bool keyboardTest=false;
    private void Awake()
    {
        isFloating = false;
        isBreaking = false;
        currentStandingCell = null;
        rb = ball.GetComponent<Rigidbody>();
        mazeInputActions = new IA_Maze();
        mazeInputActions.InGame.Enable();
        mazeInputActions.InGame.MoveAction.Enable();
        mazeInputActions.InGame.GyroMoveAction.Enable();
    }
    public void SetKeyboardTest()
    {
        keyboardTest = true;
    }
    void Start()
    {

    }

    public void StartFloat()
    {
        StartCoroutine(FloatCoroutine());
    }

    public void StartBreak()
    {
        StartCoroutine(BreakWallsCoroutine());
    }

    private IEnumerator BreakWallsCoroutine()
    {
        isBreaking = true;
        yield return new WaitForSeconds(1f);
        Vector2 coords = MazeGenerator.instance.GetGridCoordinates(currentStandingCell.GetComponent<MazeCell>());
        BreakWall(coords.x, coords.y);
        BreakWall(coords.x + 1, coords.y);
        BreakWall(coords.x - 1, coords.y);
        BreakWall(coords.x, coords.y + 1);
        BreakWall(coords.x, coords.y - 1);
        yield return new WaitForSeconds(1f);
        BreakWall(coords.x + 1, coords.y + 1);
        BreakWall(coords.x + 1, coords.y - 1);
        BreakWall(coords.x - 1, coords.y + 1);
        BreakWall(coords.x - 1, coords.y - 1);
        isBreaking = false;
    }

    private void BreakWall(float i,float j)
    {
        Vector2 dimensions = MazeGenerator.instance.GetGridDimensions();

        if (i >= 0 && i < dimensions.x && j >= 0 && j < dimensions.y)
        { 
            MazeGenerator.instance.GetCell((int)i, (int)j).gameObject.SetActive(false);
        }
    }

    public bool IsFloating() { return isFloating; }
    public bool IsBreaking() { return isBreaking; }

    private IEnumerator FloatCoroutine()
    {
        if(isFloating) { yield break; } 
        isFloating = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        // transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        rb.useGravity = false;
        yield return new WaitForSeconds(2f);
        // transform.localScale = new Vector3(1, 1, 1);
        rb.useGravity = true;
        isFloating = false;
    }
    private void CloneBallShowPath(){

    }


    void Update()
    {
        Vector3 moveDir;
        if (!keyboardTest) { 
            Quaternion deviceRotation = Input.gyro.attitude;
            Quaternion rot = new Quaternion(0, 0, -1, 0);
            Quaternion correctedRotation =  deviceRotation * rot;
            gyroMover = correctedRotation.eulerAngles;
            adjustedMover = Adjustgyro(gyroMover);
            moveDir = adjustedMover.x * Vector3.right + adjustedMover.y * Vector3.forward;
            rb.AddForce(moveDir.normalized * ForceModifier);
        }
        else {
            Vector2 mover = (mazeInputActions.InGame.MoveAction.ReadValue<Vector2>().normalized);
            moveDir = mover.x * Vector3.right + mover.y * Vector3.forward;
            rb.AddForce(moveDir.normalized * ForceModifier/10);
        }
    }
    private Vector2 Adjustgyro(Vector3 gyroMover)
    {
        Vector2 res = new Vector2(gyroMover.x, gyroMover.y);
        if (gyroMover.x > 0)
        {
            res.x = 100;
            if (gyroMover.x > 180)
            {
                res.x = -100;
            }
        }
        if (gyroMover.y > 0)
        {
            res.y = 100;
            if (gyroMover.y > 180)
            {
                res.y = -100;
            }
        }
        return res;
    }

    

    public GameObject GetCurrentStandingCell() { return currentStandingCell; }
    public void SetCurrentStandingCell(GameObject cell) { currentStandingCell = cell; }
}

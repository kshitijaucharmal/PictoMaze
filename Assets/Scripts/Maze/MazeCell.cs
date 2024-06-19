using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeCell : MonoBehaviour
{

    [SerializeField] private GameObject powerup;
    [SerializeField] private GameObject highlightCell;
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _upWall;
    [SerializeField] private GameObject _downWall;
    [SerializeField] private GameObject _unvisibleBox;
    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private BoxCollider groundCollider;
    [SerializeField] private MeshRenderer groundMeshRenderer;
    [SerializeField] private GameObject entryExitTrigger;


    [SerializeField] public bool IsVisited {  get; private set; }
    public void Visit() { IsVisited=true; _unvisibleBox.SetActive(false); }
    public void ClearLeft() { _leftWall.SetActive(false); }
    public void ClearRight() { _rightWall.SetActive(false); }
    public void ClearUp() { _upWall.SetActive(false); }
    public void ClearDown() { _downWall.SetActive(false); }
    public void SetMaterial(Material mat) { 
        foreach (MeshRenderer meshrenderer in _meshRenderers) {
            meshrenderer.material=mat;
        } 
    }
    public void SpawnPowerup()
    {
        powerup.SetActive(true);
    }

    public GameObject GetPowerup() { return powerup; }

    public void KillPowerup()
    {
        powerup.SetActive(false);

    }
    public void SetGroundMaterial(Material mat)
    {
        groundMeshRenderer.material=mat;
    }

    public void SetGroundPhysicsAsset(PhysicMaterial mat)
    {
        groundCollider.material=mat;
    }

    public void SetHighlight()
    {
        StartCoroutine(HighlightCell());
    }

    IEnumerator HighlightCell()
    {
        highlightCell.SetActive(true);
        yield return new WaitForSeconds(5f);
        highlightCell.SetActive(false);
    }
    public GameObject GetEntryExitTrigger() { return entryExitTrigger; }
}

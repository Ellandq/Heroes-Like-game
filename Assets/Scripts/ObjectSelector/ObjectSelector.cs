using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] GameObject armyHighlight;
    [SerializeField] Camera playerCamera;
    [SerializeField] InputManager inputManager;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] GameGrid gameGrid;
    [SerializeField] LayerMask layersToHit;
    
    [Header("Object selection information")]
    [SerializeField] GameObject lastObjectSelected;
    [SerializeField] bool objectSelected = false;
    [SerializeField] public GameObject selectedObject;
    [SerializeField] public string selectedObjectTag;

    void Start ()
    {
        TurnManager.OnNewPlayerTurn += ClearSelection;
        gameGrid = FindObjectOfType<GameGrid>();
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update ()
    {

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200, layersToHit)){
            selectedObject = hit.transform.gameObject;
            selectedObjectTag = selectedObject.tag;
        }else{
            selectedObject = null;
            selectedObjectTag = null;
        }

        if (inputManager.mouseInput.mouseButtonPressed_0)
        {
            if (objectSelected){
                if(lastObjectSelected.tag == "Army")
                {
                    if (lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().isMoving){
                    lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().StopMoving();
                    return;
                    }
                }
            }
            switch (selectedObjectTag)
            {
                case "Army":
                    ArmySelectionLogic();
                break;

                case "City":
                    CitySelectionLogic();
                    
                break;

                case "CityEnterance":
                    CityEnteranceSelectionLogic();
                break;

                case "Mine":
                    MineSelectionLogic();
                break;

                case "MineEnterance":
                    MineEnteranceSelectionLogic();
                break;

                case "Building":
                    BuildingSelectionLogic();
                break;

                case "BuildingEnterance":
                    BuildingEnteranceSelectionLogic();
                break;

                case "Dwelling":
                    DwellingSelectionLogic();
                break;

                case "DwellingEnterance":
                    DwellingEnteranceSelectionLogic();
                break;

                case "Resource":
                    ResourceSelectionLogic();
                break;

                case "Artifact":
                    ArtifactSelectionLogic();
                break;

                case "GridCell":
                    GridCellSelectionLogic();
                break;

                default:
                    selectedObject = null;
                    selectedObjectTag = null;
                break;
            }
        }
    }

    public void removeHighlight (Player _player)
    {
        selectedObject = null;
        selectedObjectTag = null;
        armyHighlight.SetActive(false);
    }

    public void addSelectedObject (GameObject _selectedObject)
    {
        lastObjectSelected = _selectedObject;
        objectSelected = true;
        cameraManager.cameraMovement.CameraAddObjectToFollow(_selectedObject);
    }

    public void removeSelectedObject ()
    {
        lastObjectSelected = null;
        objectSelected = false;
    }

    private void ClearSelection (Player _player)
    {
        removeHighlight(_player);
        lastObjectSelected = null;
        objectSelected = false;
    }

    private void ArmySelectionLogic()
    {
        if (selectedObject.GetComponentInParent<Army>().canBeSelectedByCurrentPlayer)
        {
            if (lastObjectSelected == null){
                addSelectedObject(selectedObject);

                if (!armyHighlight.activeSelf){
                    armyHighlight.SetActive(true);
                }
            armyHighlight.GetComponent<ArmyHighlight>().SetHighlitedObject(selectedObject);
            }else{
                if (lastObjectSelected == selectedObject){
                    Debug.Log("Do stuff with this army.");
                    cameraManager.cameraMovement.CameraAddObjectToFollow(selectedObject);
                }else {
                    lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.position);
                }
            }     
        }   
        else if(lastObjectSelected != null && lastObjectSelected.tag == "Army")
        {   
            lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.position);
        }
    }

    private void CitySelectionLogic()
    {
        if (!selectedObject){
            if (selectedObject.GetComponent<City>().canBeSelectedByCurrentPlayer){
                addSelectedObject(selectedObject);
            }else{
                armyHighlight.SetActive(false);
                removeSelectedObject();
            }
        }
    }

    private void CityEnteranceSelectionLogic()
    {
        if (lastObjectSelected != null){
        if (lastObjectSelected.tag == "Army"){
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.TransformPoint(Vector3.zero));
            }
        }else {
            if (selectedObject.GetComponentInParent<City>().canBeSelectedByCurrentPlayer){
                addSelectedObject(selectedObject);
            }else{
                removeSelectedObject();
            }
            armyHighlight.SetActive(false);
        }
    }

    private void MineSelectionLogic()
    {
        
    }

    private void MineEnteranceSelectionLogic()
    {
        if (lastObjectSelected != null){
            if (lastObjectSelected.tag == "Army"){
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.TransformPoint(Vector3.zero));
            }
        }
    }

    private void BuildingSelectionLogic()
    {
        
    }

    private void BuildingEnteranceSelectionLogic()
    {
        if (lastObjectSelected != null){
            if (lastObjectSelected.tag == "Army"){
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.TransformPoint(Vector3.zero));
            }
        }
    }

    private void DwellingSelectionLogic()
    {
        
    }

    private void DwellingEnteranceSelectionLogic()
    {
        if (lastObjectSelected != null){
            if (lastObjectSelected.tag == "Army"){
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.TransformPoint(Vector3.zero));
            }
        }
    }

    private void ResourceSelectionLogic()
    {
        if (lastObjectSelected != null){
            if (lastObjectSelected.tag == "Army"){
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.TransformPoint(Vector3.zero));
            }
        }
    }

    private void ArtifactSelectionLogic()
    {

    }

    private void GridCellSelectionLogic()
    {
        if (lastObjectSelected != null){
            if (lastObjectSelected.tag == "Army"){
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().HandleMovement(selectedObject.transform.position);
            }
        }else {
            removeSelectedObject();
            armyHighlight.SetActive(false);
        }
    }
}
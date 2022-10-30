using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector Instance;
    [SerializeField] GameObject armyHighlight;

    [Header ("Camera referances: ")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera uiCamera;

    [Header("Object referances: ")]
    [SerializeField] InputManager inputManager;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] GameGrid gameGrid;

    [Header("Raycast layers: ")]
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LayerMask uiLayers;
    
    [Header("Object selection information")]
    [SerializeField] public GameObject lastObjectSelected;
    [SerializeField] public bool objectSelected = false;
    [SerializeField] public GameObject selectedObject;
    [SerializeField] public string selectedObjectTag;

    void Start ()
    {
        Instance = this;
        TurnManager.OnNewPlayerTurn += ClearSelection;
        gameGrid = FindObjectOfType<GameGrid>();
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update ()
    {
        if (inputManager.mouseInput.IsMouseOverUI()) return;
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

    public void AddSelectedObject (GameObject _selectedObject)
    {
        lastObjectSelected = _selectedObject;
        objectSelected = true;
        cameraManager.cameraMovement.CameraAddObjectToFollow(_selectedObject);
    }

    public void RemoveSelectedObject ()
    {
        if (lastObjectSelected != null && lastObjectSelected.tag == "Army"){
            try{
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().StopMoving();
            }catch(NullReferenceException){} 
        }
        if (armyHighlight.activeSelf){
            armyHighlight.SetActive(false);
        }
        lastObjectSelected = null;
        objectSelected = false;
    }

    private void ClearSelection (Player _player)
    {
        lastObjectSelected = null;
        objectSelected = false;
    }

    private void ArmySelectionLogic()
    {
        if (selectedObject.GetComponentInParent<Army>().canBeSelectedByCurrentPlayer)
        {
            if (lastObjectSelected == null || lastObjectSelected.tag != "Army"){
                AddSelectedObject(selectedObject);

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

    public void ArmySelectionLogic(GameObject army)
    {
        selectedObject = army;
        if (selectedObject.GetComponentInParent<Army>().canBeSelectedByCurrentPlayer)
        {
            if (lastObjectSelected == null){
                AddSelectedObject(selectedObject);

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
        if (!objectSelected){
            if (selectedObject.GetComponentInParent<City>().canBeSelectedByCurrentPlayer){
                AddSelectedObject(selectedObject);
            }else{
                armyHighlight.SetActive(false);
                RemoveSelectedObject();
            }
        }else if (lastObjectSelected == selectedObject && lastObjectSelected.GetComponentInParent<City>().canBeSelectedByCurrentPlayer){
            lastObjectSelected.GetComponent<City>().CityInteraction();
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
                AddSelectedObject(selectedObject);
            }else{
                RemoveSelectedObject();
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
            RemoveSelectedObject();
            armyHighlight.SetActive(false);
        }
    }
}
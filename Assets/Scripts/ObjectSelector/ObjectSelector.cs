using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector Instance;

    [Header ("Events")]
    public UnityEvent onSelectedObjectChange;

    [Header ("Camera referances: ")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera uiCamera;

    [Header("Raycast layers: ")]
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private LayerMask uiLayers;
    
    [Header("Object selection information")]
    private Dictionary<PlayerTag, WorldObject> playerObjectDictionary;
    private WorldObject selectedObject;
    private PlayerTag currentPlayer;

    private bool isSelectorActive;

    private void Awake () { Instance = this; }

    private void Start () {
        playerObjectDictionary = InitializePlayerObjectDictionary();
        TurnManager.OnNewPlayerTurn += SetUpSelectorForNewPlayer;
        InputManager.Instance.mouseInput.OnLeftMouseButtonDown += CheckForObject;
        GameManager.Instance.StartGame();
    }

    private Dictionary<PlayerTag, WorldObject> InitializePlayerObjectDictionary()
    {
        var dictionary = new Dictionary<PlayerTag, WorldObject>();
        
        foreach (PlayerTag tag in Enum.GetValues(typeof(PlayerTag))){
            dictionary[tag] = null;
        }
        
        return dictionary;
    }

    private void CheckForObject (){
        if (InputManager.Instance.mouseInput.IsMouseOverUI()) return;
        GameObject obj = InputManager.Instance.mouseInput.GetMouseOverWorldObject(layersToHit);
        if (obj != null) SelectObject(obj);
        else{
            selectedObject = null;
        }
    }

    private void SelectObject (GameObject obj)
    {
        switch (obj.tag)
        {
            case "WorldObject":
                HandleWorldObjects(obj.GetComponentInParent<WorldObject>());
            break;

            case "Enterance":
                HandleEnterance(obj.GetComponent<ObjectEnterance>());
            break;

            case "GridCell":
                HandleGridCells(obj.GetComponent<GridCell>());
            break;

            default:
                selectedObject = null;
            break;
        }
         
    }

    public void HandleWorldObjects (WorldObject obj){
        if (obj is City){
            HandleCity(obj as City);
        }
        else if (obj is Army){
            HandleArmy(obj as Army);
        }
        else if (obj is Mine){
            HandleMine(obj as Mine);
        }
        else if (obj is ResourcesObject){
            HandleResourceObject(obj as ResourcesObject);
        }
    }

    private void HandleCity (City city){
        if (selectedObject == null){
            city.ObjectSelected();
            HandleSelectionChange(city);
        }
    }

    private void HandleArmy (Army army){
        if (IsSelectedObjectArmy()){
            if (selectedObject == army) army.ObjectSelected();
            else {
                if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
                (selectedObject as Army).Move(army.transform.position);
            }
        }else {
            HandleSelectionChange(army);
        }
    }

    private void HandleMine (Mine mine){
        mine.ObjectSelected();
    }

    private void HandleResourceObject(ResourcesObject rObj){
        if (IsSelectedObjectArmy()){
            if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
            (selectedObject as Army).Move(rObj.transform.position);
        }
    }

    private void HandleEnterance (ObjectEnterance enterance){
        if (IsSelectedObjectArmy()){
            if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
            (selectedObject as Army).Move(enterance.transform.position);
        }
    }

    private void HandleGridCells (GridCell cell){
        if (IsSelectedObjectArmy()){
            if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
            (selectedObject as Army).Move(cell.transform.position);
        }
    }

    private void HandleSelectionChange (WorldObject obj){
        if (selectedObject != null){
            obj.ObjectDeselected();
        }
        selectedObject = obj;
        onSelectedObjectChange.Invoke();
    }

    private void SetUpSelectorForNewPlayer (PlayerTag tag){
        playerObjectDictionary[currentPlayer] = selectedObject;
        if (playerObjectDictionary[tag] == null){
            SelectObject()
        }
    }

    private bool IsSelectedObjectArmy (){
        return selectedObject != null && selectedObject is Army;
    }

    // FROM THIS POINT THE METHODS ARE OBSOLETE AND NEED TO BE CHANGED TO WORK WITH THE NEW SYSTEM


    // Clears the selection and refreshes UI components on every new turn
    private void ClearSelection (Player _player)
    {
        lastObjectSelected = null;
        objectSelected = false;
        if (armyHighlight.activeSelf){
            armyHighlight.SetActive(false);
        }
        onSelectedObjectChange.Invoke();
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
                    lastObjectSelected.GetComponentInParent<Army>().ArmyInteraction();
                    CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(selectedObject);
                }else {
                    lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.position);
                }
            }     
        }   
        else if(lastObjectSelected != null && lastObjectSelected.tag == "Army")
        {   
            lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.position);
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
                    CameraManager.Instance.cameraMovement.CameraAddObjectToFollow(selectedObject);
                }else {
                    lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.position);
                }
            }     
        }   
        else if(lastObjectSelected != null && lastObjectSelected.tag == "Army")
        {   
            lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.position);
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
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.TransformPoint(Vector3.zero));
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
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.TransformPoint(Vector3.zero));
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
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.TransformPoint(Vector3.zero));
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
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.TransformPoint(Vector3.zero));
            }
        }
    }

    private void ResourceSelectionLogic()
    {
        if (lastObjectSelected != null){
            if (lastObjectSelected.tag == "Army"){
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.TransformPoint(Vector3.zero));
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
                lastObjectSelected.GetComponentInParent<CharacterPathFindingMovementHandler>().Move(selectedObject.transform.position);
            }
        }else {
            RemoveSelectedObject();
            armyHighlight.SetActive(false);
        }
    }
}
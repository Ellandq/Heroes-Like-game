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

    [Header("Raycast layers: ")]
    [SerializeField] private LayerMask layersToHit;
    
    [Header("Object selection information")]
    private Dictionary<PlayerTag, WorldObject> playerObjectDictionary;
    [SerializeField] private WorldObject selectedObject;
    private PlayerTag currentPlayer;

    private bool isSelectorActive;

    private void Awake () { Instance = this; }

    public void SetupObjectSelector () {
        isSelectorActive = true;
        playerObjectDictionary = InitializePlayerObjectDictionary();
        SetUpSelectorForNewPlayer();
        TurnManager.Instance.OnNewDay += SetUpSelectorForNewPlayer;
        InputManager.Instance.mouseInput.OnLeftMouseButtonDown += CheckForObject;
    }

    private Dictionary<PlayerTag, WorldObject> InitializePlayerObjectDictionary()
    {
        var dictionary = new Dictionary<PlayerTag, WorldObject>();
        
        foreach (PlayerTag tag in GameManager.Instance.playerTags){
            dictionary[tag] = PlayerManager.Instance.GetPlayer(tag).GetObjectToSelect();
        }
        
        return dictionary;
    }

    private void CheckForObject (){
        if (InputManager.Instance.mouseInput.IsMouseOverUI() || !isSelectorActive) return;
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

    public void HandleWorldObjects (WorldObject obj, bool forceSelection = false){
        if (obj is City){
            HandleCity(obj as City, forceSelection);
        }
        else if (obj is Army){
            HandleArmy(obj as Army, forceSelection);
        }
        else if (obj is Mine){
            HandleMine(obj as Mine);
        }
        else if (obj is ResourcesObject){
            HandleResourceObject(obj as ResourcesObject);
        }
    }

    private void HandleCity (City city, bool forceSelection = false){
        if (selectedObject == null){
            city.ObjectSelected();
            HandleSelectionChange(city);
        }else if (forceSelection){
            city.ObjectSelected();
            HandleSelectionChange(city);
        }
    }

    private void HandleArmy (Army army, bool forceSelection = false){
        if (IsSelectedObjectArmy()){
            if (selectedObject == army) army.ObjectSelected();
            else if (!forceSelection){
                if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
                (selectedObject as Army).Move(army.transform.position);
            }else{
                HandleSelectionChange(army);
            }
        }else {
            HandleSelectionChange(army);
        }
    }

    private void HandleMine (Mine mine){
        if (IsSelectedObjectArmy() && (selectedObject as Army).IsMoving()){
            (selectedObject as Army).Stop();
        }
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
            (selectedObject as Army).Move(enterance);
        }
    }

    private void HandleGridCells (GridCell cell){
        if (IsSelectedObjectArmy()){
            if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
            (selectedObject as Army).Move(cell.transform.position);
        }
    }

    private void HandleSelectionChange (WorldObject obj){
        if (obj == null) return;
        if (selectedObject != null){
            selectedObject.ObjectDeselected();
        }
        selectedObject = obj;
        obj.ObjectSelected();
        onSelectedObjectChange.Invoke();
    }

    public void CancelSelection (WorldObject obj){
        if (obj != null && obj == selectedObject){
            HandleSelectionChange(null);
        }else if (selectedObject != null){
            selectedObject.ObjectDeselected();
        }
    }

    private void SetUpSelectorForNewPlayer (){
        PlayerTag tag = PlayerManager.Instance.GetCurrentPlayerTag();
        playerObjectDictionary[currentPlayer] = selectedObject;
        if (playerObjectDictionary[tag] == null){
            playerObjectDictionary[tag] = PlayerManager.Instance.GetPlayer(tag).GetObjectToSelect();
        }
        HandleWorldObjects(playerObjectDictionary[tag]);
    }

    private bool IsSelectedObjectArmy (){
        return selectedObject != null && selectedObject is Army;
    }

    private bool IsSelectedObjectCity (){
        return selectedObject != null && selectedObject is City;
    }

    public WorldObject GetObject (PlayerTag tag){ return playerObjectDictionary[tag]; }

    public WorldObject GetSelectedObject () { return selectedObject; }

    public Army GetSelectedArmy (){ return IsSelectedObjectArmy() ? selectedObject as Army : null; }

    public City GetSelectedCity (){ return IsSelectedObjectCity() ? selectedObject as City : null; }

    public UnitsInformation GetSelectedObjectUnits () { 
        if (selectedObject is Army)
        {
            return (selectedObject as Army).GetUnitsInformation();
        }
        else if (selectedObject is City)
        {
            return (selectedObject as City).GetUnitsInformation();
        }
        else
        {
            return null;
        }   
    }
}
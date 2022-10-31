using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Army : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] TurnManager turnManager;
    [SerializeField] GameGrid gameGrid;
    [SerializeField] GameObject flag;

    [Header("Army information")]
    [SerializeField] public GameObject ownedByPlayer;
    public Vector2Int gridPosition;
    private Vector3 position;
    private Vector3 rotation;
    [SerializeField] int maxMovementPoints;
    public int movementPoints;
    public bool canBeSelectedByCurrentPlayer;

    [Header("Unit slots references")]
    [SerializeField] GameObject unitSlot1;
    [SerializeField] GameObject unitSlot2;
    [SerializeField] GameObject unitSlot3;
    [SerializeField] GameObject unitSlot4;
    [SerializeField] GameObject unitSlot5;
    [SerializeField] GameObject unitSlot6;
    [SerializeField] GameObject unitSlot7;

    void Start ()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        gameGrid = GameObject.Find("GameGrid").GetComponent<GameGrid>();
        maxMovementPoints = 2200;
        movementPoints = maxMovementPoints;
        playerManager.OnNextPlayerTurn.AddListener(UpdateArmySelectionAvailability);
        turnManager.OnNewDay.AddListener(RestoreMovementPoints);
        ownedByPlayer.GetComponent<Player>().CheckPlayerStatus();
        GetComponentInChildren<ObjectInteraction>().ChangeObjectName(this.gameObject.name);
    }

    public void AddOwningPlayer(GameObject _ownedByPlayer)
    {
        ownedByPlayer = _ownedByPlayer;
        if (_ownedByPlayer.name != "Neutral Player"){
            flag.SetActive(true);
            flag.GetComponent<MeshRenderer>().material.color = _ownedByPlayer.GetComponent<Player>().playerColor;
        }
    }

    public void RemoveOwningPlayer ()
    {
        ownedByPlayer = playerManager.neutralPlayer;
        flag.SetActive(false);
    }

    public void ArmyInitialization (string _ownedByPLayer, Vector2Int _gridPosition, float _armyOrientation, int [] _armyUnits)
    {
        gridPosition = _gridPosition;
        rotation.y = _armyOrientation;
        transform.localEulerAngles = rotation;

        unitSlot1.GetComponent<UnitSlot>().ChangeSlotStatus(_armyUnits[0], _armyUnits[1]);
        unitSlot2.GetComponent<UnitSlot>().ChangeSlotStatus(_armyUnits[2], _armyUnits[3]);
        unitSlot3.GetComponent<UnitSlot>().ChangeSlotStatus(_armyUnits[4], _armyUnits[5]);
        unitSlot4.GetComponent<UnitSlot>().ChangeSlotStatus(_armyUnits[6], _armyUnits[7]);
        unitSlot5.GetComponent<UnitSlot>().ChangeSlotStatus(_armyUnits[8], _armyUnits[9]);
        unitSlot6.GetComponent<UnitSlot>().ChangeSlotStatus(_armyUnits[10], _armyUnits[11]);
        unitSlot7.GetComponent<UnitSlot>().ChangeSlotStatus(_armyUnits[12], _armyUnits[13]);
    }

    private void UpdateArmySelectionAvailability(Player _player)
    {
        if (_player.gameObject.name  == ownedByPlayer.name){
            canBeSelectedByCurrentPlayer = true;
        }else{
            canBeSelectedByCurrentPlayer = false;
        }
    }

    public void UpdateArmyGridPosition ()
    {
        gridPosition = gameGrid.GetGridPosFromWorld(this.gameObject.transform.position);
    }

    private void CheckMovementPoints()
    {

    }

    private void RestoreMovementPoints ()
    {
        movementPoints = maxMovementPoints;
    }

    public void RemoveMovementPoints(int _pathCost)
    {
        movementPoints -= _pathCost;
    }

    public bool CanArmyMoveToPathNode (int _nextPathCost)
    {
        if (movementPoints >= _nextPathCost) return true;
        else return false;
    }

    public void ArmyInteraction (GameObject interactingArmy)
    {
        
        if (interactingArmy.GetComponent<Army>().ownedByPlayer == ownedByPlayer){
            Debug.Log("Interacting army: " + interactingArmy.name);
        }else{
            Debug.Log("Do battle with: " + interactingArmy.name);
        }
    }

    public void ArmyInteraction ()
    {
        Debug.Log("Interacting with this army");
        
    }

    void OnDestroy ()
    {
        playerManager.OnNextPlayerTurn.RemoveListener(UpdateArmySelectionAvailability);
        turnManager.OnNewDay.RemoveListener(RestoreMovementPoints);
    }
}
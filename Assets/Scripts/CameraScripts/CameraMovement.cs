using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement: MonoBehaviour 
{
    [SerializeField]CameraManager cameraManager;
    [SerializeField] GameObject inputManager;
    [SerializeField] internal Transform cameraFollowObject;
    [SerializeField] GameObject objectToMoveTowards;
    MouseInput mouseInput;
    KeyboardInput keyboardInput;

    Coroutine cameraMoveToObject;
    
    private int screenWidth;
    private int screenHeight;

    [Header("Camera Options")]
    [SerializeField] int speed = 5;
    [SerializeField] int maxSpeed = 10;
    [SerializeField] int distanceFromBoundary = 50;
    [SerializeField] int minDistanceFromBoundary = 5;
    [SerializeField] internal Vector2 cameraMoveLimit;
    internal bool cameraFollowingObject;
    [SerializeField] float rotation;
    [SerializeField] float moveSpeed;
    internal Vector3 position;  
    private Vector3 cameraOffset; 
    private Vector3 adjustedMovementVector;

    void Awake()
    {
        mouseInput = inputManager.GetComponent<MouseInput>();
        keyboardInput = inputManager.GetComponent<KeyboardInput>();
    }
	void Start () 
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
	}
	
	void Update () 
    {
        if (cameraManager.cameraEnabled)
        {
            EdgeScrollingMovement();
            ManualCameraMovement();
        }
        if (cameraFollowingObject) CameraFollowWorldObject();
        
        position.x = Mathf.Clamp(position.x, 0, cameraMoveLimit.x);
        position.z = Mathf.Clamp(position.z, -20, cameraMoveLimit.y);

        transform.position = position;
        cameraFollowObject.transform.position = position;
        rotation = transform.localEulerAngles.y;
    }

    private void EdgeScrollingMovement()
    {
        position = transform.position;
        adjustedMovementVector = CalculateMovementAngle();

        if (mouseInput.screenPosition.x > screenWidth - minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on +X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (mouseInput.screenPosition.x > screenWidth - distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, adjustedMovementVector.z) * speed * Time.deltaTime); // move on +X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }

        if (mouseInput.screenPosition.x < 0 + minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on -X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (mouseInput.screenPosition.x < 0 + distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * speed * Time.deltaTime); // move on -X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }

        if (mouseInput.screenPosition.y > screenHeight - minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on +Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (mouseInput.screenPosition.y > screenHeight - distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * speed * Time.deltaTime); // move on +Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }

        if (mouseInput.screenPosition.y < 0 + minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on -Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (mouseInput.screenPosition.y < 0 + distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * speed * Time.deltaTime); // move on -Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        transform.position = position;
    }

    private void ManualCameraMovement()
    {
        position = transform.position;

        if(keyboardInput.isRightPressed){
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on +X axis
            position = cameraFollowObject.transform.position;
        }
        if(keyboardInput.isLeftPressed){
           cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on -X axis 
           position = cameraFollowObject.transform.position;
           cameraFollowingObject = false;
        }
        if(keyboardInput.isUpPressed){
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on +Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        if(keyboardInput.isDownPressed){
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on -Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        transform.position = position;  
    }

    public void CameraAddObjectToFollow (GameObject _objectToMoveTowards)
    {
        objectToMoveTowards = _objectToMoveTowards;
        cameraFollowingObject = true;
    }

    public void CameraFollowWorldObject ()
    {
        if (cameraFollowingObject)
        {
            cameraFollowObject.transform.position = Vector3.MoveTowards(cameraFollowObject.transform.position, (objectToMoveTowards.transform.position + CalculateCameraOffset()), moveSpeed);
            position = cameraFollowObject.transform.position;
            transform.position = cameraFollowObject.transform.position;
        }
    }

    internal Vector3 CalculateCameraOffset ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y - objectToMoveTowards.transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    internal Vector3 CalculateCameraOffsetIndependent ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    internal Vector3 CalculateMovementAngle ()
    {
        double y = System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(y), 0, Convert.ToSingle(x));
    }

    internal Vector3 CalculateCenterOfRotation()
    {
        return (transform.position - CalculateCameraOffsetIndependent());
    }
}


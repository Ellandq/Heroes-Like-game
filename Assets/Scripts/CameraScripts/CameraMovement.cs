using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement: MonoBehaviour 
{
    [Header ("Movement objects")]
    [SerializeField] internal Transform cameraFollowObject;
    [SerializeField] private GameObject objectToMoveTowards;

    [Header("Camera Options")]
    [SerializeField] private int minSpeed = 5;
    [SerializeField] private int maxSpeed = 10;
    [SerializeField] private int distanceFromBoundary = 50;
    [SerializeField] private int minDistanceFromBoundary = 5;
    [SerializeField] internal Vector2 cameraMoveLimit;
    [SerializeField] private float rotation;
    [SerializeField] private float moveSpeed;
    
    internal bool cameraFollowingObject;

    internal Vector3 position;  
    private Vector3 cameraOffset; 
    private Vector3 adjustedMovementVector;

    private int screenWidth;
    private int screenHeight;

	private void Start () 
    {
        // Set the scren size variables
        screenWidth = Screen.width;
        screenHeight = Screen.height;
	}
	
    // Check what type of movement if any is supposed to run every frame
	private void Update () 
    {
        if (CameraManager.Instance.cameraEnabled && !CameraManager.Instance.cameraRotating)
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

    // Movement based on edge scrolling
    private void EdgeScrollingMovement()
    {
        position = transform.position;
        adjustedMovementVector = CalculateMovementAngle();

        if (InputManager.Instance.mouseInput.screenPosition.x > screenWidth - minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on +X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (InputManager.Instance.mouseInput.screenPosition.x > screenWidth - distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, adjustedMovementVector.z) * minSpeed * Time.deltaTime); // move on +X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }

        if (InputManager.Instance.mouseInput.screenPosition.x < 0 + minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on -X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (InputManager.Instance.mouseInput.screenPosition.x < 0 + distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * minSpeed * Time.deltaTime); // move on -X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }

        if (InputManager.Instance.mouseInput.screenPosition.y > screenHeight - minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on +Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (InputManager.Instance.mouseInput.screenPosition.y > screenHeight - distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * minSpeed * Time.deltaTime); // move on +Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }

        if (InputManager.Instance.mouseInput.screenPosition.y < 0 + minDistanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on -Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        else if (InputManager.Instance.mouseInput.screenPosition.y < 0 + distanceFromBoundary) {
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * minSpeed * Time.deltaTime); // move on -Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        transform.position = position;
    }

    // Movement based on keyboard input
    private void ManualCameraMovement()
    {
        position = transform.position;

        if(InputManager.Instance.keyboardInput.isRightPressed){
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.x, 0, -adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on +X axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        if(InputManager.Instance.keyboardInput.isLeftPressed){
           cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.x, 0, adjustedMovementVector.z) * maxSpeed * Time.deltaTime); // move on -X axis 
           position = cameraFollowObject.transform.position;
           cameraFollowingObject = false;
        }
        if(InputManager.Instance.keyboardInput.isUpPressed){
            cameraFollowObject.transform.Translate(new Vector3(adjustedMovementVector.z, 0, adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on +Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        if(InputManager.Instance.keyboardInput.isDownPressed){
            cameraFollowObject.transform.Translate(new Vector3(-adjustedMovementVector.z, 0, -adjustedMovementVector.x) * maxSpeed * Time.deltaTime); // move on -Z axis
            position = cameraFollowObject.transform.position;
            cameraFollowingObject = false;
        }
        transform.position = position;  
    }

    // Adds an object for the camera to follow
    public void CameraAddObjectToFollow (GameObject _objectToMoveTowards)
    {
        objectToMoveTowards = _objectToMoveTowards;
        cameraFollowingObject = true;
    }

    // Removes the object 
    public void CameraRemoveObjectToFollow ()
    {
        objectToMoveTowards = null;
        cameraFollowingObject = false;
    }

    // Moves the camera towards the selected object
    public void CameraFollowWorldObject ()
    {
        cameraFollowObject.transform.position = Vector3.Lerp(cameraFollowObject.transform.position, (objectToMoveTowards.transform.position + CalculateCameraOffset()), moveSpeed/10 * Time.deltaTime);
        position = cameraFollowObject.transform.position;
        transform.position = cameraFollowObject.transform.position;
    }

    // Teleports the camera to the currently selected object
    public void CameraTeleportToWorldObject ()
    {
        cameraFollowingObject = false;
        cameraFollowObject.transform.position = (objectToMoveTowards.transform.position + CalculateCameraOffset());
        position = cameraFollowObject.transform.position;
        transform.position = cameraFollowObject.transform.position;
    }

    // Calculates the position based on the camera rotation, angle and the selected object position
    internal Vector3 CalculateCameraOffset ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y - objectToMoveTowards.transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Camera offset independent of any object 
    internal Vector3 CalculateCameraOffsetIndependent ()
    {
        double angle = transform.localEulerAngles.x;
        double offsetDistance = System.Math.Round(10000 * transform.position.y / Math.Tan(angle * Math.PI / 180)) / 10000;
        double y = -System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = -System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(offsetDistance * x), transform.position.y, Convert.ToSingle(offsetDistance * y));
    }

    // Calculates the Vector at which the camera is supposed to move to move forwards
    internal Vector3 CalculateMovementAngle ()
    {
        double y = System.Math.Round(10000 * Math.Cos(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        double x = System.Math.Round(10000 * Math.Sin(transform.localEulerAngles.y * Math.PI / 180)) / 10000;
        return new Vector3(Convert.ToSingle(y), 0, Convert.ToSingle(x));
    }

    // Returns the position around which the camera is supposed to rotate
    internal Vector3 CalculateCenterOfRotation()
    {
        return (transform.position - CalculateCameraOffsetIndependent());
    }
}


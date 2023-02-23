using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header ("Movement objects")]
    public Transform cameraZoomObject;
    

    [Header("Height Options")]
    [SerializeField] private float maxCameraHeight = 70f;
    [SerializeField] private float minCameraHeight = 22f;
    [SerializeField] private float zoomSpeed = 80f;
    [SerializeField] private float zoomPercentage;

    [Header("Zoom Options")]
    [SerializeField] private float maxCameraAngle = 90f;
    [SerializeField] private float minCameraAngle = 30f;
    [SerializeField] private float anglePercentage;

    [Header("Offset Options")]
    public float smoothSpeed = 0.125f;

    private Vector3 centerOfRotation;
    private float zoomObjectRotation;
    private bool centerCalculated;

    // On Awake calculates the current camera angle
    private void Awake() 
    {
        anglePercentage = (cameraZoomObject.transform.localEulerAngles.x - minCameraAngle) / (maxCameraAngle - minCameraAngle);
    }

    // Every frame checks if the requirements are met to rotate the camera
    private void Update() 
    {
        if (!CameraManager.Instance.cameraRotating)
        {
            if (!InputManager.Instance.mouseInput.IsMouseOverUI()){
                if (InputManager.Instance.mouseInput.mouseScrollStatus > 0f) // forward
                {
                    ZoomCamera();
                    CameraManager.Instance.cameraMovement.cameraFollowingObject = false;
                }
                if (InputManager.Instance.mouseInput.mouseScrollStatus < 0f) // backwards
                {
                    UnZoomCamera();   
                    CameraManager.Instance.cameraMovement.cameraFollowingObject = false;  
                }
            }
        }
        if (InputManager.Instance.mouseInput.mouseButtonPressed_2){
            CameraManager.Instance.cameraRotating = true;
            if (Cursor.lockState == CursorLockMode.None) Cursor.lockState = CursorLockMode.Locked;
            RotateCamera();

        }else{
            centerCalculated = false;
            CameraManager.Instance.cameraRotating = false;
            Cursor.lockState = CursorLockMode.None;
        }
        if (InputManager.Instance.keyboardInput.resetCameraPressed){
            ResetCamera();
        }
    }

    // Script to zoom in the camera
    private void ZoomCamera()
    {
        if (cameraZoomObject.transform.position.y > minCameraHeight | cameraZoomObject.transform.rotation.x > minCameraAngle)
        {
            Vector3 position = cameraZoomObject.transform.position;
            Vector3 rotation = cameraZoomObject.transform.localEulerAngles;

            // Rotation
            cameraZoomObject.transform.Rotate(-zoomSpeed * Time.deltaTime, 0, 0);
            rotation.x = Mathf.Clamp(cameraZoomObject.transform.localEulerAngles.x, minCameraAngle, maxCameraAngle);
            cameraZoomObject.transform.localEulerAngles = rotation;

            // Height
            position.y = Mathf.MoveTowards(position.y, CameraUpdatedHeight(), zoomSpeed * Time.deltaTime);
            position.y = Mathf.Clamp(position.y, minCameraHeight, maxCameraHeight);

            cameraZoomObject.transform.position = position;
        }
    }
 
    // Script to unzoom in the camera
    private void UnZoomCamera()
    {
        if (cameraZoomObject.transform.position.y < maxCameraHeight | cameraZoomObject.transform.rotation.x < maxCameraAngle)
        {
            Vector3 position = cameraZoomObject.transform.position;
            Vector3 rotation = cameraZoomObject.transform.localEulerAngles;

            // Rotation
            cameraZoomObject.transform.Rotate(zoomSpeed * Time.deltaTime, 0, 0);
            rotation.x = Mathf.Clamp(cameraZoomObject.transform.localEulerAngles.x, minCameraAngle, maxCameraAngle);
            cameraZoomObject.transform.localEulerAngles = rotation;

            // Height
            position.y = Mathf.MoveTowards(position.y, CameraUpdatedHeight(), zoomSpeed * Time.deltaTime);
            position.y = Mathf.Clamp(position.y, minCameraHeight, maxCameraHeight);

            cameraZoomObject.transform.position = position;
        }
    }

    // Rotates the camera according to the height and angle
    private void RotateCamera ()
    {
        Vector3 rotation = cameraZoomObject.transform.localEulerAngles;
        Vector3 position = CameraManager.Instance.cameraMovement.cameraFollowObject.position;
        float currentMousePosition = Input.GetAxis("Mouse X");

        if (!centerCalculated){
            centerOfRotation = CameraManager.Instance.cameraMovement.CalculateCenterOfRotation();
            centerCalculated = true;
        }
        
        rotation.y += currentMousePosition;
        cameraZoomObject.transform.localEulerAngles = rotation;
        position = CameraManager.Instance.cameraMovement.CalculateCameraOffsetIndependent() + centerOfRotation;

        position.x = Mathf.Clamp(position.x, 0, CameraManager.Instance.cameraMovement.cameraMoveLimit.x);
        position.z = Mathf.Clamp(position.z, -20, CameraManager.Instance.cameraMovement.cameraMoveLimit.y);
        CameraManager.Instance.cameraMovement.cameraFollowObject.position = Vector3.Lerp(CameraManager.Instance.cameraMovement.cameraFollowObject.position, position, zoomSpeed);
        transform.position = CameraManager.Instance.cameraMovement.cameraFollowObject.position;
        CameraManager.Instance.cameraMovement.position = transform.position;
    }

    // Returns the adjusted camera height
    private float CameraUpdatedHeight()
    {
        anglePercentage = (cameraZoomObject.transform.localEulerAngles.x - minCameraAngle) / (maxCameraAngle - minCameraAngle);
        return (((maxCameraHeight - minCameraHeight) * anglePercentage) + minCameraHeight);
    }

    // Resets the camera rotation to it's neutral state
    private void ResetCamera()
    {
        cameraZoomObject.transform.localEulerAngles = new Vector3 (cameraZoomObject.transform.localEulerAngles.x, 0, 0);
    }

    // Moves and rotates the camera to the adjusted position
    private void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, cameraZoomObject.transform.position, smoothSpeed);
        smoothedPosition.x = transform.position.x;
        smoothedPosition.z = transform.position.z;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, cameraZoomObject.transform.rotation, smoothSpeed);

        transform.position = smoothedPosition;
        transform.rotation = smoothedRotation;
    }
}
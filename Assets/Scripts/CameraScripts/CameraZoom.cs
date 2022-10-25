using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    CameraManager cameraManager;

    [SerializeField] GameObject inputManager;
    MouseOneInteraction mouseOneInteraction;
    MouseInput mouseInput;
    KeyboardInput keyboardInput;
    
    public Transform cameraZoomObject;

    private float zoomObjectRotation;
    

    [Header("Height Options")]
    [SerializeField]  float maxCameraHeight = 70f;
    [SerializeField]  float minCameraHeight = 22f;
    [SerializeField]  float zoomSpeed = 80f;
    [SerializeField] float zoomPercentage;

    [Header("Zoom Options")]
    [SerializeField]  float maxCameraAngle = 90f;
    [SerializeField]  float minCameraAngle = 30f;
    [SerializeField] float anglePercentage;

    [Header("Offset Options")]
    public float smoothSpeed = 0.125f;

    void Awake()
    {
        mouseOneInteraction = inputManager.GetComponent<MouseOneInteraction>();
        mouseInput = inputManager.GetComponent<MouseInput>();
        keyboardInput = inputManager.GetComponent<KeyboardInput>();

        anglePercentage = (cameraZoomObject.transform.localEulerAngles.x - minCameraAngle) / (maxCameraAngle - minCameraAngle);
    }

    void Update()
    {
        if (cameraManager.cameraEnabled)
        {
            if (mouseInput.mouseScrollStatus > 0f) // forward
            {
                ZoomCamera();
                cameraManager.cameraMovement.cameraFollowingObject = false;
            }
            if (mouseInput.mouseScrollStatus < 0f) // backwards
            {
                UnZoomCamera();   
                cameraManager.cameraMovement.cameraFollowingObject = false;  
            }
        }
        if (mouseInput.mouseButtonPressed_2){
            
        }
    }
    void ZoomCamera()
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
    void UnZoomCamera()
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

    private float CameraUpdatedHeight()
    {
        anglePercentage = (cameraZoomObject.transform.localEulerAngles.x - minCameraAngle) / (maxCameraAngle - minCameraAngle);
        return (((maxCameraHeight - minCameraHeight) * anglePercentage) + minCameraHeight);
    }

    void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, cameraZoomObject.transform.position, smoothSpeed);
        smoothedPosition.x = transform.position.x;
        smoothedPosition.z = transform.position.z;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, cameraZoomObject.transform.rotation, smoothSpeed);

        transform.position = smoothedPosition;
        transform.rotation = smoothedRotation;
    }
}
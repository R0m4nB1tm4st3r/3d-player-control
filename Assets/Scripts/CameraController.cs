using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    const string PlayerTag = "Player";
    const byte FollowTargetIndex = 0;

    [SerializeField, Range(6f, 18f)] float cameraDistance = 9f;
	[SerializeField, Range(1f, 5f)] float rotationSpeed = 2f;

	[SerializeField]GameObject player = null;
    [SerializeField]Transform followTarget = null;

	void Start()
    {
        player = GameObject.FindWithTag(PlayerTag);

        followTarget = player.transform.GetChild(FollowTargetIndex);
        Cursor.lockState = CursorLockMode.Locked;
	}

    void Update()
    {

    }

    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        followTarget.rotation *= 
            Quaternion.AngleAxis(callbackContext.ReadValue<Vector2>().x * Time.deltaTime * rotationSpeed, Vector3.up);
    }
}

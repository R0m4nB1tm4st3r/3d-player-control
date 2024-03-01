using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	const string GroundTag = "Ground";
	const byte FollowTargetIndex = 0;

	[field: SerializeField, Range(8f, 16f)] 
	public float MoveSpeed { get; set; } = 11f;
	[field: SerializeField, Range(8f, 16f)] 
	public float JumpStrength { get; set; } = 8f;

	bool isOnGround = false;
	bool isMoveInputPresent = false;
    Rigidbody rigidBody = null;
	Transform cameraFollowTarget = null;
	InputController inputController = null;

	void Start()
	{
		if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
		if (cameraFollowTarget == null) cameraFollowTarget = transform.GetChild(FollowTargetIndex);
		if (inputController == null) inputController = FindObjectOfType<InputController>();
	}

	void Update()
	{
		UpdateMovingState();
		Rotate();
	}

	void FixedUpdate()
	{
		Move();
		Jump();
	}

	void UpdateMovingState()
	{
		isMoveInputPresent = inputController.MoveDelta != Vector3.zero;
	}

	void Move()
	{
		if (isMoveInputPresent)
		{
			Vector3 newVelocity =
				(transform.right * inputController.MoveDelta.x + transform.forward * inputController.MoveDelta.z)
					* MoveSpeed;
			rigidBody.velocity = newVelocity + Vector3.up * rigidBody.velocity.y;
		}
	}

	private void Rotate()
	{
		if (isMoveInputPresent)
		{
			transform.rotation = 
				Quaternion.Euler(new Vector3(0, cameraFollowTarget.rotation.eulerAngles.y, 0));
			cameraFollowTarget.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}

	private void Jump()
	{
		if (inputController.IsJumping && isOnGround)
		{
			Vector3 newVelocity = new(rigidBody.velocity.x, JumpStrength, rigidBody.velocity.z);
			rigidBody.velocity = newVelocity;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag(GroundTag))
		{
			isOnGround = true;
			inputController.IsJumping = false;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag(GroundTag))
		{
			isOnGround = false;
		}
	}
}

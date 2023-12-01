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
	bool isMoving = false;
	Vector2 moveVector = Vector2.zero;
    Rigidbody rigidBody = null;
	Transform cameraFollowTarget = null;
	InputController inputController = null;
	IEnumerator moveCoroutine = null, rotateCoroutine = null;

	void Start()
	{
		if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
		if (cameraFollowTarget == null) cameraFollowTarget = transform.GetChild(FollowTargetIndex);
		if (inputController == null) inputController = FindObjectOfType<InputController>();
	}

	void Update()
	{
		UpdateMovingState();
	}

	void FixedUpdate()
	{
		Move();
		Rotate();
		Jump();
	}

	void UpdateMovingState()
	{
		isMoving = inputController.MoveDelta != Vector3.zero;
	}

	void Move()
	{
		if (isMoving)
		{
			Vector3 newVelocity =
				(transform.right * inputController.MoveDelta.x + transform.forward * inputController.MoveDelta.z)
					* MoveSpeed;
			rigidBody.velocity = newVelocity + Vector3.up * rigidBody.velocity.y;
		}
	}

	private void Rotate()
	{
		if (isMoving)
		{
			rigidBody.MoveRotation(
				Quaternion.Euler(new Vector3(0, cameraFollowTarget.rotation.eulerAngles.y, 0)));
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

	public void OnMove(InputAction.CallbackContext context)
	{
		moveVector = context.ReadValue<Vector2>();

		if (context.performed)
		{
			rotateCoroutine = RotatePlayer();
			StartCoroutine(rotateCoroutine);
			moveCoroutine = MovePlayer();
			StartCoroutine(moveCoroutine);
		}
		else if (context.canceled)
		{
			Debug.Log("move canceled");
			StopCoroutine(rotateCoroutine);
			StopCoroutine(moveCoroutine);
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{	
		if (context.performed && isOnGround)
		{
			Debug.Log("jump performed");

			Vector3 newVelocity = new(rigidBody.velocity.x, JumpStrength, rigidBody.velocity.z);
			rigidBody.velocity = newVelocity;
		}
	}

	IEnumerator MovePlayer()
	{
		while(true)
		{
			Vector3 newVelocity =
				(transform.right * moveVector.x + transform.forward * moveVector.y) * MoveSpeed;
			rigidBody.velocity = newVelocity + Vector3.up * rigidBody.velocity.y;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator RotatePlayer()
	{
		while(true)
		{
			Debug.Log("rotating!");
			transform.rotation = Quaternion.Euler(0, cameraFollowTarget.rotation.eulerAngles.y, 0);
			cameraFollowTarget.localRotation = Quaternion.Euler(Vector3.zero);
			yield return new WaitForEndOfFrame();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	const string GroundTag = "Ground";

	[SerializeField, Range(8f, 16f)] float moveSpeed = 11f;
	[SerializeField, Range(3f, 8f)] float rotationSpeed = 4f;
	[SerializeField, Range(8f, 16f)] float jumpStrength = 8f;

	bool isOnGround = false;
	float rotateDirection = 0f;
	Vector2 moveVector = Vector2.zero;
    Rigidbody rigidBody = null;
	IEnumerator moveCoroutine = null;
	IEnumerator rotateCoroutine = null;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveVector = context.ReadValue<Vector2>();

		if (context.performed)
		{
			Debug.Log("move performed");
			moveCoroutine = MovePlayer();
			StartCoroutine(moveCoroutine);
		}
		else if (context.canceled)
		{
			Debug.Log("move canceled");
			if (moveCoroutine != null) StopCoroutine(moveCoroutine);
		}
	}

	public void OnRotate(InputAction.CallbackContext context)
	{
		rotateDirection = context.ReadValue<Vector2>().x * rotationSpeed;

		if (context.performed)
		{
			Debug.Log("rotate performed");
			rotateCoroutine = RotatePlayer();
			StartCoroutine(rotateCoroutine);
		}
		else if (context.canceled)
		{
			Debug.Log("rotate canceled");
			if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.performed && isOnGround)
		{
			Debug.Log("jump performed");
			rigidBody.AddForce(0, jumpStrength, 0, ForceMode.VelocityChange);
		}
	}

	IEnumerator MovePlayer()
	{
		while(true)
		{
			Vector3 newVelocity = (transform.right * moveVector.x + transform.forward * moveVector.y) * moveSpeed;
			rigidBody.velocity = newVelocity + Vector3.up * rigidBody.velocity.y;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator RotatePlayer()
	{
		while (true)
		{
			rigidBody.MoveRotation(transform.rotation * Quaternion.Euler(0f, rotateDirection, 0f));
			yield return new WaitForFixedUpdate();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag(GroundTag))
		{
			isOnGround = true;
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

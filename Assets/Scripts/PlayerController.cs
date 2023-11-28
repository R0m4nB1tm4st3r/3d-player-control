using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	const string GroundTag = "Ground";
	const byte FollowTargetIndex = 0;

	[SerializeField, Range(8f, 16f)] float moveSpeed = 11f;
	[SerializeField, Range(3f, 8f)] float rotationSpeed = 4f;
	[SerializeField, Range(8f, 16f)] float jumpStrength = 8f;

	bool isOnGround = false;
	Vector2 moveVector = Vector2.zero;
    Rigidbody rigidBody = null;
	Transform cameraFollowTarget = null;
	IEnumerator moveCoroutine = null, rotateCoroutine = null;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		cameraFollowTarget = transform.GetChild(FollowTargetIndex);
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
			if (moveCoroutine != null) StopCoroutine(moveCoroutine);
			if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{	
		if (context.performed && isOnGround)
		{
			Debug.Log("jump performed");

			Vector3 newVelocity = new(rigidBody.velocity.x, jumpStrength, rigidBody.velocity.z);
			rigidBody.velocity = newVelocity;
		}
	}

	IEnumerator MovePlayer()
	{
		while(true)
		{
			Vector3 newVelocity =
				(transform.right * moveVector.x + transform.forward * moveVector.y) * moveSpeed;
			rigidBody.velocity = newVelocity + Vector3.up * rigidBody.velocity.y;
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator RotatePlayer()
	{
		while(true)
		{
			transform.rotation = Quaternion.Euler(0, cameraFollowTarget.rotation.eulerAngles.y, 0);
			cameraFollowTarget.SetLocalPositionAndRotation(transform.position, Quaternion.Euler(Vector3.zero));
			yield return new WaitForEndOfFrame();
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

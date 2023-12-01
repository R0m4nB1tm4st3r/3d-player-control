using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [field: SerializeField]
    public Vector3 MoveDelta { get; set; } = Vector3.zero;
    [field: SerializeField]
    public Vector2 LookDelta { get; set; } = Vector2.zero;
    [field: SerializeField]
    public bool IsJumping { get; set; } = false;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveVector = context.ReadValue<Vector2>();
        MoveDelta = new Vector3(moveVector.x, 0, moveVector.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookDelta = context.ReadValue<Vector2>();
    }

	public void OnJump(InputAction.CallbackContext context)
	{
        if (context.started)
        {
			IsJumping = context.ReadValueAsButton();
		}
	}
}

using System;
using UnityEngine;

public class RotationController : MonoBehaviour
{
	[field: SerializeField, Range(1f, 5f)] 
    public float RotationSpeed { get; set; } = 2f;

	InputController inputController = null;

    void Start()
    {
        if (inputController == null) inputController = FindObjectOfType<InputController>();
    }

    void Update()
    {
        Rotate();
    }
	void Rotate()
	{
        if (inputController.LookDelta.x != 0) 
        {
			transform.localRotation *=
			    Quaternion.AngleAxis(inputController.LookDelta.x * Time.fixedDeltaTime * RotationSpeed, Vector3.up);
		}
	}
}

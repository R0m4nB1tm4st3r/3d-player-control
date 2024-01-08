using System.Collections;
using UnityEngine;

public class DoorMover : MonoBehaviour
{
    [field: SerializeField]
    public float OpenDoorHeight { get; set; } = 13f;
	[field: SerializeField]
	public float DoorOpeningSpeed { get; set; } = 5f;

    bool isOpen = false;
	Vector3 closedPos;
    Vector3 openedPos;
    PlaneInteractor interactor = null;
    Coroutine moveDoorCoroutine = null;

    void Start()
    {
        closedPos = transform.position;
        openedPos = closedPos + Vector3.up * OpenDoorHeight;

        interactor = GameManager.Instance.PlaneInteractorForDoor;
        interactor.TriggerEnterEvent.AddListener(ChangeDoor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeDoor()
    {
        if (moveDoorCoroutine != null) StopCoroutine(moveDoorCoroutine);
        moveDoorCoroutine = StartCoroutine(MoveDoor());
    }

    IEnumerator MoveDoor()
    {
        Vector3 targetPos = isOpen ? closedPos : openedPos;
		isOpen = !isOpen;

		while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * DoorOpeningSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}

using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    const string DoorOpenerTag = "DoorOpener";

    public PlaneInteractor PlaneInteractorForDoor { get; set; } = null;

	private void Awake()
	{
		PlaneInteractorForDoor = GameObject.FindGameObjectWithTag(DoorOpenerTag).GetComponent<PlaneInteractor>();
	}

	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}

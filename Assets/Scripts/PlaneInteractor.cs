using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer), typeof(MeshCollider))]
public class PlaneInteractor : MonoBehaviour
{
    const string PlayerTag = "Player";

    [field: SerializeField]
    public Color InactiveColor { get; set; } = Color.red;
	[field: SerializeField]
	public Color ActiveColor { get; set; } = Color.green;

	public UnityEvent TriggerEnterEvent { get; private set; } = null;

    bool isActive = false;
    MeshRenderer meshRenderer = null;
    Material material = null;

	void Awake()
	{
		TriggerEnterEvent = new UnityEvent();
		TriggerEnterEvent.AddListener(ChangeState);
	}

	void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        material.color = InactiveColor;
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(PlayerTag)) TriggerEnterEvent.Invoke();
	}

    void ChangeState()
    {
        material.color = isActive ? InactiveColor : ActiveColor;
        isActive = !isActive;
    }
}

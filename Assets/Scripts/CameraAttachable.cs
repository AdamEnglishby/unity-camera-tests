using UnityEngine;

// attach to a gameobject to allow CameraTrackers to follow it & respect CameraRegions
public class CameraAttachable : MonoBehaviour
{

	public delegate void Callback(Collider collider);
	public Callback triggerEnterCallback, triggerExitCallback;

	public void SetTriggerEnterCallback(Callback func)
	{
		triggerEnterCallback = func;
	}

	public void SetTriggerExitCallback(Callback func)
	{
		triggerExitCallback = func;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.GetComponent<CameraRegion>() != null)
			triggerEnterCallback(collider);
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.GetComponent<CameraRegion>() != null)
			triggerExitCallback(collider);
	}

}
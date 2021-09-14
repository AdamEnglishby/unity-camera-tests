using System.Collections.Generic;
using UnityEngine;

// attach to a camera object & specify the gameobject to track
public class CameraTracker : MonoBehaviour
{

	public GameObject trackedObject;
	public float cameraSpeed = 0.5f;
	public float maxDistanceMultiplier = 1f;

	private Vector3 initialOffset;
	private Vector3 dynamicOffset;
	private Vector2 previousPosition;
	private Vector2 maxDistance = new Vector2(2, 2);
	private Vector2 currentOffset = new Vector3();

	private List<CameraRegion> currentCameraColliders = new List<CameraRegion>();

	void Start()
	{
		CameraAttachable triggerDelegate = trackedObject.GetComponent<CameraAttachable>();
		if (triggerDelegate != null)
		{
			triggerDelegate.SetTriggerEnterCallback((Collider collider) =>
			{
				currentCameraColliders.Add(collider.GetComponent<CameraRegion>());
			});
			triggerDelegate.SetTriggerExitCallback((Collider collider) =>
			{
				currentCameraColliders.Remove(collider.GetComponent<CameraRegion>());
			});
		}

		initialOffset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		previousPosition = new Vector2(trackedObject.transform.position.x, trackedObject.transform.position.z);
		Debug.Log(this.name + " attached to " + trackedObject.name);
	}

	void Update()
	{
		// average out the desired transforms of the camera regions the gameobject is currently inside
		Vector3 averagedColliders = initialOffset;
		if (currentCameraColliders.Count > 0)
		{
			averagedColliders = Vector3.zero;
			currentCameraColliders.ForEach(cameraCollider =>
			{
				averagedColliders += cameraCollider.cameraPosition;
			});
			averagedColliders /= currentCameraColliders.Count;
		}

		Vector2 direction = new Vector2(trackedObject.transform.position.x, trackedObject.transform.position.z) - previousPosition;
		currentOffset += direction * cameraSpeed; // TODO: fix high fps weirdness with delta

		// slowly bring camera back to centre if movement is slow enough or stopped
		if (Mathf.Round(direction.x / 100) == 0)
			currentOffset.x /= 1.025f;

		if (Mathf.Round(direction.y / 100) == 0)
			currentOffset.y /= 1.025f;

		// calculate new position of camera based on max distance it can travel from the tracked gameobject
		currentOffset.x = (currentOffset.x > maxDistance.x) ? maxDistance.x : (currentOffset.x < -maxDistance.x) ? -maxDistance.x : currentOffset.x;
		currentOffset.y = (currentOffset.y > maxDistance.y) ? maxDistance.y : (currentOffset.y < -maxDistance.y) ? -maxDistance.y : currentOffset.y;

		// TODO: interpolate camera regions
		this.transform.position = new Vector3(
			averagedColliders.x + trackedObject.transform.position.x + (currentOffset.x * maxDistanceMultiplier),
			averagedColliders.y + trackedObject.transform.position.y,
			averagedColliders.z + trackedObject.transform.position.z + (currentOffset.y * maxDistanceMultiplier)
		);
		previousPosition = new Vector2(trackedObject.transform.position.x, trackedObject.transform.position.z);
	}
}

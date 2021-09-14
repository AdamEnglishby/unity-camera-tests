using System;
using UnityEngine;

public class PlayerControls : CameraAttachable
{

	public int walkSpeed = 100;
	public int runSpeed = 150;

	private Rigidbody _rigidbody;

	void Start()
	{
		this._rigidbody = GetComponent<Rigidbody>();
		if (this._rigidbody == null)
		{
			throw new Exception("No Rigidbody component found on object " + this.name);
		}
	}

	void Update()
	{
		if (this._rigidbody != null)
		{
			// setting transform manually makes collisions jerky, use AddForce instead
			this._rigidbody.AddForce(new Vector3(
				Input.GetAxisRaw("Horizontal") * (Input.GetKey(KeyCode.LeftShift) ? this.runSpeed : this.walkSpeed) * (Time.deltaTime / 0.01666667f),
				0,
				Input.GetAxisRaw("Vertical") * (Input.GetKey(KeyCode.LeftShift) ? this.runSpeed : this.walkSpeed) * (Time.deltaTime / 0.01666667f)
			));
		}
	}

}

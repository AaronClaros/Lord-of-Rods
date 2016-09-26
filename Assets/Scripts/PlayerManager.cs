using UnityEngine;
using System.Collections;

struct PlayerData{
	public Vector3 position, velocity, angVelocity;
	public Quaternion rotation;
	public Quaternion cameraRotation;
	public PlayerData(Vector3 position, Vector3 velocity, Vector3 angVelocity, Quaternion rotation, Quaternion cam_rotation) {
		this.position = position;
		this.velocity = velocity;
		this.angVelocity = angVelocity;
		this.rotation = rotation;
		this.cameraRotation = cam_rotation;
	}
}

public class PlayerManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

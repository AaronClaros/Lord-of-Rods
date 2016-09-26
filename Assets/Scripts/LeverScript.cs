using UnityEngine;
using System.Collections;

public class LeverScript : MonoBehaviour {
    public bool isActive;
    public string doorTag;
    public GameObject doorRef;
    public Animator animRef;
    Vector3 doorOpenPosition;
    Vector3 doorClosePosition;
	// Use this for initialization
	void Start () {
        doorRef = GameObject.Find(doorTag);
        animRef = GetComponent<Animator>();
        isActive = true;
        doorOpenPosition = doorRef.transform.position;
        doorClosePosition = new Vector3(doorRef.transform.position.x, doorRef.transform.position.y - 4.5f, doorRef.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isActive) {
            doorRef.transform.position = Vector3.Slerp (doorOpenPosition, doorClosePosition, 200f);
        }
        else {
            doorRef.transform.position = Vector3.Slerp (doorRef.transform.position, doorOpenPosition, 200f);
        }
        animRef.SetBool("isActive", isActive);
	}

    public void SetActive(bool state) {
        isActive = state;
    }

}

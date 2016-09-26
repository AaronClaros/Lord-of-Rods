using UnityEngine;
using System.Collections;

public class LeverScript : MonoBehaviour {
    public bool isActive;
    public string door1Tag;
    public string door2Tag;
    public GameObject door1Ref;
    public GameObject door2Ref;
    public Animator animRef;
    Vector3 door1OpenPosition;
    Vector3 door1ClosePosition;
    Vector3 door2OpenPosition;
    Vector3 door2ClosePosition;
	// Use this for initialization
	void Start () {
        door1Ref = GameObject.Find(door1Tag);
        door2Ref = GameObject.Find(door2Tag);
        animRef = GetComponent<Animator>();
        isActive = true;
        door1OpenPosition = door1Ref.transform.position;
        door1ClosePosition = new Vector3(door1Ref.transform.position.x, door1Ref.transform.position.y - 4.5f, door1Ref.transform.position.z);
        door2OpenPosition = new Vector3(door2Ref.transform.position.x, door2Ref.transform.position.y + 4.5f, door2Ref.transform.position.z);	
        door2ClosePosition = door2Ref.transform.position; 
    }
	
	// Update is called once per frame
	void Update () {
        if (!isActive) {
            door1Ref.transform.position = Vector3.Slerp(door1OpenPosition, door1ClosePosition, 200f);
            door2Ref.transform.position = Vector3.Slerp(door2ClosePosition, door2OpenPosition, 200f);
        }
        else {
            door1Ref.transform.position = Vector3.Slerp(door1Ref.transform.position, door1OpenPosition, 200f);
            door2Ref.transform.position = Vector3.Slerp(door2OpenPosition, door2Ref.transform.position, 200f);
        }
        animRef.SetBool("isActive", isActive);
	}

    public void SetActive(bool state) {
        isActive = state;
    }

}

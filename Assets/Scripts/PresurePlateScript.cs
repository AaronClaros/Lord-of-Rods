using UnityEngine;
using System.Collections;

public class PresurePlateScript : MonoBehaviour {
    public bool isActive;
    public string door1Tag;
    public GameObject door1Ref;
    Vector3 door1ClosePosition;
    Vector3 door1OpenPosition;

    Vector3 plateOnPosition;
    Vector3 plateOffPosition;
	// Use this for initialization
	void Start () {
        isActive = false;
        door1Ref = GameObject.Find(door1Tag);
        door1ClosePosition = door1Ref.transform.position;
        door1OpenPosition = new Vector3(door1Ref.transform.position.x, door1Ref.transform.position.y + 4.5f, door1Ref.transform.position.z);
        plateOffPosition = transform.position;
        plateOnPosition = new Vector3(transform.position.x, transform.position.y - 0.18f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	    if (isActive) {
            door1Ref.transform.position = Vector3.Slerp(door1Ref.transform.position, door1OpenPosition, 200f);
        }
        else {
            door1Ref.transform.position = Vector3.Slerp(door1Ref.transform.position, door1ClosePosition, 200f);
        }
	}

    void OnTriggerEnter(Collider other) {
        if (!isActive) {
            StartCoroutine(PressAnim());
            isActive = true;   
        }        
    }
    void OnTriggerExit(Collider Other){
        if (isActive) {
            StartCoroutine(UnpressAnim());
            isActive = false;
        }
    }

    IEnumerator PressAnim() {
            transform.position = Vector3.Lerp(transform.position, plateOnPosition, 100f);
            print("pressing plate");
            yield return 0;
        StopCoroutine("PressAnim");
    }

    IEnumerator UnpressAnim() {
            transform.position = Vector3.Lerp(transform.position, plateOffPosition, 100f);
            print("Unpressing plate");
            yield return 0;
        StopCoroutine("UnPressAnim");
    }
}

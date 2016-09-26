using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponManeger : TimeManipulable{ 
	public GameObject pickedObject; 
	public float pickDistance;
	public float pickSmooth; 
	public RigidbodyFirstPersonController fpcontrolRef;
    public bool pick_flag;
	// Use this for initialization
	void Start () {
		fpcontrolRef = GetComponent<RigidbodyFirstPersonController>();
        pick_flag = false;
		base.Setting();
	} 
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!pick_flag) { 
            if (Input.GetMouseButtonDown(0)){
			    print("left click down");
                ShootFreeze();
			    //base.SetRewindable();
                //base.StopPhysicsObject();
		    }
		    if (Input.GetMouseButtonDown(1)){
			    print("right click down");
                ShootRewind();
                //StartRewind();
                //base.ResumePhysicsObject();
			
		    }
        }

        if (Input.GetKey(KeyCode.C)) {
            if (Input.GetMouseButtonDown(0)){
			    print("left click down FPC");
                if (!rewindReady_flag) {
                    base.SetRewindable();
                }
                else {
                    StartRewind();
                }
		    }
		    if (Input.GetMouseButtonDown(1)){
                print("right click down FPC");
                if (!pause_flag) {
                    StopPhysicsObject();
                }
                else {
                    ResumePhysicsObject();
                }
		    }
        }


        if (Input.GetKeyDown(KeyCode.E)) {
            if (!pick_flag)
                PickObject();
            else
                DropObject();
        }

		if (timeState == TimeState.Rewind ){
			fpcontrolRef.enabled = false;
		} else if (timeState == TimeState.Normal){
			fpcontrolRef.enabled = true;
		}

        if (rewindDataStack.Count > 1000){
            StartRewind();
            print("rewindstackcount > 1000, activating rewind");
        }
			
	}

	public override void StartRewind() {
        if (rewindReady_flag) { 
		    base.StartRewind();
            StartCoroutine(Rewind(RewindMode.Transform)); 
        }
	}
	public override IEnumerator Rewind(RewindMode mode){
        yield return base.Rewind(mode);
	}

	void PickObject(){
		RaycastHit hit;
		Ray origin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast (origin, out hit, pickDistance)) {
			GameObject objTarget = hit.collider.gameObject;
            if (objTarget == null)
                return;
            if (objTarget.tag == "Pickable") {
                TimeManipulable tmRef = hit.collider.GetComponent<TimeManipulable>();
                print("trying to pick:" + objTarget.name);
                if (pickedObject == null && !tmRef.pause_flag) {
                    pick_flag = true;
                    pickedObject = objTarget;
                    Rigidbody rbPO = pickedObject.GetComponent<Rigidbody>();
                    rbPO.useGravity = false;
                    rbPO.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    StartCoroutine(Pick());
                }
            }
		}
	}
    void DropObject() {
        if (pickedObject != null) {
            print("droping: " + pickedObject.name);
            pick_flag = false;
            pickedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            pickedObject = null;
            StopCoroutine("Pick");
        }
    }

    IEnumerator Pick() {
        while (pick_flag){
            Ray ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0f));
            Vector3 point = ray.origin + (ray.direction * pickDistance);
            //print("pickedObj pos: " + point);
            pickedObject.GetComponent<Rigidbody>().velocity = new Vector3(0.001f,0.001f,0.001f);
            pickedObject.GetComponent<Rigidbody>().MovePosition(point);
            //pickedObject.transform.position = Vector3.Lerp(pickedObject.transform.position, point, pickSmooth);
            yield return 0;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Interactable") {
            LeverScript leverRef = other.gameObject.GetComponent<LeverScript>();
            if (leverRef != null) {
                print("lever nearby");
                if (Input.GetKeyDown(KeyCode.E)) { 
                    leverRef.SetActive(!leverRef.isActive);
                }
            }
        }
    }
    void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Interactable") {
            LeverScript leverRef = other.gameObject.GetComponent<LeverScript>();
            if (leverRef != null) {
                //leverRef.isActive = true;
                print("leaving lever");
            }
        }
    } 
    void ShootFreeze(){
        RaycastHit hit;
        Ray origin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(origin, out hit)){
            //print("looking to :"+hit.collider.gameObject.name);
            BoxScript objTarget = hit.collider.GetComponent<BoxScript>();
            if (objTarget == null)
                return;
            if (!objTarget.pause_flag && objTarget.timeState == TimeState.Normal){
                objTarget.StopPhysicsObject();
                print("Freezing: "+objTarget.name);
            } else {
                objTarget.ResumePhysicsObject();
                print("Unfreezing: "+objTarget.name);
            }
        }
    }

    void ShootRewind(){
        RaycastHit hit;
        Ray origin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(origin, out hit)){
            //print("looking to :"+hit.collider.gameObject.name);
            BoxScript objTarget = hit.collider.GetComponent<BoxScript>();
            if (objTarget == null)
                return;
            if (!objTarget.rewindReady_flag && objTarget.timeState == TimeState.Normal){
                objTarget.SetRewindable();
                print("Setting rewind: "+objTarget.name);
            } else if (objTarget.rewindReady_flag && objTarget.timeState == TimeState.Normal) {
                objTarget.StartRewind();
                print("Rewinding: "+objTarget.name);
            }
        }
    }
}


    /*
   
    */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct RewindData{
	public Vector3 position, scale, velocity, angular_vel;
	public Quaternion rotation;
	public RewindData(Transform tr, Rigidbody rb){
		position = tr.position;
		rotation = tr.rotation;
		scale = tr.localScale;
		velocity = rb.velocity;
		angular_vel = rb.angularVelocity;
	}
}

public enum TimeState { Normal, Pause, Rewind}
public enum RewindMode { Transform, Physics}

public class TimeManipulable : MonoBehaviour {
	public Stack<RewindData> rewindDataStack;
    RewindData pauseData;
	Transform trRef;
	public Rigidbody rbRef;
    public TimeState timeState;
	public bool rewindReady_flag;
	public bool rewindStar_flag;
    public bool pause_flag;
    bool saveData_flag;

    void Update() {
        switch (timeState){
            case TimeState.Normal:
                if (rewindReady_flag) { 
                    if (!saveData_flag) { 
                        StartCoroutine(SaveRewindDataFrame(trRef, rbRef));
                    }
                }
                break;
            case TimeState.Pause:
                break;
            case TimeState.Rewind:
                break;
            default:
                break;
        }
    }
	
	public void Setting(){
		rewindDataStack = new Stack<RewindData>();
        timeState = TimeState.Normal;
		trRef = GetComponent<Transform>();
		rbRef = GetComponent<Rigidbody>();
        rewindReady_flag = false;
		rewindStar_flag = false;
        pause_flag = false;
        saveData_flag = false;
	}

    public virtual void StopPhysicsObject(){
        if (timeState != TimeState.Pause) {
            print("freezing: "+gameObject.name);
            timeState = TimeState.Pause;
            pause_flag = true;
			pauseData = new RewindData(transform, GetComponent<Rigidbody>());
            rbRef.isKinematic = true;
            rbRef.Sleep();
        }
    }

    public virtual void ResumePhysicsObject(){
        if (timeState == TimeState.Pause) {
            print("Unfreezing: "+gameObject.name);
            timeState = TimeState.Normal;
            pause_flag = false;
			SetActualData(pauseData, RewindMode.Transform);
            rbRef.isKinematic = false;
            rbRef.WakeUp();
        }
    }

	public void SetActualData(RewindData data, RewindMode mode){
        switch (mode){
            case RewindMode.Physics:
                rbRef.MovePosition(data.position);
                rbRef.MoveRotation(data.rotation);
                break;
            case RewindMode.Transform:
                trRef.position = data.position;
                trRef.rotation = data.rotation;
                break;
            default:
                break;
        }
		trRef.localScale = data.scale;
		rbRef.velocity = data.velocity;
		rbRef.angularVelocity = data.angular_vel;
	}

	public virtual IEnumerator SaveRewindDataFrame(Transform tr, Rigidbody rb){
        saveData_flag = true;
		RewindData data = new RewindData(tr,rb);
        if (rbRef.velocity != Vector3.zero)
		    rewindDataStack.Push(data);
        print("rewindDataStack count: "+rewindDataStack.Count);
		yield return new WaitForEndOfFrame();
		saveData_flag = false;
		
	}

    public void SetRewindable() {
		if (timeState != TimeState.Rewind){
			print("rewind point setup to: "+gameObject.name);
			rewindDataStack = new Stack<RewindData>();
			rewindReady_flag = true;
		}
	}
	public virtual void StartRewind() {
		rewindReady_flag = false; 
		timeState = TimeState.Rewind; 
		print("starting rewind"); 

	}
	public virtual IEnumerator Rewind(RewindMode mode){
		if (timeState == TimeState.Rewind) {
			while (rewindDataStack.Count > 0) {
				RewindData data = rewindDataStack.Peek();
                SetActualData(data, mode);
				if (mode == RewindMode.Transform)
                    rbRef.Sleep();
                if (mode == RewindMode.Physics) {
                    rbRef.isKinematic = true;
                }
				print("Rewinding: " + gameObject.name + " StackCount: " + rewindDataStack.Count);
				if (rewindDataStack.Count == 1){
					print("stop rb_rewind");
					timeState = TimeState.Normal;
                    rbRef.isKinematic = false;
					rbRef.WakeUp();
                    StopCoroutine("Rewind");
				}
				rewindDataStack.Pop();
				yield return 0;
			}
            if (rewindDataStack.Count == 0) {
                print("stop rb_rewind");
                timeState = TimeState.Normal;
                rbRef.isKinematic = false;
                rbRef.WakeUp();
                StopCoroutine("Rewind");
            }
		}else{
			yield return 0;
		}

	}


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObjectTimeState { Freezed, Rewind, Normal };

public interface IRewindable<T> {
    T StoreDataRewind();
	void RewindData();
}

public interface IFreezable<T> {
    T SaveDataStop();
    void SetDataObject(T data);
}

public class ManegableObject : MonoBehaviour {
	[HideInInspector]
    public Rigidbody rb;
    public ObjectTimeState timeState;
    public bool hasFreezeBullet;
    public bool hasRewindBullet;
	[HideInInspector]
    public Renderer goRenderRef;
	public Material matNormal;
	//public Material matMark;
	//public Texture textureFreezed;
	//public Texture textureRewind;

	//public float timeSetRewind;

    public void SetUpData() {
        rb = GetComponent<Rigidbody>();
        goRenderRef = GetComponent<Renderer>();
		hasFreezeBullet = false;
		hasRewindBullet = false;
    }

    
    public virtual void StopPhysicsObject() {
        timeState = ObjectTimeState.Freezed;
		//goRenderRef.material.SetTexture("_albedo", textureFreezed);
		hasFreezeBullet = true;
		rb.isKinematic = true;
        rb.Sleep();
    }
    
    public virtual void ResumePhysicsObject() {
        timeState = ObjectTimeState.Normal;
		//goRenderRef.material.SetTexture("_albedo", null);
		hasFreezeBullet = false;
		rb.isKinematic = false;
        rb.WakeUp();
    }
    
	public void SetRewindObject(){
		if (!hasRewindBullet){
			hasRewindBullet = true;

		}
	}
	/*
	public void RewindPhysicsObject() {
		if (hasRewindBullet){
			print("base.Rewinding: "+gameObject.name);
			timeState = ObjectTimeState.Rewind;
			r_velocity = rb.velocity;
			r_angVelocity = rb.angularVelocity;
			rb.velocity = r_velocity * -1;
			rb.angularVelocity = r_angVelocity * -1;
			StartCoroutine(ShowRewindEffect(timeSetRewind));
		}
	}
	/*
	IEnumerator LeftObjectMark(float time){
		GameObject go = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), transform.position, transform.rotation) as GameObject;
		go.GetComponent<Renderer>().material = matMark;
		go.layer = 2;
		yield return new WaitForSeconds(time);
		Destroy(go);
		StopCoroutine("LeftObjectMark");
	} 
	IEnumerator ShowRewindEffect(float time){
		goRenderRef.material = matRewind;
		yield return new WaitForSeconds(time);
		timeState = ObjectTimeState.Normal;
		hasRewindBullet = false;
		goRenderRef.material = matNormal;
		StopCoroutine("ShowRewindEffect");
	}*/
}

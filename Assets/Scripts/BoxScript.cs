using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxScript : TimeManipulable {
	[SerializeField]
	public Vector3 moveDirection;
	public float colorChangeInterval;
	private float lastChangeColor;
	private float timeAtStop;
	[SerializeField]
	public Color colorA;
	[SerializeField]
	public Color colorB;
    Renderer renderRef;

    bool dataSave_flag = false;

	// Use this for initialization
	void Start () {
        base.Setting();
        renderRef = GetComponent<Renderer>();
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		switch (timeState) {
            case TimeState.Normal:
			ChangingColor (colorA, colorB);
            //if (rbRef.velocity == Vector3.zero)
                //rbRef.AddForce(moveDirection*-1, ForceMode.VelocityChange);
			break;
		default:
			break;
		}

	}

    public override void StartRewind() {
        if (rewindReady_flag) { 
		    base.StartRewind();
            StartCoroutine(Rewind(RewindMode.Physics)); 
        }
	}
	public override IEnumerator Rewind(RewindMode mode){
        yield return base.Rewind(mode);
	}

	void ChangingColor (Color a, Color b){
		if (Time.time > lastChangeColor + colorChangeInterval) {
			lastChangeColor = Time.time;
			print ("changing color");
			if (renderRef.material.color == a) {
                renderRef.material.color = b;
			} else {
                renderRef.material.color = a;
			}
		}
	}
}

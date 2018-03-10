using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDebug : MonoBehaviour {

	public delegate void PositionChangeDelegate(Vector3 newPosition);
	public event PositionChangeDelegate posChangeEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RepositionCurrentTargetTo(Vector3 newPosition){
		transform.position = newPosition;
		posChangeEvent(newPosition);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TargetDebug))]
public class TargetDebugInspector : Editor {
	int pos;

	Vector3[] positions;

	TargetDebug targetDebug;

	// Use this for initialization
	public void OnEnable () {
		positions = new Vector3[]{Vector3.zero, new Vector3(2,0,2), new Vector3(2,0,-2), new Vector3(-2,0,-2), new Vector3(-2,0,2)};

		targetDebug = serializedObject.targetObject as TargetDebug;
	}
	
	// Update is called once per frame
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		if(GUILayout.Button("change target position")){
			pos = (pos + 1) % 5;
			Debug.Log(pos);
			targetDebug.RepositionCurrentTargetTo(positions[pos]);
		}
	}
}

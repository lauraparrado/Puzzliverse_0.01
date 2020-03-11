using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensoresT : MonoBehaviour {

	public GameObject sensorLeft, sensorRight, sensorUp, sensorDown;
	public float radioSensor=1.0f;
	//[HideInInspector]
	public bool ocupadoLeft, ocupadoRight, ocupadoUp, ocupadoDown;

	void FixedUpdate(){
		Comprobar ();
	}
	
	void Comprobar(){
		ocupadoLeft = Physics2D.OverlapCircle (sensorLeft.transform.position, radioSensor);
		ocupadoRight  = Physics2D.OverlapCircle (sensorRight.transform.position, radioSensor);
		ocupadoUp = Physics2D.OverlapCircle (sensorUp.transform.position, radioSensor);
		ocupadoDown = Physics2D.OverlapCircle (sensorDown.transform.position, radioSensor);
	}
}

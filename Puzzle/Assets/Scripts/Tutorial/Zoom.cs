using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom : MonoBehaviour {
	Slider zoom;

	void Awake(){
		zoom = (Slider)GameObject.Find ("Zoom").GetComponent (typeof(Slider)) as Slider;
	}

	public void CambiarZoom(){
		GetComponent<Camera> ().orthographicSize = zoom.value;
	}

}

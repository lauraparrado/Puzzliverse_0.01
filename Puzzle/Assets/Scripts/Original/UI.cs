using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	Text numMovimientosTxt;
	int numMovimientos;

	void Awake(){
		
		numMovimientosTxt=GameObject.Find ("Movimientos").GetComponent (typeof(Text)) as Text ;
	}

	public void SumarMovimiento(){
		numMovimientos += 1;
		numMovimientosTxt.text = "Movimientos: " + numMovimientos.ToString ();
	}
}

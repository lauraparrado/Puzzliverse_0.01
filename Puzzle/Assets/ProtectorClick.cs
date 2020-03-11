using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectorClick : MonoBehaviour {
	public bool sePuedeMover=true;

	public void Interaccion(bool interactuando){
		sePuedeMover = !interactuando;
	}


}

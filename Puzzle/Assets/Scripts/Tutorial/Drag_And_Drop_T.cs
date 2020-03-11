using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_And_Drop_T : MonoBehaviour {

	Vector3 screenSpace, offset, posInicial;
	SensoresT sensores;
	PuzzleT puzzle;
	bool moviendoLeft, moviendoRight, moviendoUp, moviendoDown;
	UI_T ui;
	ProtectorClick protectorClick;

	void Awake(){
		sensores = GetComponentInChildren (typeof(SensoresT)) as SensoresT;
		puzzle = GameObject.Find ("Scripts").GetComponent (typeof(PuzzleT)) as PuzzleT;
		ui = GameObject.Find ("Scripts").GetComponent (typeof(UI_T)) as UI_T;
		protectorClick = GameObject.Find ("Zoom").GetComponent (typeof(ProtectorClick)) as ProtectorClick;
	}

	void OnMouseDown(){
		if (!protectorClick.sePuedeMover) {
			return;
		}
		screenSpace = Camera.main.WorldToScreenPoint (transform.position);
		offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		posInicial = transform.position;
	}

	void OnMouseDrag(){
		Vector3 posicion = new Vector3  (Input.mousePosition.x, Input.mousePosition.y, 0);
		Vector3 curScreenSpace = posicion;
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenSpace) + offset;

		if (!sensores.ocupadoLeft || !sensores.ocupadoRight) {							//Movimiento horizontal
			curPosition = new Vector3 (curPosition.x, transform.position.y, 0);
			if (!sensores.ocupadoLeft && !moviendoLeft && !moviendoRight) {
				moviendoLeft = true;
			}
			if (!sensores.ocupadoRight && !moviendoLeft && !moviendoRight) {
				moviendoRight = true;
			}
		} else if (!sensores.ocupadoUp || !sensores.ocupadoDown) {						//Movimiento vertical
			curPosition = new Vector3 (transform.position.x, curPosition.y, 0);
			if (!sensores.ocupadoUp && !moviendoUp && !moviendoDown) {
				moviendoUp = true;
			}
			if (!sensores.ocupadoDown && !moviendoUp && !moviendoDown) {
				moviendoDown = true;
			}
		} else {
			return;
		}

		//Proteger los movimientos hacia la dirección correcta

		if (moviendoLeft) {
			if(curPosition.x>posInicial.x){
				return;
			}
		}
		if (moviendoRight) {
			if(curPosition.x<posInicial.x){
				return;
			}
		}
		if (moviendoUp) {
			if(curPosition.y<posInicial.y){
				return;
			}
		}
		if (moviendoDown) {
			if(curPosition.y>posInicial.y){
				return;
			}
		}

		if(Vector3.Distance(curPosition,posInicial)>1){return;}			//Restringir movimiento a 1 de distancia como máximo

		transform.position = curPosition;
	}

	void OnMouseUp(){
		if (!protectorClick.sePuedeMover) {
			return;
		}
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), 0);
		if (transform.position != posInicial) {
			ui.SumarMovimiento ();
			puzzle.ComprobarGanador ();
		}


		moviendoLeft = false;
		moviendoRight = false;
		moviendoUp = false;
		moviendoDown = false;

	}


}

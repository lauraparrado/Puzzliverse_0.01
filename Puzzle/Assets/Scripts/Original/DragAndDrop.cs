using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	Vector3 screenSpace;
	Vector3 offset;
	Sensores sensores;
	Vector3 posInicial;
	Puzzle puzzle;
	bool moviendoLeft, moviendoRight, moviendoUp, moviendoDown;
	UI uI;

	void Awake(){
		sensores = GetComponentInChildren(typeof(Sensores)) as Sensores;
		puzzle = GameObject.Find ("Scripts").GetComponent (typeof(Puzzle)) as Puzzle;
		uI=GameObject.Find ("Scripts").GetComponent (typeof(UI)) as UI;
	}

	void OnMouseDown(){
			//translate the cubes position from the world to Screen Point
		screenSpace = Camera.main.WorldToScreenPoint(transform.position);
			//calculate any difference between the cubes world position and the mouses Screen position converted to a world point  
		offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
		posInicial = transform.position;
	}

	void OnMouseDrag () {
		Vector3 posicion=new Vector3 (Input.mousePosition.x, Input.mousePosition.y , 0); 	//keep track of the mouse position
		Vector3 curScreenSpace = posicion;    		//convert the screen mouse position to world point and adjust with offset
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;		//comprobar sensores y restringir movimiento ortogonal
			
		if (!sensores.ocupadoLeft  || !sensores.ocupadoRight) {								//Movimiento horizontal
			curPosition = new Vector3 (curPosition.x, transform.position.y, 0);
			if (!sensores.ocupadoLeft && !moviendoLeft && !moviendoRight) {
				moviendoLeft = true;
			}
			if (!sensores.ocupadoRight && !moviendoRight && !moviendoLeft) {
				moviendoRight = true;
			}
		} else if (!sensores.ocupadoUp  || !sensores.ocupadoDown) {							//Movimiento vertical
			curPosition = new Vector3 (transform.position.x, curPosition.y, 0);
			if (!sensores.ocupadoUp && !moviendoUp && !moviendoDown) {
				moviendoUp = true;
			}
			if (!sensores.ocupadoDown && !moviendoDown && !moviendoUp) {
				moviendoDown = true;
			}
		} else {
			return;																			//Si no tiene ningún lado libre no se puede mover
		}

		//Proteger los movimientos hacia la dirección correcta

		if (moviendoLeft) {
			if (curPosition.x > posInicial.x) {
				return;
			}
		}
		if (moviendoRight) {
			if (curPosition.x < posInicial.x) {
				return ;
			}
		}
		if (moviendoUp) {
			if (curPosition.y < posInicial.y) {
				return;
			}
		}
		if (moviendoDown) {
			if (curPosition.y > posInicial.y) {
				return ;
			}
		}

		if(Vector3.Distance(curPosition,posInicial)>1){return;}								//Restringir movimiento a 1 como máximo
		transform.position = curPosition;													//update the position of the object in the world
	}



	void OnMouseUp(){
			//Al dejar la ficha, fijarla a la posición entera más cercana
		transform.position=new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),0);
		moviendoLeft = false;
		moviendoRight = false;
		moviendoUp = false;
		moviendoDown = false;
		uI.SumarMovimiento ();
		puzzle.ComprobarGanador();
	}
}

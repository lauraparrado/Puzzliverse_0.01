using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Puzzle : MonoBehaviour {

	public List<Sprite> fichaImg=new List<Sprite>();
	public GameObject fichaPrfb;
	public GameObject bordePrfb;
	public Sprite fichaEscondidaImg;
	public GameObject textoGanador;

	int indexFichaEscondida;
	GameObject fichaEscondida;
	int numFichas, numCostado;
	Vector2 posFichaEscondida;
	GameObject padreFichas;
	GameObject padreBordes;
	List<Vector3> posicionesIniciales = new List<Vector3>();
	GameObject[] _fichas;

	void Awake(){
			//recuperamos el padre de las fichas y de los bordes
		padreFichas=GameObject.Find("Fichas");
		padreBordes = GameObject.Find ("Bordes");
	}

	void Start () {
		if (Mathf.Sqrt (fichaImg.Count)  == Mathf.Round (Mathf.Sqrt (fichaImg.Count))) {			//Comprobar que es un número cuadrado
			CrearFichas ();
		} else {
			print ("Imposible crear fichas");
		}
	}
	
	void CrearFichas(){
		int contador=0;
		numFichas = fichaImg.Count;																	//Guardamos el total de fichas
		numCostado = (int) Mathf.Sqrt (fichaImg.Count);												//Guardamos el número de fichas por lado

			//Doble bucle para colocar todas las fichas en su sitio
		for (int alto = numCostado+2; alto >0; alto--) {
			for (int ancho = 0; ancho < numCostado+2; ancho++) {
				Vector3 posicion = new Vector3 (ancho - (numCostado / 2), alto - (numCostado / 2),0);//posición de cada ficha

					//Comprobar si son posiciones de borde o de fichas
				if (alto == 1 || alto == numCostado +2 || ancho == 0 || ancho == numCostado+1) { 	//Es parte del borde
					GameObject borde = Instantiate (bordePrfb, posicion, Quaternion.identity);		//Instanciamos el borde  
					borde.transform.parent = padreBordes.transform;									//lo ponemos cómo hijo de Bordes
				} else {																			//Es parte del puzzle
					GameObject ficha = Instantiate (fichaPrfb, posicion, Quaternion.identity);		//Instanciamos la ficha
					ficha.GetComponent<SpriteRenderer> ().sprite = fichaImg [contador];				//asignar sprite a cada ficha y cómo hijo de Puzzle
					ficha.transform.parent = padreFichas.transform;
					ficha.name=fichaImg [contador].name;											//Dejarle el mismo nombre que tenía el sprite
					if (ficha.name == fichaEscondidaImg.name) {										//Comprobar si es la ficha que queremos esconder
						fichaEscondida  = ficha;													//Asignar
					}
					contador++;
				}
			}
		}
			
		fichaEscondida.gameObject.SetActive(false);
			//Almacenar posiciones iniciales
		_fichas=(GameObject.FindGameObjectsWithTag("Ficha"));					//Recuperamos todas las fichas con el tag Ficha
		for (int i = 0; i < _fichas.Length; i++) {
			posicionesIniciales.Add(_fichas [i].transform.position);			//Asignamos las posiciones iniciales de las fichas a la variable
		}
		Barajar ();
	}

	void Barajar(){
		int aleatorio;
			//Barajamos las posiciones de las fichas
		for (int i = 0; i < _fichas.Length; i++) {
			aleatorio = Random.Range (i, _fichas.Length);								//Creamos un numero aleatorio entre =i y el número de fichas
			Vector3 posTemp = _fichas [i].transform.position;							//En una variable temporal guardamos la posicion de la ficha[i]
			_fichas [i].transform.position = _fichas [aleatorio].transform.position  ;	//Cambiamos la posición de la ficha[i] por la de ficha[aleatorio]
			_fichas [aleatorio].transform.position  = posTemp;							//Cambiamos la posicion de la ficha[aleatorio] por la que
																						//habíamos guardado en la variable temporal
		}
	}

	public void ComprobarGanador(){
		for (int i = 0; i < _fichas.Length; i++) {
			if (posicionesIniciales [i] != _fichas [i].transform.position) {	//Repasamos las posiciones actuales y si una ya no es la
				return;															//misma que la inicial, salimos del método
			}
		}
		fichaEscondida.gameObject.SetActive(true);
		print ("Puzzle resuelto");												//Si hemos llegado a este punto es que el puzzle es correcto
		textoGanador.gameObject.SetActive(true);
	}


}

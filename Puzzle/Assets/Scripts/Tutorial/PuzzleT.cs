using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using String=System.String;
using DayOfWeek=System.DayOfWeek;
using DateTime=System.DateTime;
public class PuzzleT : MonoBehaviour {

	//public Sprite imagen;
	public int numFichasLado;
	public GameObject fichaPrfb;
	public GameObject bordePrfb;
	public GameObject textoGanador;



	List<Sprite> fichaImg=new List<Sprite>();
	Sprite fichaEscondidaImg;
	GameObject fichaEscondida;
	int numCostado;
	GameObject padreFichas;
	GameObject padreBordes;
	List<Vector3> posicionesIniciales=new List<Vector3>();
	GameObject[] _fichas;
	InputField tamañoInput;
	String url="";

	void Awake(){
		//Recuperamos el padre de las fichas y de los bordes
		padreFichas=GameObject.Find("Fichas");
		padreBordes = GameObject.Find ("Bordes");
		tamañoInput = GameObject.Find ("TamañoPuzzle").GetComponent (typeof(InputField)) as InputField;
	}



	public void CargarImagen(){
		Resetear ();
		Invoke ("AbrirImagen", 0.1f);
	}
	private String getWeek(){
        CultureInfo myCI = new CultureInfo("en-US");
        Calendar myCal = myCI.Calendar;
        CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
      	DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
		string week="";
        int week_number=myCal.GetWeekOfYear( DateTime.Now, myCWR, myFirstDOW);
        week_number=week_number-1;
		if (week_number<10){
			week="0"+week_number;
		}
		else{
			week=week_number.ToString();
		}
        int year = System.DateTime.Now.Year%2000;
		string date=year+week;
		return date;
	}
    // this section will be run independently
    private IEnumerator LoadFromLikeCoroutine()
    {
		Texture2D tex;
        tex = new Texture2D (64, 64, TextureFormat.ARGB32, false);
        url = "https://cdn.eso.org/images/screen/potw"+getWeek()+"a.jpg";
        Debug.Log("Loading ....");
        WWW wwwLoader = new WWW(url);   // create WWW object pointing to the url
                // start loading whatever in that url ( delay happens here )
        //thisSprite=Sprite.Create(wwwLoader.texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f),100);;
        yield return wwwLoader;
        Debug.Log("Loaded");
       //GetComponent<Renderer>().material.color = Color.white;     // set white
        if(wwwLoader.error != null){
			//www.LoadImageIntoTexture(tex);
            
            RecortarImagen (Resources.Load<Texture2D>("Jerry_9"));
            Debug.Log("exc");
            Debug.Log(url);
        }
        else{
			wwwLoader.LoadImageIntoTexture(tex);
            GetComponent<Renderer>().material.mainTexture = tex;  // set loaded image
			RecortarImagen (tex);
        }
        
        
    }

	public void AbrirImagen(){

		//Creamos una textura2D con la imagen que seleccionamos desde el explorador
		StartCoroutine(LoadFromLikeCoroutine());
		
	}

	string  Ruta()
	{
		OpenFileName ofn = new OpenFileName();
		ofn.structSize = Marshal.SizeOf(ofn);
		ofn.filter = "Archivos de imagen\0*.*\0\0";
		ofn.file = new string(new char[256]);
		ofn.maxFile = ofn.file.Length;
		ofn.fileTitle = new string(new char[64]);
		ofn.maxFileTitle = ofn.fileTitle.Length;
		ofn.initialDir =UnityEngine.Application.dataPath;//默认路径
		ofn.title = "Cargar imagen";
		ofn.defExt = "JPG";//显示文件的类型
		//注意 一下项目不一定要全选 但是0x00000008项不要缺少
		ofn.flags=0x00080000|0x00001000|0x00000800|0x00000200|0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
		if (DllTest.GetOpenFileName (ofn)) {
			//Debug.Log ("Selected file with full path: {0}" + ofn.file);
			return ofn.file;
		} else {
			return "";
		}
	}

	public void RecortarImagen(Texture2D textImagen){

		int contador = 0;
		int tamañoLado;
		if (textImagen.height > textImagen.width) {					//Escogemos el lado más pequeño para el cuadrado
			tamañoLado = (int)Mathf.Round (textImagen.width / numFichasLado);
		} else {
			tamañoLado = (int)Mathf.Round (textImagen.height  / numFichasLado);
		}
			//Añadir los trocitos de Sprite dentro de la lista fichaImg
		for (int y = numFichasLado - 1; y >= 0; y--) {
			for (int x = 0; x < numFichasLado; x++) {
				Sprite sprite = Sprite.Create (textImagen,
					               new Rect (x * tamañoLado, y * tamañoLado, tamañoLado, tamañoLado),
					               new Vector2 (0, 0), tamañoLado);
				sprite.name = "F_" + contador.ToString ();
				fichaImg.Add (sprite);
				contador++;
			}
		}
		fichaEscondidaImg = fichaImg [numFichasLado - 1];		//Escondemos la ficha de la esquina superior derecha
			//Comprobar que es un número cuadrado
		if (Mathf.Sqrt (fichaImg.Count) == Mathf.Round (Mathf.Sqrt (fichaImg.Count))) {
			CrearFichas ();
		} else {
			print ("Imposible crear fichas");
		}
	}

	public void Resetear(){
		GameObject[] fichas = GameObject.FindGameObjectsWithTag ("Ficha");
		foreach (GameObject ficha in fichas) {
			Destroy (ficha);
		}
		GameObject[] bordes = GameObject.FindGameObjectsWithTag ("Borde");
		foreach (GameObject borde in bordes) {
			Destroy (borde);
		}
		posicionesIniciales.Clear ();
		fichaImg.Clear ();
	}


	void CrearFichas(){
		int contador = 0;
		numCostado = (int)Mathf.Sqrt (fichaImg.Count);
			//Doble bucle para colocar todas las fichas en su sitio
		for (int alto = numCostado + 2; alto > 0; alto--) {
			for (int ancho = 0; ancho < numCostado + 2; ancho++) {
				Vector3 posicion= new Vector3(ancho-(numCostado/2),alto-(numCostado/2),0);	//posición de cada ficha

					//Comprobar si son posiciones de borde o de fichas
				if (alto == 1 || alto == numCostado + 2 || ancho == 0 || ancho == numCostado + 1) {	//Es parte del borde
					GameObject borde =Instantiate (bordePrfb,posicion,Quaternion.identity); 		//Instanciamos el borde
					borde.transform.parent=padreBordes.transform;								//lo ponemos cómo hijo de PadreBordes
				} else {																			//Es parte del puzzle
					GameObject ficha=Instantiate(fichaPrfb,posicion,Quaternion.identity);			//Instanciamos la ficha
					ficha.GetComponent<SpriteRenderer>().sprite=fichaImg[contador];			//Asignamos el sprite a cada ficha
					ficha.transform.parent=padreFichas.transform;								//Lo ponemos como hijo de PadreFichas
					ficha.name=fichaImg[contador].name;										//Dejamos el mismo nombre que el Sprite
					if (ficha.name == fichaEscondidaImg.name) {								//Si es la ficha que tenemos que esconder
						fichaEscondida=ficha;												//La asignamos
					}
					contador++;
				}
			}
		}
		fichaEscondida.gameObject.SetActive (false);							//Escondemos la fichaEscondida
			//Almacenar las posiciones iniciales
		_fichas=GameObject.FindGameObjectsWithTag("Ficha");						//Recuperamos todas las fichas con el tag ficha
		for (int i = 0; i < _fichas.Length ; i++) {
			posicionesIniciales.Add (_fichas [i].transform.position);			//Asignamos las posiciones iniciales de las fichas
		}
		Barajar ();
	}

	void Barajar(){
		int aleatorio;
		//Barajamos las posiciones de las fichas
		for (int i = 0; i < _fichas.Length ; i++) {
			aleatorio = Random.Range (i, _fichas.Length);		//Creamos un numero aleatorio entre i y el numero de fichas
			Vector3 posTemp=_fichas[i].transform.position;		//En una variable temporal guardamos la posición inicial
			_fichas[i].transform.position=_fichas[aleatorio].transform.position;	//Cambiamos la posición ficha[aleatorio] por ficha[i]
			_fichas[aleatorio].transform.position=posTemp;		//asignamos la posicion inicial que habíamos guardado a fichas[aleatorio]
		}
	}

	public void ComprobarGanador(){
		for (int i = 0; i < _fichas.Length ; i++) {
			if (posicionesIniciales [i] != _fichas [i].transform.position) {	//Repasamos las posiciones actuales y sólo que
				return;														//una ya no tenga la misma posición que la inicial
																			//salimos de la función
			}
		}
		fichaEscondida.gameObject.SetActive (true);	//Si hemos llegado a este punto es que hemos resuelto el puzzle
		print ("Puzzle resuelto!");
		textoGanador.gameObject.SetActive (true);
	}

	public void CambiarNumFichas(){
		if (tamañoInput.text != "") {
			numFichasLado = int.Parse (tamañoInput.text);
		}
	}

	public void Salir(){
		Application.Quit ();
	}

}

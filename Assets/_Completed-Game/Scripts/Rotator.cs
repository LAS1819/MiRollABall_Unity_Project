// Lluís Aracil Sabater - PMDM DAM2 21/22 BLOQUE II UNITY P2
using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	// Miembro público para controlar la rotación de los pickups
	//así podemos acceder a los valores desde el editor
	public Vector3 giro;

	// Preparamos un arraylist de objetos para guardar cada Pickup en escena
	// public ArrayList pickUps = new ArrayList();

	// Preparamos enteros que usaremos para dar vectores aleatorios a cada pickUp
	int vectorAleatorioX = 0;
	int vectorAleatorioY = 0;
	int vectorAleatorioZ = 0;

	// Método lanzado en la creación de cada objeto
	void Start () {

		// DEBUG
		//Debug.Log(gameObject.name);
		// if (gameObject) {
		// 	Debug.Log("Hola!");
		// 	// vectorAleatorioX = obtieneNumeroAleatorio();
		// 	// vectorAleatorioY = obtieneNumeroAleatorio();
		// 	// vectorAleatorioZ = obtieneNumeroAleatorio();
		// 	vectorAleatorioX = Random.Range(1, 100);
		// 	vectorAleatorioY = Random.Range(1, 100);
		// 	vectorAleatorioZ = Random.Range(1, 100);
		// 	//giro = new Vector3(15, 30, 45);
		// 	giro = new Vector3(vectorAleatorioX, vectorAleatorioY, vectorAleatorioZ);
		// }
		//gameObject.transform.
		//Debug.Log("Length: " + gameObject.GetComponents());
		// FIN DEBUG

		vectorAleatorioX = Random.Range(1, 100);
		vectorAleatorioY = Random.Range(1, 100);
		vectorAleatorioZ = Random.Range(1, 100);
		//giro = new Vector3(15, 30, 45);
		giro = new Vector3(vectorAleatorioX, vectorAleatorioY, vectorAleatorioZ);

	}

	// Before rendering each frame..
	void Update () 
	{
		// Rotate the game object that this script is attached to by 15 in the X axis,
		// 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
		// rather than per frame.
		transform.Rotate (giro * Time.deltaTime);

	}

	// Función que devuelve un número aleatorio
	// int obtieneNumeroAleatorio() {
	// 	int numeroAleatorio = 1;

	// 	// Random r = new Random();
	// 	// int rInt = r.Next(0, 100); //for ints

	// 	return numeroAleatorio;
	// }
}	
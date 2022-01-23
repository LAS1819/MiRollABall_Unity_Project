// Lluís Aracil Sabater - PMDM DAM2 21/22 BLOQUE II UNITY P2
using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	// Miembro público para controlar la rotación de la bola
	//así podemos acceder a los valores desde el editor
	public Vector3 giro = new Vector3(15, 30, 45);

	// Before rendering each frame..
	void Update () 
	{
		// Rotate the game object that this script is attached to by 15 in the X axis,
		// 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
		// rather than per frame.
		transform.Rotate (giro * Time.deltaTime);

	}
}	
// Lluís Aracil Sabater - PMDM DAM2 21/22 BLOQUE II UNITY P2

using UnityEngine;
using UnityEngine.UI; // Namespace required to use Unity UI
using UnityEngine.SceneManagement; // Namespace SceneManagement para reiniciar escena
using System.Collections;

// Clase para el control del jugador
public class PlayerController : MonoBehaviour {

	// TEXTOS
	public Text countText; // Texto para el contador de Pick Ups
	public Text winText; // Texto al ganar juego
	public Text looseText; // Texto al perder juego
	public Text livesCountText; // Texto para mostrar las vidas

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;

	// Miembro tipo Rederer para acceder al color del material
	private Renderer r;

	// JUGADOR
	// Variable privada para guardar escala del player
	private Vector3 playerScale;
	// Variable privada para guardar la posición del jugador
	private Vector3 playerPosition;
	// Variables para contener la posición inicial del jugador
	private float initPlayerPositionX;
	private float initPlayerPositionY;
	// Booleano para saber si el jugador está cayendo
	private bool playerFalling;
	// Vidas del jugador
	private int playerLives;
	// Variable pública para indicar la fuerza del salto
	public float fuerzaSalto = 5;
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;

	// PAREDES
	// Variable para obtener el objeto de la pared golpeada
	private GameObject paredGolpeada;
	// Variable privada para obtener la escala de la pared golpeada
	private Vector3 wallScale;
	// Variable para obtener la posición de la pared golpeada
	private Vector3 wallPosition;
	// Variable para obtener el nombre de la escena actual
	private string sceneName;
	// Atributo para la explosión accesible desde el editor
	[SerializeField] Transform prefabWallExplosion;

	// -----------------------------------------
	// SONIDOS
	// Variable para obtener el componente de sonido
	private AudioSource playerAudio;

	// Preparamos variables públicas para guardar los sonidos del jugador
	public AudioClip jumpAudio; // Salto
	public AudioClip pickUpAudio; // Recoger Pick Up
	public AudioClip wallCrash; // Golpear la pared
	public AudioClip playerFall; // Caída de jugador por precipicio
	

	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Obtenemos el componente Renderer y asignamos a variable privada 'r'
		r = GetComponent<Renderer>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// TEXTOS
		// Inicializamos variables
		winText.text = "";
		looseText.text = "";
		playerLives = 3;
		livesCountText.text = "Vidas: " + playerLives.ToString();


		// Obtenemos el componente AudioSource del player
		playerAudio = GetComponent<AudioSource>();

		// Damos valor falso al booleano que controla si el jugador está cayendo
		playerFalling = false;

		// Recogemos el nombre de la escena
		sceneName = SceneManager.GetActiveScene().name;

		// Recogemos la posición inicial del jugador
		// Sólo necesitamos los ejes X e Y, ya que está en el centro
		initPlayerPositionX = transform.position.x;
		initPlayerPositionY = transform.position.y;
	}

	private void Update() {

		// Guardamos posición Y del jugador
		playerPosition = transform.position;

		// DEBUG
		//Debug.Log("Posición del jugador: " + playerPosition);
		//Debug.Log("Posición Y del jugador: " + playerPosition.y);
		// FIN DEBUG

		// Si se pulsa el botón de saltar y no está ya en el aire
		if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.1f) {
			// Generamos la acción de salto
			rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);

			// Instanciamos el clip de audio del salto
			playerAudio.clip = jumpAudio;
			// Reproducimos sonido del salto
			playerAudio.Play();
		}

		// Si el eje Y del jugador es menor a -1, lanzamos sonido de caída
		// Lo lanzamos sólo una vez para que no haya un loop del sonido, esto lo conseguimos gracias al BOOL
		if (playerPosition.y < -1f && playerFalling == false) {
			// Instanciamos sonido de caída
			playerAudio.clip = playerFall;
			// Reproducimos sonido de caída
			playerAudio.Play();

			// Advertimos de que el jugador está ya cayendo
			// Así no vuelve a entrar en esta condición
			playerFalling = true;

			
		}

		// Cuando el jugador sobrepase los 30 negativos en eje y
		if (playerPosition.y < -30f) {

			// DEBUG
			Debug.Log(sceneName);
			// FIN DEBUG

			// Llamamos a función para perder vida y reunicar jugador
			PerderVidaYReubicar();
		}
	}

	// Each physics step..
	void FixedUpdate ()
	{
		if (playerLives > 0) {
			// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

			// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
			// multiplying it by 'speed' - our public player speed that appears in the inspector
			rb.AddForce (movement * speed);
		}
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// DEBUG
			//Debug.Log(GetComponents<AudioSource>());
			//Component[] componentesAudioSource;
			//componentesAudioSource = GetComponents<AudioSource>();
			// Debug.Log("Primer componente: " + componentesAudioSource[0].GetComponent<AudioSource>().clip);
			// Debug.Log("Segundo componente: " + componentesAudioSource[1].GetComponent<AudioSource>().clip);
			// Debug.Log(playerAudio);
			// Debug.Log(playerAudio.name);
			// Debug.Log("Clip: " + playerAudio.clip);
			//playerAudio.clip = "Jump-SoundBible.com";
			// FIN DEBUG

			// Reproducimos sonido al recoger Pick Up
			//GetComponent<AudioSource>().Play();
			// Instanciamos el clip de audio de recogida de PickUps
			playerAudio.clip = pickUpAudio;
			// Reproducimos el sonido
			playerAudio.Play();

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();

			// Obtenemos la escala del jugador
			playerScale = transform.localScale;

			// Si la escala es menor a 2
			if (playerScale.x < 2) {
				// Aumentamos la escala de la bola en 0.1 en todos sus vectores
				transform.localScale = new Vector3(playerScale.x + 0.1f, playerScale.y + 0.1f, playerScale.z + 0.1f);
			}


		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		// Check if our 'count' is equal to or exceeded 12
		if (count >= 12) 
		{
			// Set the text value of our 'winText'
			winText.text = "You Win!";

			// Invocamos a la función para reiniciar juego en 5 segundos
			Invoke("resetGame", 5);
		}
	}

	// Método para detectar las colisiones en las paredes
	private void OnCollisionEnter(Collision collision) {
		// Si la colisión se produce contra cualquiera de las paredes
		if (collision.gameObject.CompareTag("Pared")) {
			// Cambiamos el color de la bola a uno aleatorio
			//r.material.color = new Color(Random.value, Random.value, Random.value);
			r.material.color = new Color(Mathf.Round(Random.value), Mathf.Round(Random.value), Mathf.Round(Random.value));

			// Obtenemos el objeto de la pared golpeada
			paredGolpeada = collision.gameObject;

			// Instanciamos explosión
			// Subimos la posición del objeto para que parezca que viene de la pared
			Transform explosion = Instantiate(prefabWallExplosion,
								            new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
											Quaternion.identity);

			// Instanciamos el sonido para reproducir
			playerAudio.clip = wallCrash;
			// Reproducimos el sonido
			playerAudio.Play();

			// Destruimos la explosión pasado un segundo
			Destroy(explosion.gameObject, 1f);

			// DEBUG
			//Debug.Log("Pared golpeada");
			//Debug.Log(paredGolpeada);
			//Debug.Log(paredGolpeada.name);
			// FIN DEBUG

			// Comprobamos mediante el nombre del objeto, qué pared ha sido golpeada
			// De esta manera evitamos usar 4 etiquetas de pared y dejamos la ya creada "Pared"
			// Si la pared golpeada es la norte o la sur
			if (paredGolpeada.name == "North Wall" || paredGolpeada.name == "South Wall") {
				// Reducimos tamaño del jugador
				// Obtenemos la escala actual del objeto Bola mediante 'localScale'
				playerScale = transform.localScale;
				// Como reducimos todos sus vectores por igual, antes comprobamos que la bola sea mayor a 1
				if (playerScale.x > 0.2f) {
					// Reducimos la escala de la bola 0.1 en todos sus vectores
					transform.localScale = new Vector3(playerScale.x - 0.1f, playerScale.y - 0.1f, playerScale.z - 0.1f);	
				}	
			}

			// Si la pared golpeada es la este o la oeste
			if (paredGolpeada.name == "East Wall" || paredGolpeada.name == "West Wall") {
				// Aumentamos el tamaño del jugador
				// Obtenemos la escala actual del objeto
				playerScale = transform.localScale;
				if (playerScale.x < 2) {
					// Aumentamos la escala de la bola en 0.1 en todos sus vectores
					transform.localScale = new Vector3(playerScale.x + 0.1f, playerScale.y + 0.1f, playerScale.z + 0.1f);
				}
			}		

			// Obtenemos posición y escala de la pared golpeada
			wallPosition = paredGolpeada.transform.position;
			wallScale = paredGolpeada.transform.localScale;

			// DEBUG
			//Debug.Log("Escala de la pared golpeada: " + wallScale);
			//Debug.Log("Posición de pared golpeada: " + wallPosition);
			// FIN DEBUG

			// Comprobamos que la escala y no sea negativo, lo dejamos a uno como máximo
			if (wallScale.y > 1) {
				// Disminuimos altura de la pared y reposicionamos pared
				paredGolpeada.transform.localScale = new Vector3(wallScale.x, wallScale.y - 1, wallScale.z);
				paredGolpeada.transform.position = new Vector3(wallPosition.x, wallPosition.y - 0.5f, wallPosition.z);
			}
		}

		// Si la colisión se produce en la pared Norte o Sur
		// if (collision.gameObject.CompareTag("ParedNorte") || collision.gameObject.CompareTag("ParedSur")) {
		// 	// Reducimos tamaño del jugador
		// 	// Primero obtenemos la escala actual del objeto Bola mediante 'localScale'
		// 	playerScale = transform.localScale;
		// 	// Como reducimos todos sus vectores por igual, antes comprobamos que la bola sea mayor a 1
		// 	if (playerScale.x > 0.2f) {
		// 		// Reducimos la escala de la bola 0.1 en todos sus vectores
		// 		transform.localScale = new Vector3(playerScale.x - 0.1f, playerScale.y - 0.1f, playerScale.z - 0.1f);	
		// 	}
		// }

		// Si la colisión de produce en las pareder Este u Oeste
		// if (collision.gameObject.CompareTag("ParedEste") || collision.gameObject.CompareTag("ParedOeste")) {
		// 	// Aumentamos el tamaño del jugador
		// 	playerScale = transform.localScale;
		// 	if (playerScale.x < 2) {
		// 		// Aumentamos la escala de la bola en 0.1 en todos sus vectores
		// 		transform.localScale = new Vector3(playerScale.x + 0.1f, playerScale.y + 0.1f, playerScale.z + 0.1f);
		// 	}
		// }

		// Si la colisión se produce en la pared Norte
		// if (collision.gameObject.CompareTag("ParedNorte")) {

		// 	paredGolpeada = collision.gameObject;

		// 	// DEBUG
		// 	Debug.Log("Pared Norte golpeada");
		// 	Debug.Log(paredGolpeada);
		// 	Debug.Log(paredGolpeada.name);
		// 	// FIN DEBUG

		// 	// Obtenemos posición y escala de la pared golpeada
		// 	wallPosition = paredGolpeada.transform.position;
		// 	wallScale = paredGolpeada.transform.localScale;

		// 	// DEBUG
		// 	Debug.Log("Escala de la pared golpeada: " + wallScale);
		// 	Debug.Log("Posición de pared golpeada: " + wallPosition);
		// 	// FIN DEBUG

		// 	// Comprobamos que la escala y no sea negativo, lo dejamos a uno como máximo
		// 	if (wallScale.y > 1) {
		// 		// Disminuimos altura de la pared y reposicionamos pared
		// 		paredGolpeada.transform.localScale = new Vector3(wallScale.x, wallScale.y - 1, wallScale.z);
		// 		paredGolpeada.transform.position = new Vector3(wallPosition.x, wallPosition.y - 0.5f, wallPosition.z);
		// 	}	
		// }
	}

	// Función para reducir la vida del jugador
	private void PerderVidaYReubicar() {
		// DEBUG
		Debug.Log("Una vida menos...");
		// FIN DEBUG

		// Quitamos una vida
		playerLives--;
		// Actualizamos texto con Vidas
		livesCountText.text = "Vidas: " + playerLives.ToString();
		// Si las vidas llegan a 0
		if (playerLives == 0) {
			// DEBUG
			Debug.Log("Partida terminada...");
			// FIN DEBUG
			// Añadimos texto a 'looseText'
			looseText.text = "Game over!";

			// Invocamos a la función para reiniciar juego en 5 segundos
			Invoke("resetGame", 5);

		}

		// Reubicamos al jugador
		transform.position = new Vector3(initPlayerPositionX,
										initPlayerPositionY,
										0);
		playerFalling = false;
		rb.velocity = new Vector3(0, 0, 0);

	}

	// Función para reiniciar el juego
	private void resetGame() {
		// Reiniciamos el juego
		SceneManager.LoadScene(sceneName);
	}

}
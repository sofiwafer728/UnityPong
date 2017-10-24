using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Pong: Instrucciones
Controles P1 flecha Arriba y flecha abajo
Controles P2 W y S

Version vs pc, proximamente

Ejecutar en resolucion 1024x768, en ventana
 */


public class PongMain : MonoBehaviour {

	public float padSpeed = 8.0f;
	public float ballForceValue = 200.0f;
	public Rigidbody2D player1;
	public Rigidbody2D player2;

	bool isTwoPlayerGame = true;

	Rigidbody2D ball;

	void Start () {
		ball = GetComponent<Rigidbody2D>();
		RunBall(Vector2.left);
	}

	void Update () {
		//Player 1 movement
		if(Input.GetKey(KeyCode.UpArrow))
			player1.velocity = padSpeed * Vector2.up;

		if(Input.GetKey(KeyCode.DownArrow))
			player1.velocity = padSpeed * Vector2.down;

		if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
			player1.velocity = Vector2.zero;

		//Player 2 movement
		if(Input.GetKey(KeyCode.W))
			player2.velocity = padSpeed * Vector2.up;

		if(Input.GetKey(KeyCode.S))
			player2.velocity = padSpeed * Vector2.down;

		if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
			player2.velocity = Vector2.zero;
	}

	//Detecta cuando se atravieza un collider con el flag "isTrigger" activado
	void OnTriggerEnter2D(Collider2D other)
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		if(other.name.Contains("P1")){
			//Reinicia del lado del jugador 2
			transform.position = new Vector2(3.0f,0);
			RunBall(Vector2.right);
		}
		else {
			//Reinicia del lado del jugador 1
			transform.position = new Vector2(-3.0f,0);
			RunBall(Vector2.left);
		}

	}

	//Detecta cuando colisiona con un objeto
	void OnCollisionEnter2D(Collision2D other){
		 if (other.gameObject.name.Contains("P1")) {
			// Calculate hit Factor
			float y = positionInPad(transform.position,
								other.transform.position,
								other.collider.bounds.size.y);
			//La nueva direccion se genera a partir de la posicion en el pad
			//Se usa el vector unitario que viene de .normalized
			Vector2 dir = new Vector2(1, y).normalized;

			RunBall(dir);
		}
		if (other.gameObject.name.Contains("P2")) {
			// Calculate hit Factor
			float y = positionInPad(transform.position,
								other.transform.position,
								other.collider.bounds.size.y);

			Vector2 dir = new Vector2(-1, y).normalized;
		
			RunBall(dir);
    	}
	}

	//Retorna la posicion y en el pad de longitud 1, es decir si esta al medio devuelve 0.5
	float positionInPad(Vector2 ballPos, Vector2 padPos, float padHeight) {
		return (ballPos.y - padPos.y) / padHeight;
	}

	//Agrega una fuerza en la pelota
	public void RunBall(Vector2 direction){
		ball.velocity = Vector2.zero;
		ball.AddForce(direction * ballForceValue);
	}
}

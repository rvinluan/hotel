using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TouchManager : MonoBehaviour {
  GameStateManager gsm;
  public GameObject congratsObjectPrefab;
	public GameObject messageObjectPrefab;
	public GameObject gameOverObjectPrefab;
	public GameObject youWinObjectPrefab;
  GameObject modal;
	GameObject message;
  private GameObject thePick;
	public float timeToGameOver;
	private float timer;
	private bool foundThePick;
	private Vector3 mouseDown;
	private Vector3 mouseUp;
	public float mouseDelta;
	private bool clickOver;
	private int maxGuesses = 3;
	private int numGuesses = 0;

	//private bool loadGameOver;

	// Use this for initialization
	void Start () {
		GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
		timer = timeToGameOver;
		clickOver = false;
		//loadGameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
  	//check for touches
    for(int i = 0; i < Input.touchCount; i++) {
      Touch touch = Input.GetTouch(i);
      if (touch.phase == TouchPhase.Ended && touch.tapCount == 1) {
        Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
        registerClick(position);
      }
    }
    if(Input.GetMouseButtonDown(0)) {
			clickOver = false;
			mouseDown = Input.mousePosition;
		//	mouseUp = Vector3.zero;
      
    }
		if (Input.GetMouseButtonUp (0)) {
			mouseUp = Input.mousePosition;
			clickOver = true;
		//	mouseDown = Vector3.zero;
		}
		if (clickOver && Vector3.Distance (mouseDown, mouseUp) <= mouseDelta) {
				registerClick (mouseUp);
			  clickOver = false;
		}
//	if (foundThePick) {
//			timer -= Time.deltaTime;
//			//Debug.Log (timer);
//			if (timer < 0) {
//				SceneManager.LoadScene (2);
//		}
//
//	}


	}

  void registerClick(Vector3 pos) {
    Vector2 p = Camera.main.ScreenToWorldPoint(pos);
    Collider2D[] results = new Collider2D[5];
    int hitNum = Physics2D.OverlapPointNonAlloc(p, results);
    if(hitNum > 0 && numGuesses<=maxGuesses){
        Collider2D hit = results[0];
				thePick = hit.gameObject;
			if (numGuesses < maxGuesses) {
				createModal ();
			}
    }
		if (thePick.CompareTag("menu")){
			SceneManager.LoadScene (0);
		}
		if (thePick.CompareTag("new_game")){
			SceneManager.LoadScene (1);
		}
  }

	void createModal(){
		//first get location of target
		Vector3 hitxy = new Vector3(thePick.transform.position.x, thePick.transform.position.y, -10);
		Vector3 modalxy = hitxy;
		//barely in front of everyone
		modalxy.z = 1f;
		//find the center of the person
		modalxy.y += 1f;
		//start camera pan to hit location
		StartCoroutine(panTo(hitxy));

		// if the target hasn't yet been tapped (no children prefabs instantiated)
		// the we will build a modal


		if (thePick.transform.childCount == 0) {
			//first build a modal saying whether the guess is right or wrong

			modal = Instantiate (congratsObjectPrefab, modalxy, Quaternion.identity) as GameObject;
			modal.transform.parent = thePick.transform;
			numGuesses++;
			//then create proper message component
			// if you find the right one
			if (thePick.GetComponent<Person> ().isTheOne == true) {
				foundThePick = true;
				modal.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load ("congrats-yes", typeof(Sprite));
				message = Instantiate (youWinObjectPrefab, modalxy, Quaternion.identity) as GameObject;
			} 
			// if you didn't find the right one but still have guesses left
			else if(numGuesses<maxGuesses) {
				message = Instantiate (messageObjectPrefab, modalxy, Quaternion.identity) as GameObject;
				message.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load ("message_try_again", typeof(Sprite));
			}
			// if you didn't find the right one and no guesses left
			else if(numGuesses==maxGuesses) {
				message = Instantiate (gameOverObjectPrefab, modalxy, Quaternion.identity) as GameObject;
			}
				
			message.transform.parent = thePick.transform;



			StartCoroutine (growCongratsModal ());
			Vector3 message_position = message.gameObject.transform.position;
			thePick.transform.position= new Vector3 (thePick.transform.position.x, thePick.transform.position.y, -8);
			message.gameObject.transform.position = new Vector3 (message_position.x,message_position.y-1.5f, message_position.z-8);

		}
		// GameStateManager.timeFrozen = true;

		if (numGuesses == maxGuesses||foundThePick) {
			//SceneManager.LoadScene (2);
			GameStateManager.timeFrozen = true;
		}
	}

  public IEnumerator panTo(Vector3 location) {
    float t = 0.0f;
    while(t < 1.0) {
      t += Time.deltaTime;
      Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, location, t);
      yield return null;
    }
    // Camera.main.transform.parent = thePick.transform;
  }

  public IEnumerator growCongratsModal() {
    float t = 0.0f;
    while(t < 1.0) {
      t += Time.deltaTime * 2;
      float scaleValue = Mathf.Lerp(0f, 1.0f, t);
      modal.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
			message.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
      yield return null;
    }
  }
}

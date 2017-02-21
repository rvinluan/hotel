using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
  GameStateManager gsm;
  public GameObject congratsObjectPrefab;
  GameObject modal;
  private GameObject thePick;

	// Use this for initialization
	void Start () {
		GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
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
      registerClick(Input.mousePosition);
    }
	}

  void registerClick(Vector3 pos) {
    Vector2 p = Camera.main.ScreenToWorldPoint(pos);
    Collider2D[] results = new Collider2D[5];
    int hitNum = Physics2D.OverlapPointNonAlloc(p, results);
    if(hitNum > 0){
        Collider2D hit = results[0];
        Vector3 hitxy = new Vector3(hit.gameObject.transform.position.x, hit.gameObject.transform.position.y, -10);
        Vector3 modalxy = hitxy;
        //barely in front of everyone
        modalxy.z = 1f;
        //find the center of the person
        modalxy.y += 1f;
        StartCoroutine(panTo(hitxy));
        modal = Instantiate(congratsObjectPrefab, modalxy, Quaternion.identity) as GameObject;
        modal.transform.parent = hit.gameObject.transform;
        StartCoroutine(growCongratsModal());
        thePick = hit.gameObject;
        thePick.transform.Translate(0,0,-1f, Space.World);
        GameStateManager.timeFrozen = true;
        if(thePick.GetComponent<Person>().isTheOne == true) {
          modal.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("congrats-yes", typeof(Sprite));
        }
    }
  }

  public IEnumerator panTo(Vector3 location) {
    float t = 0.0f;
    while(t < 1.0) {
      t += Time.deltaTime;
      Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, location, t);
      yield return null;
    }
    Camera.main.transform.parent = thePick.transform;
  }

  public IEnumerator growCongratsModal() {
    float t = 0.0f;
    while(t < 1.0) {
      t += Time.deltaTime * 2;
      float scaleValue = Mathf.Lerp(0f, 2.0f, t);
      modal.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
      yield return null;
    }
  }
}

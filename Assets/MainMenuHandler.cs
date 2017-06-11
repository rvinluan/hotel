using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuHandler : MonoBehaviour {

	private bool isLoadingScene=false;

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Mouse position: " + Input.mousePosition);
			if (isLoadingScene) {
				return;
			}
			registerClick (Input.mousePosition);
		}

	}

	void registerClick(Vector3 pos){
		Debug.Log ("in register click");
		Vector2 p = Camera.main.ScreenToWorldPoint(pos);
		Debug.Log ("p: " + p);

		Collider2D[] results = new Collider2D[5];
		int hitNum = Physics2D.OverlapPointNonAlloc(p, results);

		Debug.Log ("hitNum: "+ hitNum);
		if (hitNum > 0) {
			Collider2D hit = results [0];
			GameObject target = hit.gameObject;

			if (target.CompareTag ("exit")) {
				isLoadingScene = true;
				Application.Quit ();
			}
			if (target.CompareTag ("new_game")) {
				isLoadingScene = true;
				SceneManager.LoadScene ("Gameplay");
			}
		}
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonManager : MonoBehaviour {
  public GameObject personPrefab;
  private List<Person> people;
  private Person theOne;

	// Use this for initialization
	void Start () {
    people = new List<Person>();
	 for(int i = 0; i < 50; i++) {
    Map map = GameObject.Find("Map").GetComponent<Map>();
    int randomFloor = Random.Range(0, map.floors.Length);
    Room rr = map.floors[randomFloor][ Random.Range(0, map.floors[randomFloor].Count) ];
    Vector3 pos = new Vector3(rr.transform.position.x + rr.width*0.5f, rr.transform.position.y, 0);
    GameObject pp = Instantiate(personPrefab, pos, Quaternion.identity) as GameObject;
    pp.transform.parent = transform;
    people.Add(pp.GetComponent<Person>());
   }
	}
	
	// Update is called once per frame
	void Update () {
	}
}

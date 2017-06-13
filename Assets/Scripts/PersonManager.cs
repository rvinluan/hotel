using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonManager : MonoBehaviour {
  public GameObject personPrefab;
  public Person theOne;
  private List<Person> people;
	private int numPeople = 25;

	void Start () {
    people = new List<Person>();
    float slightZdiff = 0;
	 for(int i = 0; i < numPeople; i++) {
    Map map = GameObject.Find("Map").GetComponent<Map>();
    //assigns random floor, then x position on floor
	int randomFloor = Random.Range(0, map.floors.Length);
    Room rr = map.floors[randomFloor][ Random.Range(0, map.floors[randomFloor].Count) ];
    Vector3 pos = new Vector3(rr.transform.position.x + rr.width*0.5f, rr.transform.position.y, slightZdiff);
	// z ordering of all characters so they don't collide
	// prevents flickering when on same plane
    slightZdiff += 0.5f/50;
    GameObject po = Instantiate(personPrefab, pos, Quaternion.identity) as GameObject;
    po.transform.parent = transform;
    Person pp = po.GetComponent<Person>();
    pp.currentFloor = randomFloor;
	// add to list of all people for reference, used for tap check
    people.Add(pp);
   }
	// WHERE WE PICK THE ONE
   theOne = people[ Random.Range(0, people.Count) ];
   theOne.isTheOne = true;  
	}
	
	// Update is called once per frame
	void Update () {
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {
  public float walkingSpeed = 0.1f;
  private List<Destination> route = new List<Destination>();
  private int currentStop;
  private int currentFloor;
  private int timer;
  private Activity currentlyDoing;
  private Map map;

  Destination generateRandomDestination() {
    List<Room> randomFloor = map.floors[Random.Range(0, map.floors.Length)];
    Room randomRoom = randomFloor[0];
    Destination d = new Destination();
    d.room = randomRoom;
    d.activity = Activity.Nothing;
    d.time = 100;
    return d;
  }

	// Use this for initialization
	void Start () {
    map = GameObject.Find("Map").GetComponent<Map>();
    currentStop = 0;
    for(int i = 0; i <= 5; i++) {
      route.Add(generateRandomDestination());
    }
	}
	
	// Update is called once per frame
	void Update () {
	 // FollowRoute();
	}

  void FollowRoute () {
    //if you're in transit
    if (currentlyDoing == Activity.Transit) {
      int nextStop = currentStop < route.Count - 1 ? currentStop + 1 : 0;
      Room destination = route[nextStop].room;
      //if you've reached your destination
      if (findRoomPlayerIsIn() == destination) {
        currentStop++;
        if(currentStop >= route.Count) {
          currentStop = 0;
        }
        currentlyDoing = route[currentStop].activity;
        //start doing your thing!
      } else { //if you're not at your destination yet
        if (currentFloor != destination.floor) {
          if (currentlyOnStairs()) {
            if (doneWithStairs(destination)) {
              currentFloor = destination.floor;
              return;
            }
            //walk up stairs
            transform.Translate(0, walkingSpeed, 0);
          } else {
            //walk towards stairs
            transform.Translate(-walkingSpeed, 0, 0);
          }
        } else {
          //walk towards destination
          Vector3 delta = destination.transform.position - transform.position;
          transform.Translate(delta.x * walkingSpeed, 0, 0);
        }
      }
    } else {
      if (timer >= route[currentStop].time) {
        timer = 0;
        currentlyDoing = Activity.Transit;
      } else {
        timer++;
        //whatever animation
      }
    }
  }

  public Room findRoomPlayerIsIn() {
    return new Room();
  }

  public bool currentlyOnStairs() {
    return transform.position.x < 375 || transform.position.x > 4125;
  }

  public bool doneWithStairs(Room destination) {
    if (currentFloor > destination.floor) {
      //going down
      return transform.position.y <= destination.transform.position.y;
    } else {
      //going up
      return transform.position.y >= destination.transform.position.y;
    }
  }
}

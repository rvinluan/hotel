using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {
  public float walkingSpeed = 0.05f;
  private List<Destination> route = new List<Destination>();
  public int currentStop;
  public int currentFloor;
  public int timer;
  public Activity currentlyDoing;
  private Map map;

  Destination generateRandomDestination() {
    int r = Random.Range(0, map.floors.Length);
    List<Room> randomFloor = map.floors[r];
    Room randomRoom = randomFloor[ Random.Range(0, randomFloor.Count) ];
    Destination d = new Destination();
    d.room = randomRoom;
    d.activity = Activity.Nothing;
    d.time = 100;
    return d;
  }

	// Use this for initialization
	void Start () {
    map = GameObject.Find("Map").GetComponent<Map>();
    currentStop = -1;
    currentFloor = 0;
    currentlyDoing = Activity.Transit;
    timer = 0;
    for(int i = 0; i <= 5; i++) {
      route.Add(generateRandomDestination());
    }
	}
	
	// Update is called once per frame
	void Update () {
	 FollowRoute();
	}

  void FollowRoute () {
    //if you're in transit
    if (currentlyDoing == Activity.Transit) {
      int nextStop = currentStop < route.Count - 1 ? currentStop + 1 : 0;
      Room destination = route[nextStop].room;
      //if you've reached your destination
      if (reachedDestination(destination)) {
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
            if (currentFloor > destination.floor) {
              //going down stairs
              transform.Translate(0, -walkingSpeed, 0);
            } else {
              //going up stairs
              transform.Translate(0, walkingSpeed, 0);
            }
          } else {
            //walk towards stairs
            transform.Translate(-walkingSpeed, 0, 0);
          }
        } else {
          //walk towards destination
          Vector3 roomCenter = destination.transform.position;
          roomCenter.x += destination.width*0.5f;
          Vector3 delta = roomCenter - transform.position;
          delta = Vector3.ClampMagnitude(delta, walkingSpeed);
          transform.Translate(delta.x, 0, 0);
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
    for(int j = 0; j < map.floors[currentFloor].Count; j++) {
      Room r = map.floors[currentFloor][j];
      float minX = r.transform.position.x;
      float maxX = minX + r.width;
      if( transform.position.x >= minX && transform.position.x <= maxX ) {
        return r;
      }
    }
    return null; //stairs
  }

  public bool currentlyOnStairs() {
    return transform.position.x < 3.75/2;
  }

  public bool reachedDestination(Room destination) {
    float center = (destination.transform.position.x + destination.width*0.5f);
    return findRoomPlayerIsIn() == destination && transform.position.x >= center - 0.2;
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

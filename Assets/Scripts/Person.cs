using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public enum SkinType {
  bear,
  cat,
  chicken,
  cow,
  duck,
  elephant,
  giraffe,
  hippopotamus,
  horse,
  leopard,
  lion,
  milkcow,
  monkey,
  panda,
  penguin,
  pig,
  rabbit,
  rhinoceros,
  sheep,
  tiger
}

public class Person : MonoBehaviour {
  
  private List<Destination> route = new List<Destination>();
  public int currentStop;
  public int currentFloor;
  public int timer;
  public Activity currentlyDoing;
  private Map mapRef;
  private SkeletonAnimation skeletonAnimation;
  public Spine.Skeleton skeleton;
  private bool facingLeft = true;
  public bool isTheOne = false;
  private float walkingSpeed;

  Destination generateRandomDestination() {
    int r = Random.Range(0, mapRef.floors.Length);
    List<Room> randomFloor = mapRef.floors[r];
    Room randomRoom = randomFloor[ Random.Range(0, randomFloor.Count) ];
    Destination d = new Destination();
    d.room = randomRoom;
    d.activity = Activity.Nothing;
    d.time = 300;
	d.roomTarget = Random.Range (0.2f, .8f);
    return d;
  }

	// Use this for initialization
	void Start () {
	//randomize character's walking speed
	walkingSpeed = Random.Range (0.01f, 0.03f);

	//assign character skin from skinType
    skeletonAnimation = GetComponent<SkeletonAnimation>();
    skeleton = skeletonAnimation.Skeleton;
    skeleton.SetSkin( ((SkinType)Random.Range(0, 20)).ToString() );
    //get reference to map
	mapRef = GameObject.Find("Map").GetComponent<Map>();
    // this stores where in current route-- when you start not yet at first stop, at spawn position
	currentStop = -1;
	// determines sprite animation, when you get to room then do animation assigned
	// but first start in transit
	// could start by idling
	
    currentlyDoing = Activity.Transit;
    timer = 0;
	// create 5 destinations in route
    for(int i = 0; i <= 5; i++) {
      route.Add(generateRandomDestination());
    }
	}
	
	// Update is called once per frame
	void Update () {
    if(!GameStateManager.timeFrozen || (GameStateManager.timeFrozen && isTheOne) ) {
      FollowRoute();
    }
    // if(isTheOne) {
    //   transform.localScale = new Vector3(1,1,1);
    // }
	}

  void FollowRoute () {
    //if you're in transit
    if (currentlyDoing == Activity.Transit) {
      int nextStop = currentStop < route.Count - 1 ? currentStop + 1 : 0;
      Room destinationRoom = route[nextStop].room;
      //if you've reached your destination
	  if (reachedDestination(route[nextStop])) {
        currentStop++;
        if(currentStop >= route.Count) {
          currentStop = 0;
        }
        currentlyDoing = route[currentStop].activity;
        //start doing your thing!
        skeletonAnimation.AnimationName = "idle";
      } else { //if you're not at your destination yet
        if (currentFloor != destinationRoom.floor) {
          if (currentlyOnStairs()) {
            if (doneWithStairs(destinationRoom)) {
              currentFloor = destinationRoom.floor;
              return;
            }
            //running on stairs because stairs are boring
            if (currentFloor > destinationRoom.floor) {
              //going down stairs
              transform.Translate(0, -walkingSpeed*5, 0);
            } else {
              //going up stairs
              transform.Translate(0, walkingSpeed*5, 0);
            }
            skeletonAnimation.AnimationName = "run";
          } else {
            //walk towards stairs
            if (transform.position.x <= 22.5) {
              //going to left stairs
              transform.Translate(-walkingSpeed, 0, 0);
              facingLeft = true;
            } else {
              //going to right stairs
              transform.Translate(walkingSpeed, 0, 0);
              facingLeft = false;
            }
            skeleton.flipX = !facingLeft;
            skeletonAnimation.AnimationName = "walk";
          }
        } else {
          //walk towards destination
		  //pick random point in bounds of room
		  //
		  //gets the leftmost position of destination room
          Vector3 targetLocation = destinationRoom.transform.position;
		  //adds offset for where in that room to go toward
		  targetLocation.x += route [nextStop].roomTarget * destinationRoom.width;
          Vector3 delta = targetLocation - transform.position;
          delta = Vector3.ClampMagnitude(delta, walkingSpeed);
          transform.Translate(delta.x, 0, 0);
          facingLeft = delta.x < 0;
          skeleton.flipX = !facingLeft;
          skeletonAnimation.AnimationName = "walk";
        }
      }
    } else {
      if (timer >= route[currentStop].time) {
        timer = 0;
        currentlyDoing = Activity.Transit;
      } else {
        timer++;
        //whatever animation
        skeletonAnimation.AnimationName = "idle";
      }
    }
  }

  public Room findRoomPlayerIsIn() {
    for(int j = 0; j < mapRef.floors[currentFloor].Count; j++) {
      Room r = mapRef.floors[currentFloor][j];
      float minX = r.transform.position.x;
      float maxX = minX + r.width;
      if( transform.position.x >= minX && transform.position.x <= maxX ) {
        return r;
      }
    }
    return null; //stairs
  }

  public bool currentlyOnStairs() {
    return transform.position.x < 3.75/2 || transform.position.x > (45.00 - (3.75/2));
  }

  public bool reachedDestination(Destination destination) {
		
	Vector3 target = destination.room.transform.position;
	target.x += destination.roomTarget * destination.room.width;
	target.z = 0;
	Vector3 delta = target - transform.position;
	delta.z = 0;
	Debug.DrawLine (transform.position, target);
	//Debug.Log ("Delta: "+delta);
	//Debug.Log ("Magnitude: "+delta.magnitude);
    return delta.magnitude <= 0.5f;
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

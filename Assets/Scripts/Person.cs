using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour {
  public float walkingSpeed = 1f;
  private List<Destination> route = new List<Destination>();
  private int currentStop;
  private int timer;
  private Activity currentlyDoing;
  private Random r = new Random();

  Destination generateRandomDestination() {
    System.Array rooms = System.Enum.GetValues(typeof(Room));
    System.Array activities = System.Enum.GetValues(typeof(Activity));
    Room randomRoom = (Room)Random.Range(0, rooms.Length);
    Destination d = new Destination();
    d.room = randomRoom;
    d.activity = Activity.Nothing;
    d.time = 100;
    return d;
  }

	// Use this for initialization
	void Start () {
	 currentStop = 0;
   for(int i = 0; i <= 5; i++) {
    route.Add(generateRandomDestination());
   }
	}
	
	// Update is called once per frame
	void Update () {
	 FollowRoute();
	}

  void FollowRoute () {
    //if currentlyDoing == Activity.Transit
      //if reached destination
        //currentStop++
        //currentlyDoing = route[currentStop].activity
        //kickoff whatever that animation is
      //else
        //if floor is different from curent floor
          //if already on stairs
            //stair animation
          //walk towards closest stairs
        //else
          //walk towards destination
    //else
      //if time spent = route[currentStop].time
        //timer = 0;
        //currentlyDoing = Activity.Transit
      //else
        //timer++
        //whatever animation
  }
}

using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {
  public float walkingSpeed = 1f;
  private Destination[] route;
  private int currentStop;
  private int timer;
  private Activity currentlyDoing;
  private Random r = new Random();

  Destination generateRandomDestination() {
    Array rooms = Enum.GetValues(typeof(Room));
    Array activities = Enum.GetValues(typeof(Activity));
    Room randomRoom = (Room)rooms.GetValue(r.Next(values.Length));
    // Activity randomActivity = (Activity)activities.GetValue(r.Next(values.Length));
  }

	// Use this for initialization
	void Start () {
	 currentStop = 0;
   for(int i = 0; i <= 5; i++) {

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

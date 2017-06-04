using UnityEngine;
using System.Collections.Generic;

public enum RoomType {
  Roof,
  Greenhouse,
  Gym,
  Office,
  Bedroom1a,
  Bedroom1b,
  Bedroom2a,
  Bedroom2b,
  Bedroom2c,
  Bedroom2d,
  Bedroom3a,
  Bedroom3b,
  Pool,
  Kitchen,
  Storage,
  Bar,
  Restaurant,
  Parking,
  GameRoom,
  Lobby,
  Elevator
}

public enum Activity {
  Standing,
  Talking,
  Nothing,
  Transit
}

public struct Destination {
  public Room room;
  public Activity activity;
  public int time;
	//where you're aiming to get to in the room
	// scalar from range 0 to 1 of room, relative to leftmost room position
	//used as multiplier of width
	public float roomTarget;
}

public class Map : MonoBehaviour {
  private static string[] map0 = new string[] {
    "Greenhouse|Roof",
    "Bedroom1a|Bedroom3a|Elevator|Bedroom1a|Bedroom3a",
    "Gym|Elevator|Restaurant",
    "Office|Elevator|Bedroom2a|Bedroom2a",
    "Pool",
    "Bar|Elevator|GameRoom",
    "Lobby",
    "Kitchen|Elevator|Storage"
  };
  private static string[] map1 = new string[] {
    "Greenhouse|Roof",
    "Gym|Elevator|Office",
    "Bedroom1a|Bedroom3b|Elevator|Bedroom2b|Bedroom2c",
    "Pool",
    "Bedroom3a|Bedroom1a|Elevator|Bedroom1b|Bedroom1a|Bedroom1b|Bedroom1a",
    "Bar|Elevator|Restaurant",
    "Lobby",
    "Parking",
    "Kitchen|Elevator|Storage"
  };

  private static string[] currentMap = map0;

  public List<Room>[] floors;
  public List<Room> allRooms;
  private Person theOne = null;

  //only plays from one stairwell, bottom
  //only need reference to single stairwell for audio purposes
  private GameObject stairwell = null; 

  public void initializeMap () {
    float highest = 0;
    float leftmost = 0;
	// i is  vertical for each floor in array
	// j is horizontal each room on floor
    for (int i = currentMap.Length - 1; i >= 0; i--) {
      // hs is floor height from bottom of building. floor 0 is first
	  int h = currentMap.Length - 1 - i;
      floors[h] = new List<Room>();
      string[] roomsOnFloor = currentMap[i].Split("|"[0]);
      for (int j = 0; j < roomsOnFloor.Length; j++) {
        if (j == 0) {
          //not on the roof though:
          //add stairs
          Vector3 stairpos = new Vector3(leftmost, highest, 5);
          GameObject stairpfab = Resources.Load("Room Prefabs/StairsLeft") as GameObject;
		  //quaternion.identity is default no rotation
          GameObject stairgo = Instantiate(stairpfab, stairpos, Quaternion.identity) as GameObject;
          leftmost += stairgo.GetComponent<SpriteRenderer>().bounds.size.x;
          if(i == currentMap.Length - 1) {
			//set stairwell reference to bottom stair game object
            stairwell = stairgo;
          }
		  //destroy stairs if on top floor
          if (i == 0) {
            Object.Destroy(stairgo);
          }
		  //for two rooms high, make extra set of stairs
          if(roomsOnFloor[j] == "Lobby" || roomsOnFloor[j] == "Parking" || roomsOnFloor[j] == "Pool") {
            stairpos.y += stairgo.GetComponent<SpriteRenderer>().bounds.size.y;
            GameObject stairgo2 = Instantiate(stairpfab, stairpos, Quaternion.identity) as GameObject;
          }
        }
		// next to last room drawn
        Vector3 pos = new Vector3(leftmost, highest, 5);
        GameObject pfab = Resources.Load("Room Prefabs/"+roomsOnFloor[j]) as GameObject;
        GameObject go = Instantiate(pfab, pos, Quaternion.identity) as GameObject;
        Room rm = go.GetComponent<Room>();
      	// not yet used
		//rm.roomID = "Test";

		// give string and get back roomtype
		// guarantees type strictness
        rm.type = (RoomType)System.Enum.Parse(typeof(RoomType), roomsOnFloor[j]);
		//easily get size of sprite and placement
		//bounds docs for more info
        Bounds sp = rm.gameObject.GetComponent<SpriteRenderer>().bounds;
        rm.width = sp.size.x;
        rm.height = sp.size.y;
        rm.floor = h;
		// add to the array by h, so floors[0] is bottom floor
        floors[h].Add(rm);
		// track all room for audio
        allRooms.Add(rm);
        leftmost += rm.width;
        if (j == roomsOnFloor.Length - 1) {
          //add end stairs
          Vector3 stairpos = new Vector3(leftmost, highest, 5);
          GameObject stairpfab = Resources.Load("Room Prefabs/StairsRight") as GameObject;
          GameObject stairgo = Instantiate(stairpfab, stairpos, Quaternion.identity) as GameObject;
          leftmost += stairgo.GetComponent<SpriteRenderer>().bounds.size.x;
          if(roomsOnFloor[j] == "Lobby" || roomsOnFloor[j] == "Parking" || roomsOnFloor[j] == "Pool") {
            stairpos.y += stairgo.GetComponent<SpriteRenderer>().bounds.size.y;
            GameObject stairgo2 = Instantiate(stairpfab, stairpos, Quaternion.identity) as GameObject;
          }
          if (i == 0) {
            Object.Destroy(stairgo);
          }
        }
        //add elevator
      }
      highest += floors[h][0].height;
      leftmost = 0;
    }
  }

  void Awake () {
    floors = new List<Room>[currentMap.Length];
    initializeMap();
  }

  void Start () {
  }

  void Update () {
    if(theOne == null) {
      theOne = GameObject.Find("PersonManager").GetComponent<PersonManager>().theOne;
    }
	//turn off all audio 
    for(int i = 0; i < allRooms.Count; i++) {
      AudioSource audio = allRooms[i].GetComponent<AudioSource>();
      audio.Pause();
    }
	//only turn on audio that is in the one's room
    for(int j = 0; j < floors[theOne.currentFloor].Count; j++) {
      Room rm = floors[theOne.currentFloor][j];
	  //audio source assigned to room in prefab
	  //change to attach multiple to prefab and then choose random in room script
      AudioSource audio = rm.GetComponent<AudioSource>();
	  //get center of room that the ONE is in
      Vector3 roomCenter = new Vector3(rm.transform.position.x + rm.width/2, rm.transform.position.y, 0);
      //how far the one is from center of room
	  float dist = Vector3.Distance(roomCenter, theOne.transform.position);
      
	  //11 is radius to audio
	  if(dist < 11) {
        if(!audio.isPlaying) {
          audio.Play(); 
        }
        audio.volume = Mathf.Lerp(1, 0.01f, dist/11);
      } else {
		//stop audio if too far
        audio.Pause();
      }
    }
    //always play the audio from the room you're in;
	//if not in a room, then you're in stairwell
	// it's possible for large rooms to be bigger than 11 radius, which means no sound will play
	// even if you're in the room
	//so we do one extra check 
    if(theOne.findRoomPlayerIsIn() == null) {
      if(!stairwell.GetComponent<AudioSource>().isPlaying) {
        stairwell.GetComponent<AudioSource>().Play();
      }
    } else {
      AudioSource a = theOne.findRoomPlayerIsIn().GetComponent<AudioSource>();
      if(!a.isPlaying) {
        a.Play();
		//better to hear full volume of sound than to lerp
        a.volume = 1;
      }
      stairwell.GetComponent<AudioSource>().Pause();
    }
  }
}

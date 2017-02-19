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
}

public class Map : MonoBehaviour {
  private static string[] map0 = new string[] {
    "Greenhouse|Roof",
    "Bedroom1a|Bedroom3a|Elevator|Bedroom1b|Bedroom3b",
    "Pool",
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

  public List<Room>[] floors;

  public void initializeMap () {
    float highest = 0;
    float leftmost = 0;
    for (int i = map1.Length - 1; i >= 0; i--) {
      int h = map1.Length - 1 - i;
      floors[h] = new List<Room>();
      string[] roomsOnFloor = map1[i].Split("|"[0]);
      for (int j = 0; j < roomsOnFloor.Length; j++) {
        if (j == 0) {
          //add stairs
          Vector3 stairpos = new Vector3(leftmost, highest, 5);
          GameObject stairpfab = Resources.Load("Room Prefabs/Stairs") as GameObject;
          GameObject stairgo = Instantiate(stairpfab, stairpos, Quaternion.identity) as GameObject;
          leftmost += stairgo.GetComponent<SpriteRenderer>().bounds.size.x;
          if(roomsOnFloor[j] == "Lobby" || roomsOnFloor[j] == "Parking" || roomsOnFloor[j] == "Pool") {
            stairpos.y += stairgo.GetComponent<SpriteRenderer>().bounds.size.y;
            GameObject stairgo2 = Instantiate(stairpfab, stairpos, Quaternion.identity) as GameObject;
          }
        }
        Vector3 pos = new Vector3(leftmost, highest, 5);
        GameObject pfab = Resources.Load("Room Prefabs/"+roomsOnFloor[j]) as GameObject;
        GameObject go = Instantiate(pfab, pos, Quaternion.identity) as GameObject;
        Room rm = go.GetComponent<Room>();
        rm.roomID = "Test";
        rm.type = (RoomType)System.Enum.Parse(typeof(RoomType), roomsOnFloor[j]);
        Bounds sp = rm.gameObject.GetComponent<SpriteRenderer>().bounds;
        rm.width = sp.size.x;
        rm.height = sp.size.y;
        rm.floor = h;
        floors[h].Add(rm);
        leftmost += rm.width + 0.2f;
        //add elevator
      }
      highest += floors[h][0].height + 0.2f;
      leftmost = 0;
    }
  }

  void Awake () {
    floors = new List<Room>[map1.Length];
    initializeMap();
  }

  void Start () {
  }

  void Update () {
  }
}

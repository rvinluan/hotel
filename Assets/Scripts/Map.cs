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
    "Bedroom1a|Bedroom1a|Bedroom1a|Bedroom1a|Elevator|Bedroom1a|Bedroom1a|Bedroom1a|Bedroom1a",
    "Pool",
    "Lobby",
    "Kitchen|Elevator|Storage"
  };
  private static string[] map1 = new string[] {
    "Greenhouse|Roof",
    "Gym|Elevator|Office",
    "Bedroom1|Bedroom3|Elevator|Bedroom2|Bedroom2",
    "Pool",
    "Bedroom3|Bedroom1|Elevator|Bedroom1|Bedroom1|Bedroom1|Bedroom1",
    "Bar|Elevator|Restaurant",
    "Lobby",
    "Parking",
    "Kitchen|Elevator|Storage"
  };

  public List<Room>[] floors;

  public void initializeMap () {
    float highest = 0;
    float leftmost = 0;
    for (int i = map0.Length - 1; i >= 0; i--) {
      int h = map0.Length - 1 - i;
      floors[h] = new List<Room>();
      string[] roomsOnFloor = map0[i].Split("|"[0]);
      for (int j = 0; j < roomsOnFloor.Length; j++) {
        if (j == 0 || j == roomsOnFloor.Length - 1) {
          //add stairs
        }
        Vector3 pos = new Vector3(leftmost, highest, 0);
        GameObject pfab = Resources.Load("Room Prefabs/"+roomsOnFloor[j]) as GameObject;
        GameObject go = Instantiate(pfab, pos, Quaternion.identity) as GameObject;
        Room rm = go.GetComponent<Room>();
        rm.roomID = "Test";
        rm.type = RoomType.Lobby;
        Bounds sp = rm.gameObject.GetComponent<SpriteRenderer>().bounds;
        rm.width = sp.size.x;
        rm.height = sp.size.y;
        rm.floor = i;
        floors[h].Add(rm);
        leftmost += rm.width;
        //add elevator
      }
      highest += floors[h][0].height;
      leftmost = 0;
    }
  }

  void Start () {
    floors = new List<Room>[map0.Length];
    initializeMap();
  }

  void Update () {
  }
}

using UnityEngine;
using System.Collections.Generic;

public enum Room {
  Roof,
  Greenhouse,
  Gym,
  Office,
  Bedroom1,
  Bedroom2,
  Bedroom3,
  Bedroom4,
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
  private static string[] firstmap = new string[] {
    "Greenhouse|Roof",
    "Gym|Elevator|Office",
    "Bedroom1|Bedroom3|Elevator|Bedroom2|Bedroom2",
    "Pool",
    "Bedroom4|Elevator|Bedroom1|Bedroom1|Bedroom1|Bedroom1",
    "Bar|Elevator|Restaurant",
    "Lobby",
    "Parking",
    "Kitchen|Elevator|Storage"
  };

  public List<Room>[] floors;

  public void initializeMap () {
    //for i = map1.length BACKWARDS
      //roomsOnFloor = map1[i].Split("|")
      //for roomsOnFloor
        //if j is first or last
          //add stairs
        //floors[i].Add(new Room(roomsOnFloor[j]))
        //calculate x and y
        //Instantiate(Resources.Load(Room.prefab))
        //add elevator
  }

  void Start () {
    initializeMap();
  }

  void Update () {
  }
}

public enum Room {
  Roof,
  Pool,
  Gym,
  Office,
  Bedrooms,
  Lobby,
  Restaurant,
  ParkingLot,
  Kitchen
}

public enum Activity {
  Standing,
  Talking,
  Transit
}

public struct Destination {
  Room room;
  Activity activity;
  int time;
}

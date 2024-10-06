using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    Default, // Single shot - done
    CircleShot, // Circle of bullets around player
    SpreadShot, // 3 bullet spread - done
    FastShot, // Faster, smaller bullet, less damage - done
    HeavyShot, // Slower, larger bullet, more damage - done
    TrackingShot, // Bullet moves towards closest enemy
    PiercingShot, // Bullet goes through one enemy
    AutomaticShot // Fires automatically - done
}

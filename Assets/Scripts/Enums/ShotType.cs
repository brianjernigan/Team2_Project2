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
    BouncingShot, // Bullet bounces off object once
    PiercingShot, // Bullet goes through one enemy
    ExplodingShot, // On hit, bullet explodes into 5 smaller bullets
    AutomaticShot // Fires automatically - done
}

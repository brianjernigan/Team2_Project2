using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    Default, // Single shot
    CircleShot, // Circle of bullets around player
    SpreadShot, // 3 bullet spread
    FastShot, // Faster bullet, less damage
    HeavyShot, // Slower bullet, more damage
    TrackingShot, // Bullet moves towards closest enemy
    BouncingShot, // Bullet bounces off object once
    PiercingShot, // Bullet goes through one enemy
    ExplodingShot, // On hit, bullet explodes into 5 smaller bullets
    AutomaticShot
}

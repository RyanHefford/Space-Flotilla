using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class README : MonoBehaviour
{
    //As some of the player's sensors were changed within the different models you need to change the sensor's params as follows
    //in order to run the brain

    //SIMPLE MODEL: Surround [Rays per direction = 10, Max Ray Degrees = 180, Ray Length = 20]
    //                          Front [Rays per direction = 15, Max Ray Degrees = 60, Ray Length = 20]
    //                          Space Size = 14

    //IMPROVED MODEL: Surround [Rays per direction = 10, Max Ray Degrees = 180, Ray Length = 20]
    //                          Front [Rays per direction = 15, Max Ray Degrees = 60, Ray Length = 20]
    //                          Space Size = 17


    //CURRICULUM MODEL: Surround [Rays per direction = 20, Max Ray Degrees = 180, Ray Length = 25]
    //                  Front [Rays per direction = 20, Max Ray Degrees = 60, Ray Length = 25]
    //                  Space Size = 55
}

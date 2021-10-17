using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Super simple class which plays a sound if something enters its collider
/// </summary>
public class SoundTrigger : MonoBehaviour
{
    /// <summary>
    /// Note that we could perform some tests here to check which object is colliding (e.g. checking the layer), and only play the sound in case of collisions 
    /// with specific objects. In the current code, any collision triggers the sound.
    /// </summary>
    private void OnTriggerEnter( Collider other )
    {
        Debug.Log( "Pins hit!" );
        GetComponent<AudioSource>().Play(); //if the object this script has been added to does not contain an AudioSource component, this line will throw an error
    }
}

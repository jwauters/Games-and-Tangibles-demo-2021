using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class detects whether pins have fallen over. It does this by increasing a counter when the spherical collider at the top of a pin
/// has hit the box collider on this GameObject, which is positioned low on the bowling lane. If all pins have been knocked over, we play the win animation.
/// 
/// Important! Check out Edit > Project Settings > Physics to see that the layer "BowlingPin" only detects collisions with other objects within this same
/// layer. The only objects that are on this layer are the pin colliders and the floor collider. That way, collisions with the bowling ball are not counted
/// towards the amount of fallen pins.
/// </summary>
public class FallDetector : MonoBehaviour
{
    //How many pins have we knocked over?
    private int fallenPinCount = 0;

    //Win effects
    public ParticleSystem winParticles;
    public AudioClip winSound;          //this is public, since we want to expose it in the inspector (editor window)
    private AudioSource audioPlayer;    //this is private, since it doesn't need to be exposed in the inspector

    //UI
    [SerializeField]
    private Text textPinCount;          //this way we can keep an attribute private, and still expose it in the inspector
    [SerializeField]
    private Text textVictory;


    /// <summary>
    /// Excuted once, upon starting the game
    /// </summary>
    private void Start()
    {
        //Finds the audiosource in the scene. Careful when using this, it is inefficient and should be used sparingly (i.e. never in an update method).
        //- GameObject.Find finds a gameobject with a certain name (in this case, there are other versions)
        //- We need to retrieve the audiosource component because that is the type of our attribute audioPlayer
        audioPlayer = GameObject.Find( "SoundTrigger" ).GetComponent<AudioSource>();

        //PS: we could have achieved the same effect by making the audioPlayer public and assigning it through the inspector window
    }

    /// <summary>
    /// This method is called whenever a collider enters this object's collider.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter( Collider other )
    {
        fallenPinCount++;
        textPinCount.text = fallenPinCount.ToString();

        if( fallenPinCount >= 10 )
        {
            winParticles.Play(); //play the particle system
            audioPlayer.PlayOneShot( winSound ); //play the win sound (PlayOneShot means it will not be repeated)
            textVictory.gameObject.SetActive( true ); //SetActive turns on the gameObject. Check the scene, it is turned off by default
        }
    }
}

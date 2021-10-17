using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switches between the bowling ball and the car
/// </summary>
public class MovableSwitcher : MonoBehaviour
{
    public SerialController arduinoController;

    //We are storing the bowlingBall with a reference to its GameObject, and the car with a reference to its CarController component.
    //Both are possible, but they have consequences when using them. See SwitchMovable() below.
    public GameObject bowlingBall;
    public CarController car;

    //Is the ball currently active? If no, the car is active.
    private bool ballActive = true;


    /// <summary>
    /// Switches the current movable object from the car to the bowling ball, or vice versa.
    /// </summary>
    public void SwitchMovable()
    {
        if( ballActive )
        {
            //Change the arduinoController's listener object to the car
            arduinoController.messageListener = car.gameObject;

            //Car is a reference to CarController which contains the Reset() method. So all is fine here
            car.Reset(); 
            //Here too. SetActive is a method from the class GameObject so since the bowlingBall is stored as a gameObject, this works
            bowlingBall.SetActive( false );
            //But here we need to retrieve the car's gameObject. Just doing car.SetActive( true ) would not compile, since the compiler 
            //sees car as an object of type CarController (which does not contains a definition for the method SetActive())
            //Note that this is the short form notation of car.GetComponent<GameObject>().SetActive( true ) 
            car.gameObject.SetActive( true );

            //Finally, we swap the boolean
            ballActive = false;
        }
        else
        {
            //Identical to above
            arduinoController.messageListener = bowlingBall.gameObject;

            //Here we have the inverse situation. bowlingBall is stored as a GameObject, so we first need to retrieve its BowlingBallController 
            //before we can call Reset.
            bowlingBall.GetComponent<BowlingBallController>().Reset();

            //The rest is the same as above (with booleans inverted of course)
            bowlingBall.SetActive( true );
            car.gameObject.SetActive( false );

            ballActive = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A class controlling the bowling ball's behavior. The ball is controlled by the physics engine.
/// </summary>
public class BowlingBallController : MonoBehaviour
{
    public float deltaZ; //ball speed (you can adjust this from the inspector, even while the game is running)
    private float prevAngle = 0; //previous angle of the controller (needed to calculate force)


    /// <summary>
    /// Check for keyboard input each frame, throw the ball if space has been pressed
    /// </summary>
    void Update()
    {        
        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            ThrowBall();
        }
    }

    /// <summary>
    /// Applies forward force to the ball
    /// </summary>
    private void ThrowBall()
    {
        Debug.Log( "Throwing ball!" );
        //Careful: avoid using GetComponent too often (such as in the Update loop), as it is relatively expensive
        this.GetComponent<Rigidbody>().AddForce( new Vector3( 0, 0, deltaZ ) );
    }

    /// <summary>
    /// Processes the message sent by the Arduino. Incoming messages have the following structure:
    /// Angle=73.45
    /// We cut the number off and convert it to a float. 
    /// Then we calculate the difference with the previous angle and use this amount of force to apply to the bowling ball.
    /// </summary>
    void OnMessageArrived( string msg )
    {
        //Split message
        string angleString = msg.Split( '=' )[1];

        //convert to float
        NumberFormatInfo fmt = new NumberFormatInfo();
        fmt.NegativeSign = "-";
        fmt.NumberDecimalSeparator = ".";
        float angle = float.Parse( angleString, fmt );

        //Calculate force based on difference with previous angle
        float force = ( prevAngle - angle ) * 50; //the 50 is an empirical constant
        prevAngle = angle;

        //Apply force to ball
        this.GetComponent<Rigidbody>().AddForce( new Vector3( force, 0, 0 ) );
    }

    /// <summary>
    /// Called upon connecting to the Arduino
    /// </summary>
    void OnConnectionEvent( bool success )
    {
        if ( success )
            Debug.Log( "Connection established" );
        else
            Debug.Log( "Connection attempt failed or disconnection detected" );
    }

    /// <summary>
    /// Resets the bowling ball to its intial position
    /// </summary>
    public void Reset()
    {
        transform.position = new Vector3( 0, 0.69f, 0.74f ); //hard-coded values are bad! A better approach would be to store this position as an attribute.
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //this line is needed to defocus the UI button after pressing it. If it is not included, pressing space will also cause the button to be pressed again, 
        //thereby generating a new ball in the center of the track right before it is thrown (try it!)
        EventSystem.current.SetSelectedGameObject( null, null );
    }
}

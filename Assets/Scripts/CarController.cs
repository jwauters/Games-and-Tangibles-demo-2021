using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A class controlling the car's behavior. Unlike the bowling ball, the car is not controlled by physics, but by
/// manipulating its transform directly. This gives you more precise control, while physics lead to more emergent 
/// behavior.
/// </summary>
public class CarController : MonoBehaviour
{
    public float forwardSpeed;  //forward driving speed
    public float backwardSpeed; //backward driving speed
    public float rotationSpeed; //rotation speed


    // Update is called once per frame
    void Update()
    {
        //Both ways of translating below do the same thing (in difference directions of course), but in a different way.
        //The both add/subtract a certain amount of distance along the car's local(!) forward axis.
        //Careful of the difference between the local and world coordinate systems!

        //Forward movement
        if ( Input.GetKey( KeyCode.UpArrow ) )
        {
            //Note the use of Time.deltaTime (the time since the last frame), this makes the speed independent of framerate
            //If you change Space.Self to Space.World, you will translate over the world coordinate system. 
            //This is independent of the car's rotation, and means it will always translate in the direction of the bowling track. Try it!
            transform.Translate( new Vector3( 0, 0, forwardSpeed * Time.deltaTime ), Space.Self );
        }

        //Backward movement
        if ( Input.GetKey( KeyCode.DownArrow ) )
        {
            transform.position += transform.forward * Time.deltaTime * -backwardSpeed; //negative value to move backward
        }

        //Both ways of rotating below do the same thing (in different directions of course), but in a different way. 
        //They both add/subtract rotationSpeed degrees of rotation over the y-axis.

        //Turn left
        if ( Input.GetKey( KeyCode.LeftArrow ) )
        {
            //Rotation using euler angles (standard 3-axis coordinate system). This is easy to read, but can be unsafe to use in certain cases (gimbal lock).
            transform.eulerAngles += new Vector3( 0, -rotationSpeed, 0 );
        }

        //Turn right
        if ( Input.GetKey( KeyCode.RightArrow ) )
        {
            //Rotation using quaternions. This is the internal representation Unity uses and is safer than euler angles - at the cost of being less intuitive.
            transform.rotation *= Quaternion.Euler( new Vector3( 0, rotationSpeed, 0 ) );
        }
    }

    /// <summary>
    /// Processes the Arduino message. See BowlingBallController.
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

        float angleDelta = -angle / 15; //15 is empirical constant to modify rotation speed with tangible

        //Adjust the car's rotation by angleDelta each time a message is received
        transform.rotation *= Quaternion.Euler( new Vector3( 0, angleDelta, 0 ) );
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
    /// Resets the car to its intial position
    /// </summary>
    public void Reset()
    {
        transform.position = new Vector3( 0, 0.69f, 0.74f ); //hard-coded values are bad! A better approach would be to store this position as an attribute.
        transform.rotation = Quaternion.identity; //reset rotation

        //this line is needed to defocus the UI button after pressing it. If it is not included, pressing space will also cause the button to be pressed again, 
        //thereby generating a new ball in the center of the track right before it is thrown (try it!)
        EventSystem.current.SetSelectedGameObject( null, null );
    }
}
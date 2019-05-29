using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;  // backgrounds to be parallaxed
    private float[] parallaxScales; // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;  // How smooth parallax is going to be. Make sure to set this above zero

    private Transform cam;   // Reference to the main cameras transform
    private Vector3 previousCamPosition;  // Position of the camera in the previous frame

    // Called before start() but after game objects are set up.  Great for setting up references
    void Awake() {
        cam = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start() {
        // the previous fame had the current frame's camera position
        previousCamPosition = cam.position;

        parallaxScales = new float[backgrounds.Length];

        // assigning corresponding parallaxScales
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < backgrounds.Length; i++) {
            // the parallax is the oposite of the camera movement because the previous frame multiplied by scale
            float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];

            // set a target x position which is the current position plus the parallax 
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target position which is the background's current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // set the previousCamPos to the camera's position at the end of the frame
        previousCamPosition = cam.position;
    }
}

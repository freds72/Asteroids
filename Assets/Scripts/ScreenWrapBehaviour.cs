using UnityEngine;
using System.Collections;

public class ScreenWrapBehaviour : MonoBehaviour {

    // We use ghosts in advanced wrapping to create a nice wrapping illusion
    Transform[] ghosts = new Transform[8];
    Renderer[] renderers;

    float screenWidth;
    float screenHeight;

	// Use this for initialization
	void Start () {
        // Fetch all the renderers that display ship graphics.
        // In the demo we only have one mesh for the ship and thus
        // only one renderer.
        // We could have a complicated ship, made out of several meshes
        // and this would fetch all the renderers.
        // We use the renderer(s) so we can check if the ship is
        // visible or not.
        renderers = GetComponentsInChildren<Renderer>();

        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        // The width is then equal to difference between the rightmost and leftmost x-coordinates
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        // The height, similar to above is the difference between the topmost and the bottom yycoordinates
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        CreateGhosts();
	}
	
	// Update is called once per frame
	void Update () {
        var cam = Camera.main;
        var newPosition = transform.position;

        // We need to check whether the object went off screen along the horizontal axis (X),
        // or along the vertical axis (Y).
        //
        // The easiest way to do that is to convert the ships world position to
        // viewport position and then check.
        //
        // Remember that viewport coordinates go from 0 to 1?
        // To be exact they are in 0-1 range for everything on screen.
        // If something is off screen, it is going to have
        // either a negative coordinate (less than 0), or
        // a coordinate greater than 1
        //
        // So, we get the ships viewport position
        var viewportPosition = cam.WorldToViewportPoint(transform.position);

        Debug.Log(string.Format("{0} / {1}", newPosition.x, newPosition.y));
        if (viewportPosition.x < 0 )
            newPosition.x += screenWidth;
        if (viewportPosition.x > 1)
            newPosition.x -= screenWidth;
        if (viewportPosition.y < 0)
            newPosition.y += screenHeight;
        if (viewportPosition.y > 1)
            newPosition.y -= screenHeight;


        //Apply new position
        transform.position = newPosition;
    }

    void CreateGhosts()
    {
        for (int i = 0; i < 8; i++)
        {
            // Ghost ships should be a copy of the main ship
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;

            // But without the screen wrapping component
            DestroyImmediate(ghosts[i].GetComponent<ScreenWrapBehaviour>());
        }

        PositionGhosts();
    }

    void PositionGhosts()
    {
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        var ghostPosition = transform.position;

        // We're positioning the ghosts clockwise behind the edges of the screen.
        // Let's start with the far right.
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }
    }
}

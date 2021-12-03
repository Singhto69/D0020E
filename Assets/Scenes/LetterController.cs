using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterController : MonoBehaviour
{
    public Canvas canvas;//Set in editor
    private Camera cam;
    private Vector2 mousePos = new Vector2();
    private Font arial;

    private enum UpDown { Down = -1, Start = 0, Up = 1 };
    private Text mousetext;
    private float textHeight;
    private float screenWidth;

    private List<GameObject> currentLetters = new List<GameObject>();
    private int AMOUNT_LETTERS = 3;

    void Start()
    {
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        cam = Camera.main;
        screenWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, cam.nearClipPlane)).x;

        // Create Mouse Letter
        GameObject textGO = new GameObject("TextObject");
        textGO.transform.SetParent(this.transform);
        textGO.AddComponent<Text>();
        mousetext = textGO.GetComponent<Text>();
        mousetext.font = arial;
        mousetext.fontSize = 80;
        mousetext.text = "A";
        mousetext.horizontalOverflow = HorizontalWrapMode.Overflow;
        mousetext.alignment = TextAnchor.MiddleCenter;
        mousetext.transform.localScale = Vector3.one;

        textHeight = ((RectTransform)mousetext.transform).rect.height;
    }

    void FixedUpdate()
    {
        Vector3 pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        mousetext.transform.position = pos;

        if(currentLetters.Count < AMOUNT_LETTERS)
        {
            currentLetters.Add(SpawnLetter());
        }

        //Reverse iterate to allow removing elements
        for(int i = currentLetters.Count-1; i>=0; i--)
        {
            GameObject letter = currentLetters[i];
            if (letter.transform.position.y < cam.ScreenToWorldPoint(new Vector3(0, 0 - textHeight, cam.nearClipPlane)).y)
            {
                Destroy(letter);
                currentLetters.RemoveAt(i);
            }
        }
    }

    //Spawn random letter at random location on screen
    private GameObject SpawnLetter()
    {
        // Create Gameobject Letter
        GameObject LetterObject = new GameObject("LetterObject");
        LetterObject.transform.SetParent(this.transform);
        LetterObject.AddComponent<Text>();
        LetterObject.AddComponent<Rigidbody2D>();

        // Vars
        Text letter = LetterObject.GetComponent<Text>();
        Rigidbody2D rigidbody = LetterObject.GetComponent<Rigidbody2D>();
        int letterOffset = Random.Range(0, 26);
        int startPosition = Random.Range(0+(int)textHeight/2, Screen.width-(int)textHeight/2);
        int direction = Random.Range(0,2)*2-1; //Random number 0 or 1, *2 == 0 or 2, -1 == -1 or 1 (Negative or positive)
        float yVelocity = Random.Range(12, 20);

        // Properties
        rigidbody.gravityScale = 2;
        letter.font = arial;
        letter.fontSize = 80;
        letter.horizontalOverflow = HorizontalWrapMode.Overflow;
        letter.alignment = TextAnchor.MiddleCenter;
        letter.transform.localScale = Vector3.one;
        letter.transform.position = cam.ScreenToWorldPoint(new Vector3(startPosition, 0, cam.nearClipPlane));

        // Behaviour
        rigidbody.velocity = new Vector2(0, yVelocity);
        float sideVelocity = Random.Range(0,MaxSideVelocity(LetterObject));
        rigidbody.velocity = new Vector2(sideVelocity*direction, rigidbody.velocity.y);

        //Change Velocity direction if letter will leave screen
        if (WillFallOffScreen(LetterObject))
        { 
            rigidbody.velocity = new Vector2(-rigidbody.velocity.x, rigidbody.velocity.y);
        }

        letter.text = char.ToString((char)('A' + letterOffset));

        return LetterObject;
    }

    private float MaxSideVelocity(GameObject letter)
    {
        Rigidbody2D rigidbody = letter.GetComponent<Rigidbody2D>();
        float yVelocity = rigidbody.velocity.y;
        float startPosition = letter.transform.position.x;
        float distanceToSide = Mathf.Max(screenWidth-startPosition, startPosition);

       // Debug.Log("To side: " + distanceToSide + " StartPosition: " + startPosition + " Width: " + screenWidth);

        //dx = Vx * (2Vy/g)  ==  Vx = (dx*g)/(2Vy)
        return (distanceToSide * Mathf.Abs(Physics2D.gravity.y) * rigidbody.gravityScale) / (2 * yVelocity);
    }

    private bool WillFallOffScreen(GameObject letter)
    {
        Rigidbody2D rigidbody = letter.GetComponent<Rigidbody2D>();
        Vector2 velocity = rigidbody.velocity;
        float startPosition = letter.transform.position.x;

        //x-Displacement = Vx * t == Vx * (2Vy/g)
        float sideDisplacement = velocity.x * (2*velocity.y / (Mathf.Abs(Physics2D.gravity.y)*rigidbody.gravityScale));
        float position = startPosition + sideDisplacement;
        float screenStart = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x;

        //Debug.Log("Fall: " + (position < screenStart || position > screenWidth) + " land: " + position + " Start: " + startPosition);
        return position < screenStart || position > screenWidth;

    }

    void OnGUI()
    {
        Event currentEvent = Event.current;

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + cam.ScreenToWorldPoint(mousePos));
        GUILayout.EndArea();
    }
}

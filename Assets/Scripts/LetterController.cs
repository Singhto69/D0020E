using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterController : MonoBehaviour
{
    public GameObject Letter_Prefab;//Set in editor
    public netMan networkManager;
    private Camera cam;
    private Vector2 mousePos = new Vector2();

    private enum UpDown { Down = -1, Start = 0, Up = 1 };
    public GameObject sword;
    private float textHeight = 0;
    private float rightEdge;
    private float leftEdge;
    private int NO_COLLISION_LAYER;
    private int COLLISION_LAYER;

    private List<GameObject> currentLetters = new List<GameObject>();
    public int AMOUNT_LETTERS = 2;
    public int AMOUNT_STARTING_POS = 2;

    void Start()
    {   

        cam = Camera.main;
        rightEdge = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        leftEdge = cam.ScreenToWorldPoint(new Vector2(0, 0)).x;

        if (sword == null)
        {
            // Create Sword
            sword = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            sword.transform.SetParent(this.transform);
            sword.transform.localScale = new Vector3(20, 40, 20);
            sword.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1);
        }

        COLLISION_LAYER = LayerMask.NameToLayer("letter_collision");
        NO_COLLISION_LAYER = LayerMask.NameToLayer("letter_no_collision");
        Physics2D.IgnoreLayerCollision(NO_COLLISION_LAYER, NO_COLLISION_LAYER);

        var tempLetter = SpawnLetter();
        textHeight = ((RectTransform)tempLetter.transform).rect.height;
        Destroy(tempLetter);
    }

    void FixedUpdate()
    {
        networkManager.ReceiveData();
        
        Vector3 udppos = new Vector3((float)networkManager.coordList[0] ,
                        (float)networkManager.coordList[1],
                        0.0f);
        print(udppos);

        Vector3 pos = cam.ScreenToWorldPoint(udppos);
        //Vector3 pos2 = cam.ScreenToWorldPoint(Input.mousePosition);

        //Input.mousePosition
        //cam.ScreenToWorldPoint(udppos);
        //print(Input.mousePosition);
        sword.transform.position = pos;
        (networkManager.Sphere).transform.position = pos;

        if(currentLetters.Count < AMOUNT_LETTERS)
        {
            currentLetters.Add(SpawnLetter());
        }

        //Reverse iterate to allow removing elements
        for (int i = currentLetters.Count-1; i>=0; i--)
        {
            GameObject letter = currentLetters[i];

            //If letter has already been destroyed
            if(letter == null)
            {
                Debug.Log("Removing already destroyed letter");
                currentLetters.RemoveAt(i);
                continue;
            }

            if(letter.layer == NO_COLLISION_LAYER && letter.transform.position.y > cam.ScreenToWorldPoint(new Vector2(0, Screen.height / 4)).y)
            {
                letter.layer = COLLISION_LAYER;
            }

            if (letter.transform.position.y < cam.ScreenToWorldPoint(new Vector2(0, 0 - textHeight)).y)
            {
                Destroy(letter);
                currentLetters.RemoveAt(i);
            }
        }
    }

    //Spawn random letter at random location on screen
    private GameObject SpawnLetter()
    {
        //Avoid division by 0 or lower
        int startingPositions = AMOUNT_STARTING_POS <= 0 ? 1 : AMOUNT_STARTING_POS;

        // Create Gameobject Letter
        GameObject LetterObject = Instantiate(Letter_Prefab);
        LetterObject.transform.SetParent(this.transform);

        // Vars
        Text letter = LetterObject.GetComponent<Text>();
        Rigidbody2D rigidbody = LetterObject.GetComponent<Rigidbody2D>();

        int letterOffset = Random.Range(0, 10027);
        float yVelocity = Random.Range(12, 20);
        int direction = Random.Range(0,2)*2-1; //Random number 0 or 1, *2 == 0 or 2, -1 == -1 or 1 (Negative or positive)

        int startingBlock = Random.Range(0, startingPositions);
        float blockWidth = Screen.width / startingPositions;
        float startPosition = blockWidth*startingBlock + blockWidth/2;


        // Properties
        letter.transform.localScale = Vector3.one;
        letter.transform.position = cam.ScreenToWorldPoint(new Vector3(startPosition, 0, 0));

        // Behaviour
        rigidbody.velocity = new Vector2(0, yVelocity);
        float sideVelocity = MaxSideVelocity(LetterObject);
        rigidbody.velocity = new Vector2(sideVelocity*direction, rigidbody.velocity.y);

        //Change Velocity direction if letter will leave screen
        if (WillFallOffScreen(LetterObject))
        { 
            rigidbody.velocity = new Vector2(-rigidbody.velocity.x, rigidbody.velocity.y);
        }


        //https://www.sttmedia.com/characterfrequency-swedish

        //Sätt denna som resultat av switchen
        int finalOffset; 

        //Ska reworka till en iterativ lösning, men detta är en okej templösning.
        switch (letterOffset)
        {
            case int n when (n <= 1004):
                finalOffset = 0; //A 10.04%
                break;
            case int n when (n >= 1005 && n < 1136):
                finalOffset = 1; //B 01.31%
                break;
            case int n when (n >= 1137 && n < 1308):
                finalOffset = 2; //C 01.71%
                break;
            case int n when (n >= 1309 && n < 1799):
                finalOffset = 3; //D 04.9%
                break;
            case int n when (n >= 1800 && n < 2785):
                finalOffset = 4; //E 09.85%
                break;
            case int n when (n >= 2786 && n < 2967):
                finalOffset = 5; //F 01.81%
                break;
            case int n when (n >= 2968 && n < 3312):
                finalOffset = 6; //G 03.44%
                break;
            case int n when (n >= 3313 && n < 3598):
                finalOffset = 7; //H 02.85%
                break;
            case int n when (n >= 3599 && n < 4100):
                finalOffset = 8; //I 05.01%
                break;
            case int n when (n >= 4101 && n < 4191):
                finalOffset = 9; //J 00.90%
                break;
            case int n when (n >= 4192 && n < 4516):
                finalOffset = 10; //K 03.24%
                break;
            case int n when (n >= 4517 && n < 4998):
                finalOffset = 11; //L 04.81%
                break;
            case int n when (n >= 4999 && n < 5354):
                finalOffset = 12; //M 03.55%
                break;
            case int n when (n >= 5355 && n < 6200):
                finalOffset = 13; //N 08.45%
                break;
            case int n when (n >= 6201 && n < 6607):
                finalOffset = 14; //O 04.06%
                break;
            case int n when(n >= 6608 && n < 6765):
                finalOffset = 15; //P 01.57%
                break;
            case int n when (n >= 6766 && n < 6767):
                finalOffset = 16; //Q 00.01%
                break;
            case int n when (n >= 6768 && n < 7556):
                finalOffset = 17; //R 07.88%
                break;
            case int n when (n >= 7557 && n < 8089):
                finalOffset = 18; //S 05.32%
                break;
            case int n when (n >= 8090 && n < 8979):
                finalOffset = 19; //T 08.89%
                break;
            case int n when (n >= 8980 && n < 9166):
                finalOffset = 20; //U 01.86%
                break;
            case int n when (n >= 9167 && n < 9422):
                finalOffset = 21; //V 02.55% & W 0%
                break;
            case int n when (n >= 9423 && n < 9434):
                finalOffset = 23; //X 00.11%
                break;
            case int n when (n >= 9434 && n < 9483):
                finalOffset = 24; //Y 00.49%
                break;
            case int n when (n >= 9484 && n < 9488):
                finalOffset = 25; //Z 00.04%
                break;
            //A = 65
            //Å = 197 => 132
            //Ä = 196 => 131
            //Ö = 214 => 149
            case int n when (n >= 9489 && n < 9665):
                finalOffset = 132; //Å 01.66%
                break;
            case int n when (n >= 9666 && n < 9876):
                finalOffset = 131; //Ä 02.10%
                break;
            case int n when (n >= 9877 && n < 10027): //crap
                finalOffset = 149; //Ö 01.50%
                break; 
            default:
                finalOffset = -1;
                break;
        }
        Debug.Log(letterOffset + " " + finalOffset);

        letter.text = char.ToString((char)('A' + finalOffset));

        return LetterObject;
    }

    private float MaxSideVelocity(GameObject letter)
    {
        Rigidbody2D rigidbody = letter.GetComponent<Rigidbody2D>();
        float yVelocity = rigidbody.velocity.y;
        float startPosition = letter.transform.position.x;
        float distanceToSide = Mathf.Max(rightEdge-startPosition, startPosition-leftEdge);

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

        //Debug.Log("Out left: " + (position < screenStart) + " Out right: " + (position > screenWidth) + " land: " + position + " Start: " + startPosition);
        //Debug.Log("screenStart: " + screenStart + " screenWidth: "+ rightEdge);

        // -1 and +1 to avoid Round-off errors...
        return position < leftEdge-1 || position > rightEdge+1;

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

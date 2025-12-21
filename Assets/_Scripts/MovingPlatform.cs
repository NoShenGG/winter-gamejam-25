using JetBrains.Annotations;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
                     private float startX;
                     private float startY;
    [SerializeField] private float finalX;
    [SerializeField] private float finalY;

    [SerializeField] private int timeTaken;
    [SerializeField] private int timeToReset;
    [SerializeField] private bool reset;
    [SerializeField] private bool moves;

    [SerializeField] GameObject platform;

    private Vector3 startVector;
    private Vector3 finalVector;

    private Vector3 tempVector;

    private int elapsedFrames = 0;


    void Start()
    {



        startX = platform.transform.position.x;
        startY = platform.transform.position.y;

        startVector = platform.transform.position;


        finalVector = new Vector3(finalX, finalY, 0);


    }

    void FixedUpdate(){
        if(moves){
            float interpolationRatio = (float)elapsedFrames / (timeTaken * 50);

            platform.transform.position = Vector3.Lerp(startVector, finalVector, interpolationRatio);

            elapsedFrames = (elapsedFrames + 1) % (timeToReset * 50);

            if (reset && elapsedFrames == 0)
            {
                tempVector = startVector;
                startVector = finalVector;
                finalVector = tempVector;
            }

        }
    }


}

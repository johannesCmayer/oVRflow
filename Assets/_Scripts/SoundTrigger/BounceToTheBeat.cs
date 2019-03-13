using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceToTheBeat : MonoBehaviour
{
    public float timeBetweenBounces = 1f;
    public float growSpeed = 1;
    public float shrinkSpeed = 1;
    public float maxScale = 2f;

    float counter = 0f;

    Vector3 myTransform;
    Vector3 currentScale;

    SoundManagement soundMan;

    bool bounceNow = true;
    bool shrinkNow;

    float timeStamp;

    void Start()
    {
        soundMan = SoundManagement.instance;

        myTransform = transform.localScale;
        currentScale = myTransform;

        timeStamp = Time.time;
    }

    void Update()
    {
        if (counter <= Time.time - timeStamp)
        {
            bounceNow = true;
            counter += soundMan.beatIntervall * timeBetweenBounces;
        }

        if (bounceNow == true)
        {
            if (transform.localScale.x < maxScale)
            {
                float growAmount = growSpeed / soundMan.beatIntervall * Time.deltaTime;
                currentScale = new Vector3(currentScale.x + growAmount, currentScale.y + growAmount, currentScale.z + growAmount);
            }
            else
            {
                bounceNow = false;
                shrinkNow = true;
            }
            transform.localScale = currentScale;
        }
        if (shrinkNow == true)
        {
            if (transform.localScale.x > myTransform.x)
            {
                float shrinkAmount = shrinkSpeed / soundMan.beatIntervall * Time.deltaTime;
                currentScale = new Vector3(currentScale.x - shrinkAmount, currentScale.y - shrinkAmount, currentScale.z - shrinkAmount);
            }
            else
            {
                transform.localScale = myTransform;
                shrinkNow = false;
            }
			if (currentScale == new Vector3(0, 0, 0) || currentScale.x > 5000)
            {
                currentScale = new Vector3(1, 1, 1);
            }
            transform.localScale = currentScale;
        }
    }
}

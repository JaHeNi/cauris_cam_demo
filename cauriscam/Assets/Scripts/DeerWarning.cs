using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeerWarning : MonoBehaviour
{
    public Image arrowImage;  // Reference to the UI Image component
    public Sprite leftArrow;  // Reference to the left arrow image
    public Sprite rightArrow; // Reference to the right arrow image
    public Sprite emptyPlaceholder; // Reference to the empty image
    private int timeOnScreen = 2;  // How long the warning stays on screen before disappearing
    private float blinkInterval = 0.2f; // Interval for blinking in seconds
    private CarControl carScript;
    void Start()
    {
        carScript = FindObjectOfType<CarControl>();
        // Initialize the arrowImage to null so no image is displayed initially
        arrowImage.sprite = emptyPlaceholder;
    }

    // Call this method when an enemy spawns
    public void UpdateArrowImage(string leftOrRight)
    {
        StopAllCoroutines(); // Stop any existing coroutines

        if (carScript.caurisCamOn)
        {
            if (leftOrRight == "left")
            {
                // Enemy is to the left
                arrowImage.sprite = leftArrow;
            }
            else
            {
                // Enemy is to the right
                arrowImage.sprite = rightArrow;
            }
        }


        // Start the coroutine to change the image to an empty placeholder after a delay 
        StartCoroutine(ChangeToCircleAfterDelay());

        // Start the coroutine to blink the image
        //StartCoroutine(BlinkImage());
    }

    IEnumerator ChangeToCircleAfterDelay()
    {
        yield return new WaitForSeconds(timeOnScreen);
        arrowImage.sprite = emptyPlaceholder;
        StopAllCoroutines(); // Stop blinking when the image is cleared
    }

    /*
    // Coroutine to make the image blink
    IEnumerator BlinkImage()
    {
        while (true)
        {
            arrowImage.enabled = !arrowImage.enabled; // Toggle the image visibility
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextFadeController : MonoBehaviour {
    private static float MINIMUM_ALPHA = 0.0f;
    private static float MAXIMUM_ALPHA = 1.0f;

    [Tooltip("Angle in degrees within which the text will start to become visible. A value between 1 and 90.")]
    [Range(1.0f, 90.0f)] public float visibilityAngle = 5.0f;

    [Tooltip("Scale how quickly the linear fade is applied")]
    [Range(0.01f, 1.0f)] public float fadeScale = 0.5f;

    [Tooltip("Minimum alpha value to reach before automatically fading the rest of the text to full")]
    [Range(0.1f, 1.0f)] public float minimumAlphaBeforeAutoFade = 0.5f;

    private float alpha = MINIMUM_ALPHA;
    private bool transitionComplete = false;
    private TextMesh textMesh;
    private Camera theCamera;


    // Use this for initialization
    void Start () {
        textMesh = gameObject.GetComponent<TextMesh>();
        SetTextAlpha(alpha);
        theCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Check to see if the user is looking at this object.
    // If so, linearly update the alpha to 1.0f and once we reach that keep it there.
    // If the user looks away then linearly update the alpha back to 0.0f
    void Update () {
        // Transition complete?
        if(transitionComplete) {
            return;
        }

        float diff = Time.deltaTime * fadeScale;
        float signMultiplier = -1.0f;

        if (IsWithinGaze() || (alpha > minimumAlphaBeforeAutoFade)) {
            signMultiplier = 1.0f;
        }

        alpha += diff * signMultiplier;

        alpha = Mathf.Clamp(alpha, MINIMUM_ALPHA, MAXIMUM_ALPHA);
        SetTextAlpha(alpha);
        // Set the transition complete when we reach maximum alpha
        if(Mathf.Approximately(alpha, MAXIMUM_ALPHA)) {
            TransitionComplete();
        }
    }

    private void SetTextAlpha(float newAlpha) {
        Color colour = textMesh.color;
        colour.a = newAlpha;
        textMesh.color = colour;
    }

    private bool IsWithinGaze() {
        return theCamera.IsFacingObject(gameObject, visibilityAngle);
    }

    private void TransitionComplete() {
        Debug.Log("Character transition complete");
        transitionComplete = true;

        // NOTE: Do any other processing here such as message sending or playing a sound etc.
    }
}

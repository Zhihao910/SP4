using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    Canvas Canvas;
    Image Joy;
    Image Stick;
    Vector3 startPosition;
    [HideInInspector]
    public Vector3 direction;


    // Use this for initialization
    void Start()
    {
        Canvas = transform.GetComponent<Canvas>();
        Joy = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<Image>();
        Stick = Joy.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Canvas.pixelRect.max;
        pos.x *= 0.1f;
        pos.y *= 0.1f;
        Joy.transform.position = pos;
    }

    public void StartPosition()
    {
        startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
    }

    public void Dragging()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER ||UNITY_EDITOR
        //Vector3 screenPos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 tempPosition = new Vector3(Input.mousePosition.x - startPosition.x, Input.mousePosition.y - startPosition.y, 1);
        float radius = 50;
        Vector2 clampPos = Vector2.ClampMagnitude(new Vector2(tempPosition.x, tempPosition.y), radius);

        Vector3 newPosition = new Vector3(clampPos.x, clampPos.y, 1);


#elif UNITY_ANDROID
        Touch mytouch = Input.GetTouch(0);
        Vector3 tempPosition = new Vector3(mytouch.position.x - startPosition.x, mytouch.position.y - startPosition.y, 1);
        float radius = 50;
        Vector2 clampPos = Vector2.ClampMagnitude(new Vector2(tempPosition.x, tempPosition.y), radius);

        Vector3 newPosition = new Vector3(clampPos.x, clampPos.y, 1);

#endif
        Stick.rectTransform.localPosition = newPosition;
        direction = Stick.rectTransform.localPosition;
    }

    public void StopDrag()
    {
        Stick.rectTransform.localPosition = new Vector3(0, 0, 1);
        direction = Vector3.zero;
    }
}

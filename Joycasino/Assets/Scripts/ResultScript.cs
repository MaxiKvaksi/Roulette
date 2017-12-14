using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour {

    public Sprite cool;
    public Sprite awful;
    Vector3 startPosition;
    Vector3 endPosition;
    bool anim;
    bool res;
    sbyte temp = 0;
    int delay = 100;

    public bool Animation
    {
        get
        {
            return anim;
        }

        set
        {
            anim = value;
        }
    }

    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x - (float)Screen.width / 5,
            startPosition.y, startPosition.z);
        Debug.Log(startPosition + "/" + endPosition);
    }

	void Update () {
		if(anim)
        {
            ResultAnimMotion();
        }
	}
    public void Anim(bool res)
    {
        anim = true;
        temp = -1;
        if (res) gameObject.GetComponent<Image>().sprite = cool;
        else gameObject.GetComponent<Image>().sprite = awful;

    }
    public void ResultAnimMotion()
    {
        if (temp == - 1)transform.position -= new Vector3(1.5f, 0, 0);
        if (temp == 1) transform.position += new Vector3(1.5f, 0, 0);
        if (delay > 0 && temp == 0) delay--;
        if (delay == 0) temp = 1;
        if (transform.position.x < endPosition.x && delay != 0) temp = 0;
        if (transform.position.x > startPosition.x)
        {
            anim = false;
            delay = 100;
        }
    }
}

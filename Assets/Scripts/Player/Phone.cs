using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public bool isActive;
    public Vector3 inHandCords;
    public Vector3 activeCords;
    public Vector3 inHandRotation;
    public Vector3 activeRotation;
    public GameObject root;
    public GameObject clone;
    public float offsetY = -50;

    public GameObject pixelsRoot;
    public List<GameObject> pixels = new List<GameObject>();
    public float currentTimer = 0;
    public float pixelBrokeTimer = 20;

    void Start()
    {
        isActive = false;
        for(int i = 0; i < pixelsRoot.transform.childCount; i++)
        {
            Color color = pixelsRoot.transform.GetChild(i).GetComponent<Image>().color;
            color.a = 0;
            pixelsRoot.transform.GetChild(i).GetComponent<Image>().color = color;
            pixels.Add(pixelsRoot.transform.GetChild(i).gameObject);
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("Activate"))
        {
            if (isActive)
            {
                gameObject.transform.position = root.transform.position + root.transform.rotation * inHandCords;
                clone.transform.position = root.transform.position + root.transform.rotation * inHandCords + new Vector3(0, offsetY, 0);
                gameObject.transform.rotation = root.transform.rotation * Quaternion.Euler(inHandRotation);
                clone.transform.rotation = root.transform.rotation * Quaternion.Euler(inHandRotation);
                isActive = false;
            }
            else
            {
                gameObject.transform.position = root.transform.position + root.transform.rotation * activeCords;
                clone.transform.position = root.transform.position + root.transform.rotation * activeCords + new Vector3(0, offsetY, 0);
                gameObject.transform.rotation = root.transform.rotation * Quaternion.Euler(activeRotation);
                clone.transform.rotation = root.transform.rotation * Quaternion.Euler(activeRotation);
                isActive = true;
            }
        }

        currentTimer -= Time.deltaTime;
        if(currentTimer < 0)
        {
            BrokePixel();
            currentTimer = pixelBrokeTimer;
        }
    }

    private void BrokePixel()
    {
        if(pixels.Count > 0)
        {
            int index = Random.Range(0, pixels.Count);
            GameObject brokenPixel = pixels[index];
            Color color = brokenPixel.GetComponent<Image>().color;
            color.a = 1;
            brokenPixel.GetComponent<Image>().color = color;
            pixels.RemoveAt(index);
        }
    }
}

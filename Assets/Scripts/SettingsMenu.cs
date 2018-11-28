using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject Panel;
    public GameObject BlendShapesOutput;
    public GameObject HeadTracker;
    public GameObject FaceEventsTracker;
    public GameObject EyesTracker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel() {
        if(Panel != null) {
            Animator animator = Panel.GetComponent<Animator>();
            if(animator != null) {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }
    }

    public void ToggleHeadTracker(bool isSelected) {
        if(isSelected) {
            HeadTracker.SetActive(true);
        }
        else {
            HeadTracker.SetActive(false);
        }
    }

    public void UpdateHeadInterval(string interval)
    {
        HeadTracker.GetComponent<HeadTracker>().updateInterval = Convert.ToDouble(interval);
    }

    public void ToggleFaceTracker(bool isSelected)
    {
        if (isSelected)
        {
            FaceEventsTracker.SetActive(true);
        }
        else
        {
            FaceEventsTracker.SetActive(false);
        }
    }

    public void UpdateFaceInterval(string interval)
    {
            FaceEventsTracker.GetComponent<FaceEventsTracker>().updateInterval = Convert.ToDouble(interval);
    }

    public void ToggleEyesTracker(bool isSelected)
    {
        if (isSelected)
        {
            EyesTracker.SetActive(true);
        }
        else
        {
            EyesTracker.SetActive(false);
        }
    }

    public void UpdateEyesInterval(string interval)
    {
            EyesTracker.GetComponent<EyesTracker>().updateInterval = Convert.ToDouble(interval);
    }

    public void ToggleBlendShapes(bool isSelected)
    {
        Debug.Log("Is selected" + isSelected.ToString());
        if (isSelected)
        {
            BlendShapesOutput.SetActive(true);
        }
        else
        {
            BlendShapesOutput.SetActive(false);
        }
    }

    public void UpdateBlendInterval(string interval)
    {
            BlendShapesOutput.GetComponent<ShapePrinter>().updateInterval = Convert.ToDouble(interval);
    }
}

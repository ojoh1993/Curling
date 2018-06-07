using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour {

    public Camera mainCam;
    bool attached = false;
    Vector3 mainPos = new Vector3(0, 61f, -71f);
    Vector3 attachPos = new Vector3(0, 30f, -2f);
    Vector3 mainAngle = new Vector3(45f, 0, 0);
    Vector3 attachAngle = new Vector3(60f, 0, 0);

    private static CamManager camManager;

    public static CamManager instance
    {
        get
        {
            if (!camManager)
            {
                camManager = FindObjectOfType(typeof(CamManager)) as CamManager;

                if (!camManager)
                {
                    Debug.LogError("There needs to be one active CamManager script on a GameObject in your scene.");
                }
                else
                {
                    camManager.Init();
                }
            }

            return camManager;
        }
    }

    private void Init()
    {
    }

    public void ResetCamPosition()
    {
        mainCam.transform.parent = null;
        mainCam.transform.position = mainPos;
        mainCam.transform.rotation = Quaternion.Euler(mainAngle);
        attached = false;
    }

    public void AttachCamToStone(GameObject stone)
    {
        if (!attached)
        {
            mainCam.transform.parent = stone.transform;
            mainCam.transform.localPosition = attachPos;
            mainCam.transform.localRotation = Quaternion.Euler(attachAngle);
            attached = true;
        }

    }
}

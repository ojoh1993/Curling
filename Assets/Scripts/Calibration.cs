using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour {

    public RectTransform bar;

    public const float CALIBRATION_ERROR_MAX = 90 * Mathf.Deg2Rad;
    const float BORDER = 250f;
    const float BAR_SPEED = 1f;

    private bool dir_left = false;
    public float lerp_value = 0.5f;


    private static Calibration calibration;

    public static Calibration instance
    {
        get
        {
            if (!calibration)
            {
                calibration = FindObjectOfType(typeof(Calibration)) as Calibration;

                if (!calibration)
                {
                    Debug.LogError("There needs to be one active script on a GameObject in your scene.");
                }
            }

            return calibration;
        }
    }


    public float GetCalibrationRatio()
    {
        return bar.localPosition.x / BORDER;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        calibration = this;
    }

    // Update is called once per frame
    void Update () {

        if (!dir_left)
        {
            lerp_value += Time.deltaTime * BAR_SPEED;
            if (lerp_value >= 1f)
                dir_left = true;
        }
        else
        {
            lerp_value -= Time.deltaTime * BAR_SPEED;
            if (lerp_value <= 0f)
                dir_left = false;

        }
        bar.localPosition = new Vector3(Mathf.Lerp(-BORDER, BORDER, lerp_value),0);
	}

    public void SetCalibration()
    {
        gameObject.SetActive(true);
    }
    public void UnsetCalibration()
    {
        gameObject.SetActive(false);
        lerp_value = 0.5f;
    }
}

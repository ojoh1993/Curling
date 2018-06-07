using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 표적을 관리하는 클래스.
/// 공을 하나 던질 때 마다 표적의 위치를 변경하는 기능을 담당한다.
/// </summary>
public class MarkerManager : RandomPositionManager {

    public float min_marker_pos_z,
                 max_marker_pos_z,
                 min_marker_pos_x,
                 max_marker_pos_x;

    public GameObject area_0,area_1000,area_2000;

    private static MarkerManager markerManager;

    public static MarkerManager instance
    {
        get
        {
            if (!markerManager)
            {
                markerManager = FindObjectOfType(typeof(MarkerManager)) as MarkerManager;

                if (!markerManager)
                {
                    Debug.LogError("There needs to be one active MarkerManager script on a GameObject in your scene.");
                }
                else
                {
                    markerManager.Init();
                }
            }

            return markerManager;
        }
    }

    private void Init()
    {
        SetBoundary(min_marker_pos_x,
                    max_marker_pos_x,
                    min_marker_pos_z,
                    max_marker_pos_z);
        ResetPosition();
    }

    public Vector3 GetPosition() {
         return instance.transform.position;
    }

    public float GetScale()
    {
        return instance.transform.localScale.z;
    }

    public void MoveToRandomSpace()
    {
        MoveToRandomSpace(0f);
    }

}

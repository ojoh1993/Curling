#define VER3

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : RandomPositionManager
{
    public float min_obstacle_pos_z;
    private void Start()
    {
#if VER3
        GetComponent<MeshCollider>().material.dynamicFriction = GameManager.instance.Friction;
#endif        
        Vector3 MarkerPos = MarkerManager.instance.GetPosition();
        SetBoundary(MarkerManager.instance.min_marker_pos_x,
                    MarkerManager.instance.max_marker_pos_x,
                    min_obstacle_pos_z,
                    MarkerPos.z - MarkerManager.instance.GetScale() * 2f);

        MoveToRandomSpace();
    }
    public void MoveToRandomSpace()
    {
        MoveToRandomSpace(5f);
    }

}

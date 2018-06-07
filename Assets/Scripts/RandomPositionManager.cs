using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 표적을 관리하는 클래스.
/// 공을 하나 던질 때 마다 표적의 위치를 변경하는 기능을 담당한다.
/// </summary>
public class RandomPositionManager : MonoBehaviour
{
    private float min_pos_z, max_pos_z,
                min_pos_x, max_pos_x;

    public Vector3 origin_pos;

    private void Start()
    {
        ResetPosition();
    }

    /// <summary>
    /// 대상의 위치를 원점으로 돌리는 함수
    /// </summary>
    public void ResetPosition()
    {
        transform.position = origin_pos;
    }

    /// <summary>
    /// 대상의 위치를 랜덤하게 옮겨주는 함수
    /// </summary>
    protected void MoveToRandomSpace(float y)
    {
        transform.position = new Vector3(Random.value * (max_pos_x - min_pos_x) + min_pos_x,
                                        y,
                                        Random.value * (max_pos_z - min_pos_z) + min_pos_z);
    }

    public void SetBoundary(float min_x, float max_x, float min_z, float max_z)
    {
        min_pos_x = min_x;
        min_pos_z = min_z;

        max_pos_x = max_x;
        max_pos_z = max_z;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 점수 판정을 담당하는 클래스. 컬링공 중앙에 붙은 회색 막대가 점수를 판정한다.
/// 그 회색막대를 판정 막대라 칭한다. 그리고 이 클래스는 그 판정막대에 붙어서 동작한다.
/// </summary>
public class Judge : MonoBehaviour {

    public int score;
    public bool ready;
    
    private void Init()
    {
        score = -1000;
        ready = false;
    }

    /// <summary>
    /// 판정 막대가 득점 구역에 들어 갈 경우 몇점인지 판정하는 함수.
    /// -1000 / 0 / 1000 / 2000 점으로 나뉜다.
    /// </summary>
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == GameManager.instance.Plane)
            ready = true;

        if (col.gameObject == MarkerManager.instance.area_0)
        {
            score = 0;
        }
        else if (col.gameObject == MarkerManager.instance.area_1000)
        {
            score = 1000;
        }
        else if (col.gameObject == MarkerManager.instance.area_2000)
        {
            score = 2000;
        }

    }

    /// <summary>
    /// 판정 막대가 득점 구역에서 빠져 나올 경우 몇점인지 판정하는 함수.
    /// </summary>
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Plane")
        {
            transform.parent.GetComponent<StoneManager>().whenStop();
        }

        if (col.gameObject == MarkerManager.instance.area_0)
        {
            score = -1000;
        }
        else if (col.gameObject == MarkerManager.instance.area_1000)
        {
            score = 0;
        }
        else if (col.gameObject == MarkerManager.instance.area_2000)
        {
            score = 1000;
        }
    }
}

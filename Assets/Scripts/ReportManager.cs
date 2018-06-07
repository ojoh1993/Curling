using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 게임 기록을 담당하는 클래스.
/// 공을 클릭하고 나서 발사하기 까지의 시간을 기록한다.
/// </summary>
public class ReportManager : MonoBehaviour {

    static List<float> trialTime = new List<float>();
    
    /// <summary>
    /// 기록된 발사 시간을 txt파일로 저장하는 함수.
    /// </summary>
    public static void Report()
    {
        var f = new FileStream("Report.txt", FileMode.Append, FileAccess.Write);
        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
        writer.WriteLine("///////////////");
        foreach (var item in trialTime)
        {
            writer.WriteLine(item);
        }
        writer.Close();
        trialTime.Clear();
    }

    /// <summary>
    /// 돌을 발사하기 까지 걸린 시간을 기록하는 함수.
    /// </summary>
    /// <param name="interval">돌 발사에 걸린 시간</param>
    public static void ReportTrialTime(float interval)
    {
        trialTime.Add(interval);
    }
}

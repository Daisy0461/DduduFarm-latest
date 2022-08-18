using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeStringFormatter {

    public static string Sec2Time(this int secValue) {
        string time = string.Empty;
        if (secValue / 3600 > 0)
        {
            time = string.Concat(time, (secValue/3600).ToString(), " 시간 ");
            secValue = secValue % 3600;
        }
        if (secValue / 60 > 0)
        {
            time = string.Concat(time, (secValue/60).ToString(), " 분 ");
            secValue = secValue % 60;
        }
        if (secValue % 60 > 0)
        {
            time = string.Concat(time, (secValue%60).ToString(), " 초");
        }
        if (time == string.Empty)
        {
            time = string.Concat(time, "0 초");
        }
        return time;
    }
}

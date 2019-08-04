using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TimeChannel
{
    Absolute,
    Player,
    World,
}



public class TimeManager : MonoBehaviour
{
    class timeInfo
    {
        public float relativeTimeScale = 1f;
        public float cumulativeTimeScale = 1f;
        public List<TimeChannel> children = new List<TimeChannel>();
    }

    [System.Serializable]
    public struct TimeChannelSettings
    {
        public TimeChannel channel;
        public TimeChannel[] childrenChannels;
    }

    public TimeChannelSettings[] timeChannels;

    private void Awake()
    {
        foreach(TimeChannelSettings channel in timeChannels)
        {
            timeInfo info = new timeInfo();
            foreach(TimeChannel child in channel.childrenChannels)
            {
                info.children.Add(child);
            }
            timeInfos.Add(channel.channel, info);
        }
    }

    static float dt = 0f;

    private void Update()
    {
        dt = Time.deltaTime;
    }

    static Dictionary<TimeChannel, timeInfo> timeInfos = new Dictionary<TimeChannel, timeInfo>();

    public static void SetTimeScale(TimeChannel channel, float timeScale)
    {
        float modifyValue = timeScale / timeInfos[channel].relativeTimeScale;
        timeInfos[channel].cumulativeTimeScale *= modifyValue;
        timeInfos[channel].relativeTimeScale = timeScale;
        foreach(TimeChannel child in timeInfos[channel].children)
        {
            ModifyCumulativeTimeScale(child, timeInfos[channel].cumulativeTimeScale *= modifyValue);
        }
    }

    static void ModifyCumulativeTimeScale(TimeChannel channel, float parentValue)
    {
        timeInfos[channel].cumulativeTimeScale = timeInfos[channel].relativeTimeScale * parentValue;
        foreach (TimeChannel child in timeInfos[channel].children)
        {
            ModifyCumulativeTimeScale(child, parentValue);
        }
    }

    public static float GetTimeDelta(TimeChannel channel)
    {
        return dt*timeInfos[channel].cumulativeTimeScale;
    }

    public static float GetTimeScale(TimeChannel channel)
    {
        return timeInfos[channel].cumulativeTimeScale;
    }

}

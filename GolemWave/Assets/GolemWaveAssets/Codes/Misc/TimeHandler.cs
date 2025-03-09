using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public void TimePaused()
    {
        Time.timeScale = 0.0f;
    }

    public void TimeUnpaused()
    {
        Time.timeScale = 1.0f;
    }
}

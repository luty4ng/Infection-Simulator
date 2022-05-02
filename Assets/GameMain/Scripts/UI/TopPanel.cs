using UnityEngine;
using UnityEngine.UI;
using GameKit;

public class TopPanel : MonoBehaviour
{
    public Text day;
    public Text time;
    public Text death;
    private int deathCount = 0;

    private void Start()
    {
        EventManager.instance.AddEventListener("Agent Dead", () =>
        {
            deathCount += 1;
        });
    }

    private void Update()
    {
        day.text = "Day " + GameCenter.current.currentDay;
        time.text = "Time " + (int)GameCenter.current.currentTime;
        death.text = "Death " + deathCount;
    }
}
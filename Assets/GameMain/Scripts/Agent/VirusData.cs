using UnityEngine;
public enum Symptom
{
    None,
    Mild,
    Moderate,
    Severe
}
[System.Serializable]
public class VirusData
{
    public static float SpreadRate = 0.8f;
    public static float SpreadRangeMultipier = 1f;
    public static float SpreadMinIncremental = 5f;
    public static float SpreadMaxIncremental = 10f;
    public static float SpreadCycle = 1.5f;
    public float InfectedValue;
    private float mildCountTime = 0f;
    private float severeCountTime = 0f;
    public Symptom symptom = Symptom.None;
    public bool IsInfected
    {
        get
        {
            return InfectedValue >= Random.Range(60, 70);
        }
    }

    public VirusData(float infectedValue = 0)
    {
        this.InfectedValue = infectedValue;
    }

    public void OnVirusUpdateSympton()
    {
        if (IsInfected)
        {
            severeCountTime += Time.deltaTime;
            if (severeCountTime >= 30f)
            {
                severeCountTime = 0f;
                symptom = Random.Range(0f, 1f) > 0.5f ? Symptom.Moderate : Symptom.Severe;
            }
        }

        if (InfectedValue >= 30 && InfectedValue < 60)
        {
            mildCountTime += Time.deltaTime;
            if (mildCountTime >= 20f)
            {
                mildCountTime = 0f;
                symptom = Random.Range(0f, 1f) > 0.5f ? Symptom.Moderate : Symptom.Severe;
            }
        }
        else if (InfectedValue < 30)
        {
            if (InfectedValue < 0f)
            {
                InfectedValue = 0f;
            }
            symptom = Symptom.None;
        }
    }
}
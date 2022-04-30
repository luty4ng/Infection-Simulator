using UnityEngine;
[System.Serializable]
public class VirusData
{

    [Range(0f, 1f)] public float spreadRate;
    [Range(0f, 1f)] public float deadRate;
    [Range(0.5f, 1.5f)] public float spreadRangeMultipier;
    [Range(0.5f, 5f)] public float spreadCycle;
    public bool isInfected = false;

    public VirusData()
    {
        this.spreadRate = 0.2f;
        this.deadRate = 0f;
        this.spreadRangeMultipier = 1f;
        this.spreadCycle = 3f;
    }
}
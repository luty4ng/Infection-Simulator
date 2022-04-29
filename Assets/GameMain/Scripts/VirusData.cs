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
    public Symptom symptom;
    [Range(0f, 1f)] public float spreadRate;
    [Range(0f, 1f)] public float deadRate;
    [Range(0.5f, 1.5f)] public float spreadRangeMultipier;
    [Range(0.5f, 5f)] public float spreadCycleMultipier;
    [Range(0.5f, 5f)] public float VariationCycleMultipier;
    [Range(0.5f, 5f)] public float symptomCycleMultipier;
}
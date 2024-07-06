using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class TimeStopEffect : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public GameObject maskObject;
    private bool isTimeStopped = false;
    private UnityEngine.Rendering.PostProcessing.ColorGrading colorGrading;
    private UnityEngine.Rendering.PostProcessing.Vignette vignette;

    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        postProcessVolume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTimeStop();
        }
    }

    void ToggleTimeStop()
    {
        isTimeStopped = !isTimeStopped;
        Time.timeScale = isTimeStopped ? 0 : 1;
        maskObject.SetActive(isTimeStopped);

        if (isTimeStopped)
        {
            colorGrading.enabled.value = true;
            vignette.enabled.value = true;
        }
        else
        {
            colorGrading.enabled.value = false;
            vignette.enabled.value = false;
        }
    }
}

using System.Collections;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    public GameObject mask1Object; // Reference to Mask1 object
    public GameObject mask2Object; // Reference to Mask2 object
    public GameObject stillMaskObject; // Reference to Still_mask object
    public GameObject bgmObject; // Reference to BGM object

    private bool isQTECompleted = false;
    private bool isLocked = false; // Lock flag to prevent function re-entry
    private AudioSource bgmAudioSource; // Audio source for BGM

    void Start()
    {
        if (bgmObject != null)
        {
            bgmAudioSource = bgmObject.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isLocked)
        {
            StartCoroutine(HandleMasks());
        }
    }

    private IEnumerator HandleMasks()
    {
        isLocked = true; // Lock the function

        // Normalize and enable the first two mask objects from 0.25 sine position
        SteadyLoad(mask1Object, 0.25f);
        SteadyLoad(mask2Object, 0.25f);

        // Lerp audio pitch down
        if (bgmAudioSource != null)
        {
            yield return StartCoroutine(LerpAudioPitch(bgmAudioSource, 1f, 0.1f, 0.5f));
        }

        // Play the first half of the animation (0.5 sine cycle)
        yield return StartCoroutine(PlayHalfAnimation());

        // Disable the first two mask objects
        mask1Object.SetActive(false);
        mask2Object.SetActive(false);

        // Enable the Still_mask object
        stillMaskObject.SetActive(true);

        // Wait for QTE input
        yield return StartCoroutine(WaitForQTE());

        // Lerp audio pitch up
        if (bgmAudioSource != null)
        {
            yield return StartCoroutine(LerpAudioPitch(bgmAudioSource, 0.1f, 1f, 0.5f));
        }

        // Disable the Still_mask object
        stillMaskObject.SetActive(false);

        // Normalize and enable the first two mask objects again from 0.75 sine position
        SteadyLoad(mask1Object, 0.75f);
        SteadyLoad(mask2Object, 0.75f);

        // Continue the second half of the animation (0.5 sine cycle)
        yield return StartCoroutine(PlayHalfAnimation());

        // Disable the first two mask objects
        mask1Object.SetActive(false);
        mask2Object.SetActive(false);

        // Unlock the function after a safe duration (half sine time)
        yield return new WaitForSeconds(2.0f);
        isLocked = false;
    }

    private void SteadyLoad(GameObject obj, float sineAdvance)
    {
        // Normalize object
        NormalizeObject(obj, sineAdvance);

        // Ensure the object is not active before activating
        if (!obj.activeSelf)
        {
            obj.SetActive(true);
        }
    }

    private void NormalizeObject(GameObject obj, float sineAdvance)
    {
        // Reset position, rotation, and scale to initial values
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        // Set the sine animation to the specified advance point (0.25, 0.75 cycle start)
        Material mat = obj.GetComponent<Renderer>().material;
        mat.SetFloat("_SineTime", sineAdvance);
    }

    private IEnumerator PlayHalfAnimation()
    {
        float duration = 2.0f; // Duration for half of the sine animation
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WaitForQTE()
    {
        Debug.Log("Waiting for QTE...");
        isQTECompleted = false;
        while (!isQTECompleted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isQTECompleted = true;
            }
            yield return null;
        }
    }

    private IEnumerator LerpAudioPitch(AudioSource audioSource, float startPitch, float endPitch, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            audioSource.pitch = Mathf.Lerp(startPitch, endPitch, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.pitch = endPitch;
    }
}

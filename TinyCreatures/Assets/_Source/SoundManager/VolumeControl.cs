using UnityEngine;
using UnityEngine.UI;

public class VolumeControler : MonoBehaviour
{
    [SerializeField] private Sprite audioOn;
    [SerializeField] private Sprite audioOff;
    [SerializeField] private GameObject buttonAudio;

    [SerializeField] private Slider volumeSlider;

    public AudioClip clipMenu;
    public AudioSource audio;

    private void Update()
    {
        audio.volume = volumeSlider.value;
    }

    public void OnOffAudio()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            buttonAudio.GetComponent<Image>().sprite = audioOff;
        }
        else
        {
            AudioListener.volume = 1;
            buttonAudio.GetComponent<Image>().sprite = audioOn;
        }
    }

    public void PlaySound()
    {
        audio.PlayOneShot(clipMenu);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioState {
    OpenWindow,
    PressTask,
    CloseWindow,
    MoveNpc,
    TaskComplete
}

public class AudioManager : MonoBehaviour {
    public static AudioManager _instance;

    [SerializeField] private AudioClip openWindowClip;
    [SerializeField] private AudioClip pressTaskClip;
    [SerializeField] private AudioClip closeWindowClip;
    [SerializeField] private AudioClip moveNpcClip;
    [SerializeField] private AudioClip taskCompleteClip;

    private AudioSource audioSource;

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioState state) {
        switch (state) {
            case AudioState.OpenWindow:
                audioSource.PlayOneShot(openWindowClip);
                break;
            case AudioState.PressTask:
                audioSource.PlayOneShot(pressTaskClip);
                break;
            case AudioState.CloseWindow:
                audioSource.PlayOneShot(closeWindowClip);
                break;
            case AudioState.MoveNpc:
                audioSource.PlayOneShot(moveNpcClip);
                break;
            case AudioState.TaskComplete:
                audioSource.PlayOneShot(taskCompleteClip);
                break;
        }
    }
    public void PlayAudioButton(int state) {
        PlayAudio((AudioState)state);
    }

}

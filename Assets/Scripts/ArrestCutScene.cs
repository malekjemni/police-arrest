using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ArrestCutScene : MonoBehaviour
{
    public PlayableDirector director;
    public TimelineAsset timelineAsset;
    public Animator playerAnimator;
    public Transform playerPlacement;
    public PlayerInput playerInput;

    private GameObject target;
    private Vector3 initialPos;

    public void BindTimelineTracks(GameObject _outlawTrack)
    {
        timelineAsset = (TimelineAsset)director.playableAsset;
        AnimationTrack track = null;
        foreach (var output in timelineAsset.outputs)
        {
            if (output.streamName == "outlawAnimation")
            {
                track = (AnimationTrack)output.sourceObject;
                director.SetGenericBinding(track, _outlawTrack);
                break;
            }

            
        }
    }
    public void PlayCutScene(GameObject _target,Transform _player)
    {
        playerAnimator.SetTrigger("Arrest");
        initialPos = _player.localPosition;
        playerInput.enabled = false;   
        BindTimelineTracks(_target);
        _target.GetComponent<Animator>().SetTrigger("hold");
        target = _target;
        director.Play();
        director.stopped += OnCutsceneStopped;
    }

    private void OnCutsceneStopped(PlayableDirector playableDirector)
    {
        playerPlacement.localPosition = initialPos;
        playerInput.enabled = true;
        Destroy(target);
        director.stopped -= OnCutsceneStopped;
    }


}

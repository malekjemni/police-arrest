using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ArrestCutScene : MonoBehaviour
{
    public PlayableDirector director;
    public TimelineAsset timelineAsset;
    public GameObject outlawTrack;
    public Animator playerAnimator;
    public Animator outlawAnimator;
    public bool autoBindTracks = true;

    private GameObject target;


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
    public void PlayCutScene(GameObject _target)
    {
        
        BindTimelineTracks(_target);
        playerAnimator.enabled = true;
        _target.GetComponent<Animator>().SetTrigger("hold");
        target = _target;
        director.Play();
        director.stopped += OnCutsceneStopped;
    }

    private void OnCutsceneStopped(PlayableDirector playableDirector)
    {
        playerAnimator.enabled = false;
        Destroy(target);
        director.stopped -= OnCutsceneStopped;
    }


}

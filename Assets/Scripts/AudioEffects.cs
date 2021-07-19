using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioEffects : MonoBehaviour
{
    public GameController gameController;
    public Spawner spawner;

    [SerializeField] private AudioMixer audioMixer;

    public List<AudioMixerSnapshot> level;
    public List<float> transitionTime;

    void Start()
    {
        level[0].TransitionTo(transitionTime[0]);
    }

    void Update()
    {
        if (gameController.score >= spawner.levelControllers[0])
            level[1].TransitionTo(transitionTime[1]);

        if (gameController.score >= spawner.levelControllers[1])
            level[2].TransitionTo(transitionTime[2]);

        if (gameController.score >= spawner.levelControllers[2])
            level[3].TransitionTo(transitionTime[3]);
    }
}

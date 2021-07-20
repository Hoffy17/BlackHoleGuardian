using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioEffects : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Reference-Types)
    //Calls the following scripts
    public GameController gameController;
    public Spawner spawner;

    //Call the audio mixer
    public AudioMixer audioMixer;

    //The default audio mixer snapshot
    public AudioMixerSnapshot firstLevel;
    //Audio mixer snapshots that play as the player's score increases
    public List<AudioMixerSnapshot> levelSnapshot;
    //The transition time between snapshots
    public List<float> transitionTime;

    void Start()
    {
        //Play the default audio snapshot
        firstLevel.TransitionTo(0f);
    }

    void Update()
    {
        //Transition to the next audio snapshot when the player's level increases
        for (int i = 0; i < levelSnapshot.Count; i++)
        {
            if (gameController.score >= spawner.levelControllers[i])
                levelSnapshot[i].TransitionTo(transitionTime[i]);
        }
    }
}

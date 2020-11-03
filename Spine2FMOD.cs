using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class Spine2FMOD : MonoBehaviour
{
  
    // List Start
    [Tooltip("Use 'Size' to choose animations number")]
    public List<AnimationSound> Animations = new List<AnimationSound>();

    [System.Serializable]
    public class AnimationSound
    {
        // Spine Event
        [SerializeField]
        [Tooltip("Insert Spine Audio Event Name here")]
        [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string SpineEvent;

        // FMOD Event
        [SerializeField]
        [Tooltip("Insert Fmod Event here")]
        [FMODUnity.EventRef]
        public string FMODEvent;

        // DebugLog Event Button
        public bool DebugLog = false;
    }
    // List End

    //********************************************
    void Start()
    {
        // Get a reference for "Skeleton Animation"
        SkeletonAnimation anim = GetComponent<SkeletonAnimation>();

        //  Get the animation state of the "Skeleton Animations" 
        //  And then subscribe to the Event
        anim.AnimationState.Event += OnEvent;
    }

    // Takes a Spine Animation state, track index and then the actual event reference parameters
    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        // For each animation in the list,  0 to Count - 1
        for (int i = 0; i < Animations.Count; i++)
        {
            // Declaring and value a temporary variable with the element I'm checking
            AnimationSound tempAnim = Animations[i];

            // If DebugLog is true Debug.Log script name + event name
            if (tempAnim.DebugLog) Debug.Log("Spine2FMOD Event: " + e.Data.Name);

            // If the Date name of the object that triggered the event is the same
            // to the name of the temporary object event I'm evaluating
            if (e.Data.Name == tempAnim.SpineEvent)
            {

                // Debug.Log("Spine2FMOD WORKING");
                //FMOD One Shot Sound with the variable of our list in the ispector
                FMODUnity.RuntimeManager.PlayOneShot(tempAnim.FMODEvent, GetComponent<Transform>().position);

                // Warning: if it is sufficient for this condition to occur only once, and therefore
                // it is not necessary to continue checking the other elements if one has already been found
                // corresponding to the search, then you can end the for loop
                // and avoid subsequent unnecessary checks.
                break;
            }
        }
    }
   
}
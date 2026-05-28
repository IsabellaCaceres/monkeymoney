using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class TimelineScript : MonoBehaviour
{
    public PlayableDirector director;
    InputAction startAction;

    void Awake()
    {
        startAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        if (startAction.WasPressedThisFrame())
        {
            director.Play();
            enabled = false; // prevent retriggering
        }
    }
}

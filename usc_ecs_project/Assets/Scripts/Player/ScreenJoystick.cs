using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public enum State { Up, Down, Hold }

    private static ScreenJoystick instance;
    public static ScreenJoystick Instance => instance;

    // ------- Inspector visible variables ---------------------------------------

    [Tooltip("The range in non-scaled pixels for which we can drag the joystick around.")] [SerializeField]
    protected float movementRange = 50f;

    [Tooltip("Should the Base image move along with the finger without any constraints?")] [SerializeField]
    protected bool moveBase = true;

    [Tooltip("Should the joystick snap to finger? If it's FALSE, the MoveBase checkbox logic will be ommited")]
    [SerializeField]
    protected bool snapsToFinger = true;

    [Tooltip("Rect Transform of the joystick base")] [SerializeField]
    protected RectTransform baseTransform;

    [Tooltip("Rect Transform of the stick itself")] [SerializeField]
    protected RectTransform stickTransform;

    protected State state = State.Up;
    protected Vector2 direction = Vector2.zero;
    // ---------------------------------------------------------------------------

    private Vector2 initialStickPosition;
    private Vector2 intermediateStickPosition;
    private Vector2 initialBasePosition;

    public Action<State> onStateChanged;

    public Vector2 Direction => direction / movementRange;
    
    private Camera CurrentEventCamera { get; set; }

    private void Awake()
    {
        instance = this;
        initialStickPosition = stickTransform.anchoredPosition;
        intermediateStickPosition = initialStickPosition;
        initialBasePosition = baseTransform.anchoredPosition;
        Hide(true);
    }

    private void FixedUpdate()
    {
        if (!Input.GetMouseButton(0))
            SetInactive();
    }

    public void SetInactive()
    {
        // reset everything to the initial state
        direction = Vector2.zero;
        baseTransform.anchoredPosition = initialBasePosition;
        stickTransform.anchoredPosition = initialStickPosition;
        intermediateStickPosition = initialStickPosition;
        Hide(true);
        state = State.Up;
        onStateChanged?.Invoke(state);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(stickTransform, eventData.position, CurrentEventCamera, out var worldJoystickPosition);
        stickTransform.position = worldJoystickPosition;

        // We then query its anchored position. It's calculated internally and quite tricky to do from scratch here in C#
        var stickAnchoredPosition = stickTransform.anchoredPosition;

        // Find current difference between the previous central point of the joystick and it's current position
        Vector2 difference = stickAnchoredPosition - intermediateStickPosition;

        // Normalization stuff
        var diffMagnitude = difference.magnitude;
        var normalizedDifference = difference / diffMagnitude;

        stickTransform.anchoredPosition = stickAnchoredPosition;

        direction = difference;

        if (state != State.Hold)
        {
            state = State.Hold;
            onStateChanged?.Invoke(state);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        direction = Vector2.zero;
        // When we lift our finger, we reset everything to the initial state
        baseTransform.anchoredPosition = initialBasePosition;
        stickTransform.anchoredPosition = initialStickPosition;
        intermediateStickPosition = initialStickPosition;

        Hide(true);
        state = State.Up;
        onStateChanged?.Invoke(state);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        direction = Vector2.zero;
        if (snapsToFinger)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(stickTransform, eventData.position, CurrentEventCamera, out var localStickPosition);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(baseTransform, eventData.position, CurrentEventCamera, out var localBasePosition);

            baseTransform.position = localBasePosition;
            stickTransform.position = localStickPosition;
            intermediateStickPosition = stickTransform.anchoredPosition;
        }
        else 
            OnDrag(eventData);

        Hide(false);
        state = State.Down;
        onStateChanged?.Invoke(state);
    }

    private void Hide(bool isHidden)
    {
        baseTransform.gameObject.SetActive(!isHidden);
        stickTransform.gameObject.SetActive(!isHidden);
    }
}
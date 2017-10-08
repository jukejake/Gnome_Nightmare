using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap_Zoom : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private Vector2 MouseStart;
    private Vector2 MouseMove;
    private Vector2 MouseDifference;
    public Camera MiniMap_Camera;
    public Vector2 ClampDistance;

    private void Start() {
        Camera MiniMap = (Camera)Instantiate(MiniMap_Camera);
        MiniMap.name = "MiniMap_Camera";
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        MouseStart = eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        MouseMove = eventData.position;

        MouseDifference = MouseMove - MouseStart;
        float Temp = MouseDifference.magnitude * 0.1f;
        Temp = Mathf.Clamp(Temp, ClampDistance.x, ClampDistance.y);
        Debug.Log(Temp);
        GameObject.Find("MiniMap_Camera").GetComponent<Camera>().orthographicSize = Temp;
        //MiniMap_Camera.orthographicSize = Temp;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        MouseDifference = MouseMove - MouseStart;

        float Temp = MouseDifference.magnitude * 0.1f;
        Temp = Mathf.Clamp(Temp, ClampDistance.x, ClampDistance.y);
        //MiniMap_Camera.orthographicSize = Temp;
    }
}

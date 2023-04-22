using UnityEngine;
using UnityEngine.EventSystems;

public class MakeEventSystem : MonoBehaviour
{
    private GameObject eventSystem;

    private void Awake()
    {
        eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule), typeof(BaseInput));
    }
}

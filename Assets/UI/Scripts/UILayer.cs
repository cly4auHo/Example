using UnityEngine;

public class UILayer : MonoBehaviour
{
    [field: SerializeField] public Transform Container { get; private set; }

    public int Layer => gameObject.layer;
}

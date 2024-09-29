using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public static Slot[] slots = new Slot[4];

    private void Awake()
    {
        slots[index] = this;
    }

    public int index;

    public TMP_Text text;

    public bool active;
}

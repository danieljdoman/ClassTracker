using UnityEngine;

public class Hyperlink : MonoBehaviour
{
    public void Interact(string URL)
    {
        Application.OpenURL(URL);
    }
}

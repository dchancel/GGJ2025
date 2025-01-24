using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float conveyorMoveSpeed;
    public bool isPlaying;

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

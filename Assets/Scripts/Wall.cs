using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private PlayerPosition _typeWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager.Instance.SwitchPositionTo(_typeWall);
    }
}

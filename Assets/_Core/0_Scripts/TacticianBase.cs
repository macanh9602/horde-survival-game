using UnityEngine;

public abstract class TacticianBase : MonoBehaviour, IMovement
{
    public virtual void MoveTo(Vector3 worldPos)
    {
        //transform.position = worldPos;
    }
}
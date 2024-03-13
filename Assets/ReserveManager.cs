using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserveManager : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> m_ReserveSlots;

    public void AddPawnToReserve(int playerID, Pawn pawn)
    {
        if (playerID == 0)
        {
            SpriteRenderer sprite = GetEmptySlot();
            pawn.transform.SetParent(sprite.transform);
            pawn.transform.SetPositionAndRotation(sprite.transform.position, Quaternion.identity);
        }
        else
        {
            SpriteRenderer sprite = GetEmptySlot();
            pawn.transform.SetParent(sprite.transform);
            pawn.transform.SetPositionAndRotation(sprite.transform.position, Quaternion.identity);
        }
        pawn.transform.localPosition = new Vector3();
    }

    private SpriteRenderer GetEmptySlot()
    {
        for (int i = 0; i < m_ReserveSlots.Count; i++)
        {
            if (m_ReserveSlots[i].gameObject.transform.childCount == 0) return m_ReserveSlots[i];
        }
        return null;
    }
}

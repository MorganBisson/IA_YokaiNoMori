using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReserveManager : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> m_ReserveSlots;

    public void AddPawnToReserve(int playerID, Pawn pawn)
    {
        if (playerID == 0)
        {
            SpriteRenderer sprite = GetEmptySlot();
            pawn.transform.SetParent(sprite.transform);
            pawn.transform.DOMove(sprite.transform.position, 1f).SetEase(Ease.OutBack);
        }
        else
        {
            SpriteRenderer sprite = GetEmptySlot();
            pawn.transform.SetParent(sprite.transform);
            pawn.transform.DOMove(sprite.transform.position, 1f).SetEase(Ease.OutBack);
        }
    }

    public void RemovePawnFromReserve(int playerID, Pawn pawn)
    {
        if (playerID == 0)
        {
            SpriteRenderer sprite = GetEmptySlot();
            pawn.transform.SetParent(sprite.transform);
            pawn.transform.DOMove(sprite.transform.position, 1f).SetEase(Ease.OutBack);
        }
        else
        {
            SpriteRenderer sprite = GetEmptySlot();
            pawn.transform.SetParent(sprite.transform);
            pawn.transform.DOMove(sprite.transform.position, 1f).SetEase(Ease.OutBack);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPawnData", menuName = "Yokai/PawnData")]
public class PawnData : ScriptableObject
{

    public struct SpawnPosition
    {
        public Vector2 Player1;
        public Vector2 Player2;
    }


    [SerializeField]
    private YokaiType _yokaiType;
    public YokaiType YokaiType => _yokaiType;

    [SerializeField]
    private Sprite _sprite;
    public Sprite PawnSprite => _sprite;

    [SerializeField]
    private Vector2[] _availableDirections;
    public Vector2[] AvailableDirections => _availableDirections;
}

public enum YokaiType
{
    Koropokkuru,
    Kitsune,
    Tanuki,
    Kodama,
    KodamaSamurai
}
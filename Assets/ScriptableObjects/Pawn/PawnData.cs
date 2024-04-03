using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPawnData", menuName = "Yokai/PawnData")]
public class PawnData : ScriptableObject
{

    [System.Serializable]
    public struct SpawnPosition
    {
        public Vector2Int Player1;
        public Vector2Int Player2;
    }


    [SerializeField]
    private YokaiType _yokaiType;
    public YokaiType YokaiType => _yokaiType;

    [SerializeField]
    private Sprite _sprite;
    public Sprite PawnSprite => _sprite;

    [SerializeField]
    private Vector2Int[] _availableDirections;
    public Vector2Int[] AvailableDirections => _availableDirections;


    [SerializeField]
    private SpawnPosition _spawnPosition;

    public SpawnPosition PawnSpawnPosition => _spawnPosition;

}



public enum YokaiType
{
    Koropokkuru,
    Kitsune,
    Tanuki,
    Kodama,
    KodamaSamurai
}
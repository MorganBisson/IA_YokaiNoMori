using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yokai : ScriptableObject
{
    [SerializeField]
    private Sprite _sprite;
    public Sprite PawnSprite => _sprite;


    [SerializeField]
    private Vector2 _spawnPosition;
    public Vector2 SpawnPosition => _spawnPosition;


}

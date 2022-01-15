using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Warrior, 
    Magician
}

/// <summary>
/// 프로퍼티화를위해 시리얼라이저블을사용
/// </summary>
[Serializable]
public class PlayerData  
{
    public int hp;
    public string name;
    public PlayerClass thisCalss;

}

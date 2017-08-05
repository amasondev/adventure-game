using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardinalSprite : ScriptableObject
{
  public Sprite south;
  public Sprite southEast;
  public Sprite east;
  public Sprite northEast;
  public Sprite north;
  public Sprite northWest;
  public Sprite west;
  public Sprite southWest;  
	

  public enum Direction
  {
    South,
    SouthEast,
    East,
    NorthEast,
    North,
    NorthWest,
    West,
    SouthWest
  }
}

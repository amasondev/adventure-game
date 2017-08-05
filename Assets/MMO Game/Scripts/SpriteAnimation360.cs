using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpriteAnimation360 : ScriptableObject
{  
  [Range(0, 60)]
  public float frameRate;
  
  public Texture2D[] sprites;
  public Texture2D[] normalMaps;
}

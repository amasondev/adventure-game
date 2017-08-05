using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class CardinalAnimator : MonoBehaviour
{

  new public SpriteAnimation360 animation;
  
  public uint frames = 1;

  private float time = 0;
  private float maxTime = 0;
  private uint spriteIndex = 0;
  private MaterialPropertyBlock materialPropertyBlock;
  private CardinalSprite.Direction currentDirection = CardinalSprite.Direction.South;

  private CardinalDirection _direction;
  protected CardinalDirection direction
  {
    get
    {
      if (_direction == null) _direction = GetComponent<CardinalDirection>();

      return _direction;
    }
  }

  new public Renderer renderer;

  public uint currentFrame
  {
    get; private set;
  }


  void Awake()
  {
    time = 0;
    if (animation && animation.frameRate != 0)
      maxTime = frames / animation.frameRate;
    else
      maxTime = 1.0f;
  }

  private void OnEnable()
  {
    materialPropertyBlock = new MaterialPropertyBlock();
    if (renderer)
    {
      Billboard b = renderer.GetComponent<Billboard>();
      if (b)
      {
        b.onBillboardRendered -= AnimationUpdate;
        b.onBillboardRendered += AnimationUpdate;
      }
    }
  }

  private void OnDisable()
  {
    if (renderer)
    {
      Billboard b = renderer.GetComponent<Billboard>();
      if (b)
      {
        b.onBillboardRendered -= AnimationUpdate;
      }
    }
  }

  private void Update()
  {
    time += Time.deltaTime;

    if (time > maxTime)
      time %= maxTime;

    uint frame = (uint)(time * animation.frameRate) % frames;
    currentFrame = frame;
  }


  void AnimationUpdate()
  {
    /*
    if (currentDirection != direction.currentDirection)
    {
      UpdateSprite();
      currentDirection = direction.currentDirection;
    }
    else if (currentFrame != frame)
    {
      UpdateSprite();
      currentFrame = frame;
    }
    */
    currentDirection = direction.currentDirection;
    UpdateSprite();
    
  }

  protected void UpdateSprite()
  {
    if (!animation) return;

    spriteIndex = SelectSprite();
    //Sprite s = animation.sprites[spriteIndex];
    Texture2D s = animation.sprites[spriteIndex];
    Texture2D n = animation.normalMaps[spriteIndex];

    //if (s == null) return;

    //materialPropertyBlock.SetTexture("_MainTex", s.texture);
    if (materialPropertyBlock != null)
    {
      if (s != null)
        materialPropertyBlock.SetTexture("_MainTex", s);
      if (n != null)
        materialPropertyBlock.SetTexture("_BumpMap", n);
    }
    renderer.SetPropertyBlock(materialPropertyBlock);
    //spriteRenderer.sprite = s;
  }

  protected uint SelectSprite()
  {
    uint index = 0;

    switch (currentDirection)
    {
      case CardinalSprite.Direction.West:
        break;
      case CardinalSprite.Direction.South:
        index += frames * 1;
        break;
      case CardinalSprite.Direction.SouthWest:
        index += frames * 2;
        break;
      case CardinalSprite.Direction.SouthEast:
        index += frames * 3;
        break;
      case CardinalSprite.Direction.North:
        index += frames * 4;
        break;
      case CardinalSprite.Direction.NorthWest:
        index += frames * 5;
        break;
      case CardinalSprite.Direction.NorthEast:
        index += frames * 6;
        break;
      case CardinalSprite.Direction.East:
        index += frames * 7;
        break;
    }

    index += currentFrame;


    return index;
  }

  protected string GetAnimationName(string format)
  {
    string ret = format;

    ret = ret.Replace("[direction]", direction.currentDirection.ToString().ToLower());
    ret = ret.Replace("[1]", currentFrame.ToString("D"));
    ret = ret.Replace("[2]", currentFrame.ToString("D2"));
    ret = ret.Replace("[3]", currentFrame.ToString("D3"));
    
    return ret;
  }
  
}

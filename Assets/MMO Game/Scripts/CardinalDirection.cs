using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Optimize by using array rather than switch

[ExecuteInEditMode]
public class CardinalDirection : MonoBehaviour {

  private const float PI = Mathf.PI;
  private const float TWOPI = 2 * Mathf.PI;
  private const float ONEEIGTHPI = Mathf.PI / 8.0f;


  #region Components
  
  new public Renderer renderer;

  #endregion


  //public CardinalSprite cardinalSprite;

  private float currentAngle;

  public CardinalSprite.Direction currentDirection = CardinalSprite.Direction.South;


  protected void OnEnable()
  {
    if (renderer)
    {
      Billboard b = renderer.GetComponent<Billboard>();
      b.onBillboardRendered -= OnSpriteRendered;
      b.onBillboardRendered += OnSpriteRendered;
    }
  }

  protected void OnDisable()
  {
    if (renderer)
    {
      Billboard b = renderer.GetComponent<Billboard>();
      b.onBillboardRendered -= OnSpriteRendered;
    }
  }

  protected void OnSpriteRendered()
  {
    if(Camera.current == null) return;

    CardinalSprite.Direction calculatedDirection = GetCurrentDirection(Camera.current);
    currentDirection = calculatedDirection;
  }

    //transform.Rotate(0, currentAngle, 0);

    /*
    if (calculatedDirection != currentDirection)
      UpdateSprite(calculatedDirection);
  }

  protected void UpdateSprite(CardinalSprite.Direction direction)
  {
    if (!cardinalSprite) return;
    if (!spriteRenderer) return;
    Sprite s = cardinalSprite.south;

    // Opposite direction: If you camera is south, we see the back of the object, facing north.
    switch(direction)
    {
      case CardinalSprite.Direction.South:
        s = cardinalSprite.north;
        break;
      case CardinalSprite.Direction.SouthEast:
        s = cardinalSprite.northWest;
        break;
      case CardinalSprite.Direction.East:
        s = cardinalSprite.west;
        break;
      case CardinalSprite.Direction.NorthEast:
        s = cardinalSprite.southWest;
        break;
      case CardinalSprite.Direction.North:
        break;
      case CardinalSprite.Direction.NorthWest:
        s = cardinalSprite.southEast;
        break;
      case CardinalSprite.Direction.West:
        s = cardinalSprite.east;
        break;
      case CardinalSprite.Direction.SouthWest:
        s = cardinalSprite.northEast;
        break;
    }
    spriteRenderer.sprite = s;

    currentDirection = direction;
  }

  */

  protected CardinalSprite.Direction GetCurrentDirection(Camera cam)
  {
    CardinalSprite.Direction direction = CardinalSprite.Direction.South;

    Vector3 localSpacePoint = transform.InverseTransformPoint(cam.transform.position);
    // ONEEIGHTHPI offsets so that our unit circle is rotated.
    float angle = Mathf.Atan2(localSpacePoint.x, localSpacePoint.z) + ONEEIGTHPI + PI;

    angle %= TWOPI;

    angle /= TWOPI;

    // Opposite direction
    // If camera is south, we face north
    direction = (CardinalSprite.Direction)(int)(angle * 8.0f);
    

    currentAngle = angle;


    return direction;
  }


  protected void OnDrawGizmosSelected()
  {
    Matrix4x4 originalMatrix = Gizmos.matrix;
    Gizmos.matrix = transform.localToWorldMatrix;
    Gizmos.color = new Color(0.5f, 0.75f, 1f);

    float lineLength = 2.0f;

    for(int i = 0; i < 8; i++)
    {
      float l = (float)i / 8.0f;
      float radian = l * TWOPI;

      if (i == 1) Gizmos.color = Color.grey;

      Vector3 localPoint = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
      Gizmos.DrawLine(Vector3.zero, localPoint * lineLength);
    }

    Gizmos.matrix = originalMatrix;
  }
}

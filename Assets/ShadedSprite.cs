using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SpriteRenderer))]
public class ShadedSprite : MonoBehaviour {

  public ShadowCastingMode shadowCastingMode = ShadowCastingMode.TwoSided;
  public bool receiveShadows = false;

  #region Components
  private SpriteRenderer _spriteRenderer;
  protected SpriteRenderer spriteRenderer
  {
    get
    {
      if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
      return _spriteRenderer;
    }
  }

  #endregion

  void Reset()
  {
    spriteRenderer.shadowCastingMode = shadowCastingMode;
    spriteRenderer.receiveShadows = receiveShadows;
  }

  void OnValidate()
  {
    spriteRenderer.shadowCastingMode = shadowCastingMode;
    spriteRenderer.receiveShadows = receiveShadows;
  }
}

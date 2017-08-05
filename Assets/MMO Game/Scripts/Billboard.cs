// Copyright 2015 Accel Games, all rights reserved.

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("Rendering/Billboard")]
[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
  public bool vertical = true;

  // Public event allows other scripts to add to OnWillRenderObject
  public delegate void OnBillboardRendered();
  public event OnBillboardRendered onBillboardRendered;

  private static Quaternion BillboardRotation_ViewAligned = Quaternion.identity;
  private static Quaternion BillboardRotation_Vertical = Quaternion.identity;

  #region Components
  private Renderer _renderer;
  new protected Renderer renderer
  {
    get
    {
      if (_renderer == null) _renderer = GetComponent<Renderer>();
      return _renderer;
    }
  }
    
    // Cache transform for highest possible speed
    private Transform _transform;
    protected Transform myTransform
    {
      get
      {
        if (_transform == null) _transform = gameObject.transform;
        return _transform;
      }
    }
  #endregion

  #if UNITY_EDITOR
  [InitializeOnLoadMethod]
  #endif
  [RuntimeInitializeOnLoadMethod]
  private static void InitializedPreCullCallback()
  {
    // Initializes one copy of CalculateBillboardRotations to be called on Camera.onPreCull
    Camera.onPreCull -= CalculateBillboardRotations;
    Camera.onPreCull += CalculateBillboardRotations;
  }

  void OnWillRenderObject()
  {
    if (Camera.current == null) return; 

    Bill();

    if (onBillboardRendered != null)
      onBillboardRendered.Invoke();
  }

  // Called once per billboard for every camera
  // Must be FAST
  protected void Bill()
  {
    myTransform.rotation = (vertical) ? BillboardRotation_Vertical : BillboardRotation_ViewAligned;
  }

  // Called precisely once for each camera PreCull
  private static void CalculateBillboardRotations(Camera cam)
  {
    if (!cam) return;

    // TODO: Modify for camera roll
    BillboardRotation_ViewAligned = cam.transform.rotation;

    BillboardRotation_Vertical = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
  }
}

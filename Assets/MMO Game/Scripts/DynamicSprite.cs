using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class DynamicSprite : MonoBehaviour
{
  [SerializeField]
  private Sprite sprite;
  [SerializeField]
  private Color tint = Color.white;

  public MaterialPropertyBlock properties;

  private static Hashtable mesh_cache = new Hashtable();

  #region Components
  private Renderer _renderer;
  new public Renderer renderer
  {
    get
    {
      if (_renderer == null) _renderer = GetComponent<Renderer>();

      return _renderer;
    }
  }
  private MeshFilter _meshFilter;
  public MeshFilter meshFilter
  {
    get
    {
      if (_meshFilter == null) _meshFilter = GetComponent<MeshFilter>();

      return _meshFilter;
    }
  }
  #endregion

  void OnValidate()
  {
    UpdateSprite();
  }

  private void OnEnable()
  {
    UpdateSprite();
  }

  protected void UpdateSprite()
  {
    SetupMesh();
    SetupMaterialPropertyBlock();
  }

  protected void SetupMaterialPropertyBlock()
  {
    properties = new MaterialPropertyBlock();

    if (sprite)
      properties.SetTexture("_MainTex", sprite.texture);
    else
      properties.SetTexture("_MainTex", null);

    properties.SetColor("_Color", tint);

    renderer.SetPropertyBlock(properties);
  }

  public void SetupMesh()
  {
    if (!sprite)
    {
      if (meshFilter.sharedMesh) DestroyImmediate(meshFilter.sharedMesh);

      meshFilter.sharedMesh = null;
      return;
    }

    Mesh mesh;

    // Is the mesh already in cache?
    if (mesh_cache[sprite] != null)
    {
      Debug.Log("Using cached mesh.");
      mesh = mesh_cache[sprite] as Mesh;

    }
    else
    {
      mesh = new Mesh();
      mesh.name = sprite.name + " Generated Mesh";
      mesh.MarkDynamic();
      mesh.Clear();
      mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
      mesh.uv = sprite.uv;
      mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);
      mesh.RecalculateNormals();

      mesh_cache[sprite] = mesh;
    }
    
    meshFilter.sharedMesh = mesh;
  }


  public void SetSprite(Sprite s)
  {
    sprite = s;
    UpdateSprite();
  }

  public void SetTint(Color c)
  {
    tint = c;
    UpdateSprite();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
public class CustomVignetteImageEffect : ImageEffectBase {
  public Color fogColor;
  public float radius;
  public float smoothness;
  
  // Called by camera to apply image effect
  void OnRenderImage (RenderTexture source, RenderTexture destination) {
    material.SetColor ("_Fog", fogColor);
    material.SetFloat ("_Radius", radius);
    material.SetFloat ("_Smoothness", smoothness);
    Graphics.Blit (source, destination, material);
  }
}
}

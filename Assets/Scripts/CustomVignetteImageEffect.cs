using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomVignetteImageEffect : MonoBehaviour {
  public Texture  vignetteTexture;

  // Called by camera to apply image effect
  void OnRenderImage (RenderTexture source, RenderTexture destination) {
    // Graphics.Blit (source, destination, material);
  }
}

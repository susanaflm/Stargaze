using UnityEngine;
using UnityEngine.Rendering;

public class Blip : MonoBehaviour
{
    private CommandBuffer cmd;
    
    // A Material with the Unity shader you want to process the image with
    public Material mat;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Read pixels from the source RenderTexture, apply the material, copy the updated results to the destination RenderTexture
        cmd.Blit(src, dest, mat);
    }
}

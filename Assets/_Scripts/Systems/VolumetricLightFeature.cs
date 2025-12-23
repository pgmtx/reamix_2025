using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class VolumetricLightSettings
{
    public Material shadingMaterial = null;
    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
    public int maxSteps = 200;
    public float maxDistance = 50f;
    public float scatteringPower = 1.0f;
    public bool executeInSceneView = false;
    public bool debugRaymarchBuffer = false;
    public RenderTextureFormat tempTextureFormat = RenderTextureFormat.DefaultHDR;
    public bool jitter = true;
}

public class VolumetricLightFeature : ScriptableRendererFeature
{
    public VolumetricLightSettings settings = new VolumetricLightSettings();

    class VolumetricPass : ScriptableRenderPass
    {
        public VolumetricLightSettings settings;
        private RenderTargetHandle tempTexture;

        public VolumetricPass(VolumetricLightSettings s)
        {
            settings = s;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            tempTexture.Init("_TempVolumetricTex");
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor.width, cameraTextureDescriptor.height, 0, FilterMode.Bilinear, settings.tempTextureFormat);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (settings.shadingMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Volumetric Light");

            // Récupération de la camera target à l'intérieur du pass
            var cameraTarget = renderingData.cameraData.renderer.cameraColorTarget;

            // Pass settings au shader
            settings.shadingMaterial.SetInt("_MaxSteps", settings.maxSteps);
            settings.shadingMaterial.SetFloat("_MaxDistance", settings.maxDistance);
            settings.shadingMaterial.SetFloat("_ScatteringPower", settings.scatteringPower);
            settings.shadingMaterial.SetInt("_Jitter", settings.jitter ? 1 : 0);

            // Blit vers temporary RT puis vers l'écran
            cmd.Blit(cameraTarget, tempTexture.Identifier(), settings.shadingMaterial);
            if (settings.debugRaymarchBuffer)
                cmd.Blit(tempTexture.Identifier(), cameraTarget);
            else
                cmd.Blit(tempTexture.Identifier(), cameraTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (tempTexture != RenderTargetHandle.CameraTarget)
                cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    VolumetricPass pass;

    public override void Create()
    {
        pass = new VolumetricPass(settings);
        pass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.shadingMaterial != null && (settings.executeInSceneView || !renderingData.cameraData.isSceneViewCamera))
        {
            // On ne touche pas à cameraColorTarget ici !
            renderer.EnqueuePass(pass);
        }
    }
}

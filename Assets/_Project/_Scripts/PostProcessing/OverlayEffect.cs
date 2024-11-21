using UnityEngine;

[ExecuteInEditMode]
public class OverlayEffect : MonoBehaviour {
    public Material overlayMaterial;  // Назначь сюда материал с текстурой бумаги

    private void OnRenderImage( RenderTexture src, RenderTexture dest ) {
        if ( overlayMaterial != null ) {
            // Передаём изображение сцены в _MainTex шейдера
            overlayMaterial.SetTexture( "_MainTex", src );
            Graphics.Blit( src, dest, overlayMaterial );
        } else {
            // Если материал не назначен, выводим изображение без изменений
            Graphics.Blit( src, dest );
        }
    }
}

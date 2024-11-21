using UnityEngine;

[ExecuteInEditMode]
public class OverlayEffect : MonoBehaviour {
    public Material overlayMaterial;  // ������� ���� �������� � ��������� ������

    private void OnRenderImage( RenderTexture src, RenderTexture dest ) {
        if ( overlayMaterial != null ) {
            // ������� ����������� ����� � _MainTex �������
            overlayMaterial.SetTexture( "_MainTex", src );
            Graphics.Blit( src, dest, overlayMaterial );
        } else {
            // ���� �������� �� ��������, ������� ����������� ��� ���������
            Graphics.Blit( src, dest );
        }
    }
}

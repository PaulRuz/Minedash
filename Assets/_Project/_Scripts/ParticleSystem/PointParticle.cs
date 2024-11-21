using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PointParticle : BaseParticle {
    [SerializeField] TextDisplay textDisplay;
    [SerializeField] float baseSpeed = 1f;
    [SerializeField][Range( 0.1f, 0.8f )] float timeDeactivate;

    public void SetText( int value, Color color, float fontSize, string prefix) {
        textDisplay.SetText( $"{prefix}{value}", color, fontSize );
    }
    protected override void ConfigureParticle() {
        _speed = baseSpeed;
    }
    protected override IEnumerator PostActivateActions() {
        yield return new WaitForSeconds( timeDeactivate );
        Deactivate();
    }
}

[Serializable]
public class TextDisplay {
    public TextMeshPro textMesh;
    public void SetText( string content, Color color, float fontSize ) {
        textMesh.text = content;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DustEffectHandler : MonoBehaviour
{
 // Arraste o sistema de partículas no Inspector
    [SerializeField] 
    private ParticleSystem dustSystem;

    void Start()
    {
        // Configura a poeira
        SetupDustSystem();
    }

    void SetupDustSystem()
    {
        var main = dustSystem.main;
        main.startLifetime = 5.0f; // Quanto tempo cada partícula vive
        main.startSize = 0.1f;     // Tamanho bem pequeno
        main.startColor = new Color(1f, 1f, 1f, 0.3f); // Branco transparente
        
        var emission = dustSystem.emission;
        emission.rateOverTime = 5; // Cria 5 partículas por segundo
        
        var vel = dustSystem.velocityOverLifetime;
        vel.x = new MinMaxCurve(-0.1f, 0.1f); // Move levemente pros lados
        vel.y = new MinMaxCurve(-0.05f, 0.05f); // Move levemente pra cima/baixo
    }
}

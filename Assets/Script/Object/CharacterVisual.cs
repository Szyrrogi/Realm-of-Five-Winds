using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wizualizacja postaci złożonej z osobnych SpriteRendererów (części ciała).
/// - Nogi stoją stabilnie w miejscu.
/// - Płaszcz i brzuch są zsynchronizowane (poruszają się identycznie).
/// - Obsługa obracającej się kuli w osi Z.
/// </summary>
public class CharacterVisual : MonoBehaviour
{
    [Header("Wymagane części ciała")]
    public Transform nogaLewa;
    public Transform nogaPrawa;
    public Transform rekaLewa;
    public Transform rekaPrawa;
    public Transform glowa;
    public Transform brzuch;
    public Transform szyja;

    [Header("Opcjonalne części ciała")]
    public Transform plaszcz;
    public Transform wlosyTyl;
    public Transform skrzydla;
    public Transform skrzydlaDodatkowe;
    public Transform bron;
    
    [Header("Opcjonalna Kula (Rotacja Z)")]
    [Tooltip("Obiekt kuli, która będzie obracać się wokół własnej osi")]
    public Transform kula;
    [Tooltip("Prędkość obrotu kuli (w stopniach na sekundę)")]
    public float kulaRotationSpeed = 180f;

    [Header("Ustawienia animacji idle")]
    public float amplitude = 0.06f;
    public float speed = 1.4f;
    [Range(0f, 1f)] public float phaseRandomness = 0.8f;
    public int randomSeed = 0;

    [Header("Ustawienia animacji śmierci")]
    public float deathDuration = 1.2f;
    public float scatterRadius = 1.5f;
    public float scatterTorque = 360f;
    public AnimationCurve deathFadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private class PartData
    {
        public Transform t;
        public SpriteRenderer sr;
        public Vector3 basePos;
        public Quaternion baseRot;
        public float phase;
        public float ampMul;
        public float speedMul;
        public bool isRotating; 
    }

    private readonly List<PartData> parts = new List<PartData>();
    private bool isDead;
    private System.Random rng;

    void Awake()
    {
        rng = randomSeed != 0 ? new System.Random(randomSeed) : new System.Random();

        // Nogi w ogóle się nie ruszają - amplituda ustawiona na 0
        AddPart(nogaLewa, 0f, 0f);
        AddPart(nogaPrawa, 0f, 0f);
        
        AddPart(rekaLewa, 1f, 1f);
        AddPart(rekaPrawa, 1f, 1f);

        // Losujemy jedną wspólną fazę dla korpusu (brzucha) i płaszcza
        float torsoPhase = (float)(rng.NextDouble() * phaseRandomness * Mathf.PI * 2f);

        // Brzuch i Płaszcz korzystają z tej samej fazy, prędkości (0.75) i wychylenia (0.55)
        AddPart(brzuch, 0.55f, 0.75f, false, torsoPhase);
        AddPart(plaszcz, 0.55f, 0.75f, false, torsoPhase);

        AddPart(szyja, 0.45f, 0.85f);
        AddPart(glowa, 0.65f, 0.9f);

        AddPart(wlosyTyl, 0.9f, 0.9f);
        AddPart(skrzydla, 1.7f, 0.55f);
        AddPart(skrzydlaDodatkowe, 1.9f, 0.5f);
        AddPart(bron, 0.8f, 1f);
        
        AddPart(kula, 0.5f, 1f, true); 
    }

    /// <summary>
    /// Dodaje część ciała do animacji.
    /// Parametr customPhase pozwala wymusić konkretną fazę (użyte do zsynchronizowania brzucha z płaszczem).
    /// </summary>
    void AddPart(Transform t, float ampMul, float speedMul, bool isRotating = false, float? customPhase = null)
    {
        if (t == null) return;

        var sr = t.GetComponent<SpriteRenderer>();

        // Ustawiamy wymuszoną fazę, a jeśli jej nie ma (null), losujemy nową
        float finalPhase = customPhase.HasValue ? customPhase.Value : (float)(rng.NextDouble() * phaseRandomness * Mathf.PI * 2f);

        parts.Add(new PartData
        {
            t = t,
            sr = sr,
            basePos = t.localPosition,
            baseRot = t.localRotation,
            phase = finalPhase,
            ampMul = ampMul,
            speedMul = speedMul,
            isRotating = isRotating
        });
    }

    void Update()
    {
        if (isDead) return;

        float time = Time.time;
        foreach (var p in parts)
        {
            // Animacja unoszenia się 
            float y = Mathf.Sin(time * speed * p.speedMul + p.phase) * amplitude * p.ampMul;
            p.t.localPosition = p.basePos + new Vector3(0f, y, 0f);

            // Rotacja kuli w osi Z
            if (p.isRotating)
            {
                p.t.localRotation = p.baseRot * Quaternion.Euler(0f, 0f, time * kulaRotationSpeed);
            }
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllCoroutines();
        StartCoroutine(DeathAnimation());
    }

    public void ResetVisual()
    {
        StopAllCoroutines();
        isDead = false;
        foreach (var p in parts)
        {
            p.t.localPosition = p.basePos;
            p.t.localRotation = p.baseRot;
            if (p.sr != null)
            {
                Color c = p.sr.color;
                c.a = 1f;
                p.sr.color = c;
            }
        }
    }

    private struct ScatterInfo
    {
        public PartData part;
        public Vector3 startPos;
        public Quaternion startRot; 
        public Vector3 targetPos;
        public float targetRotZ;
        public float delay;
    }

    private IEnumerator DeathAnimation()
    {
        var scatter = new List<ScatterInfo>(parts.Count);

        foreach (var p in parts)
        {
            Vector2 dir = Random.insideUnitCircle;
            if (dir.sqrMagnitude < 0.001f) dir = Vector2.up;
            dir.Normalize();

            Vector3 startPos = p.t.localPosition;
            Quaternion startRot = p.t.localRotation; 
            Vector3 target = startPos + new Vector3(dir.x, Mathf.Abs(dir.y) * 0.3f - 0.6f, 0f) * Random.Range(scatterRadius * 0.5f, scatterRadius);

            scatter.Add(new ScatterInfo
            {
                part = p,
                startPos = startPos,
                startRot = startRot,
                targetPos = target,
                targetRotZ = Random.Range(-scatterTorque, scatterTorque),
                delay = Random.Range(0f, deathDuration * 0.3f)
            });
        }

        float elapsed = 0f;
        while (elapsed < deathDuration)
        {
            elapsed += Time.deltaTime;

            foreach (var d in scatter)
            {
                float localT = Mathf.Clamp01((elapsed - d.delay) / Mathf.Max(0.0001f, deathDuration - d.delay));
                if (localT <= 0f) continue;

                float eased = 1f - Mathf.Pow(1f - localT, 3f); 

                d.part.t.localPosition = Vector3.Lerp(d.startPos, d.targetPos, eased);
                d.part.t.localRotation = d.startRot * Quaternion.Euler(0f, 0f, d.targetRotZ * eased);

                if (d.part.sr != null)
                {
                    Color c = d.part.sr.color;
                    c.a = deathFadeCurve.Evaluate(localT);
                    d.part.sr.color = c;
                }
            }

            yield return null;
        }

        foreach (var d in scatter)
        {
            if (d.part.sr != null)
            {
                Color c = d.part.sr.color;
                c.a = 0f;
                d.part.sr.color = c;
            }
        }
    }
}
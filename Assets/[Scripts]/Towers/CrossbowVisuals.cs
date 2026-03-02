using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = 0.1f;

    [Header("Glowing Visuals")]
    [SerializeField] private MeshRenderer meshRenderer;
    private Material material;

    [Space]
    [SerializeField] private float maxIntensity = 150f;
    private float currentIntensity;

    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Rotor Visuals")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorUnloaded;
    [SerializeField] private Transform rotorLoaded;

    [Header("Front Glow String")]
    [SerializeField] private LineRenderer frontStringL;
    [SerializeField] private LineRenderer frontStringR;

    [Space]

    [SerializeField] private Transform frontStartPointL;
    [SerializeField] private Transform frontStartPointR;
    [SerializeField] private Transform frontEndPointL;
    [SerializeField] private Transform frontEndPointR;

    [Header("Front Glow String")]
    [SerializeField] private LineRenderer backStringL;
    [SerializeField] private LineRenderer backStringR;

    [Space]
    [SerializeField] private Transform backStartPointL;
    [SerializeField] private Transform backStartPointR;
    [SerializeField] private Transform backEndPointL;
    [SerializeField] private Transform backEndPointR;

    [SerializeField] private LineRenderer[] lineRenderers;

    private void Awake()
    {
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;

        UpdateLineRenderersMaterials();

        StartCoroutine(ChangeEmission(1));
    }


    private void Update()
    {
        EmissionAlert();
        UpdateStrings();
        UpdateAttackVisualsIfNeeded();
    }
    private void UpdateLineRenderersMaterials()
    {
        foreach (LineRenderer lr in lineRenderers)
        {
            lr.material = material;
        }
    }

    private void UpdateAttackVisualsIfNeeded()
    {
        if (attackVisuals.enabled && enemy != null)
            attackVisuals.SetPosition(1, enemy.CenterPoint());
    }

    private void UpdateStrings()
    {
        UpdateStringVisual(frontStringL, frontStartPointL, frontEndPointL);
        UpdateStringVisual(frontStringR, frontStartPointR, frontEndPointR);
        UpdateStringVisual(backStringL, backStartPointL, backEndPointL);
        UpdateStringVisual(backStringR, backStartPointR, backEndPointR);
    }

    private void EmissionAlert()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);

        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);

        material.SetColor("_EmissionColor", emissionColor);
    }

    public void PlayReloadVFX(float duration)
    {
        float newDuration = duration / 2;

        StartCoroutine(ChangeEmission(newDuration));
        StartCoroutine(UpdateRotorPosition(newDuration));
    }

    public void PlayAttackFX(Vector3 starPoint, Vector3 endPoint, Enemy newEnemy)
    {
        StartCoroutine(VFXCoroutine(starPoint, endPoint, newEnemy));
    }

    private IEnumerator VFXCoroutine(Vector3 starPoint, Vector3 endPoint, Enemy newEnemy)
    {
        //towerCrossbow.EnableRotation(false);
        enemy = newEnemy;

        attackVisuals.enabled = true;

        attackVisuals.SetPosition(0, starPoint);
        attackVisuals.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDuration);

        attackVisuals.enabled = false;

        //towerCrossbow.EnableRotation(true);
    }

    private IEnumerator ChangeEmission(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0f;


        while (Time.time - startTime < duration) 
        {
            //Calculates the proportion of the duration that has elapsed since the start of the coroutine
            float tValue = (Time.time - startTime) / duration;

            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
        }

        currentIntensity = maxIntensity;
    }

    private IEnumerator UpdateRotorPosition(float duration)
    {
        float starTime = Time.time;

        while (Time.time - starTime < duration)
        {
            float tValue = (Time.time - starTime) / duration;

            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, tValue);
            yield return null;
        }
        rotor.position = rotorLoaded.position;
    }

    private void UpdateStringVisual(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
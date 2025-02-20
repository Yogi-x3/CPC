using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostFX : MonoBehaviour
{
    public UI UIscript;


    [Header("Volume")]
    public Volume post;
    [Header("Bloom")]
    private Bloom bloom;
    public float bloomThreshold;
    public float bloomIntensity;
    [Header("Grain")]
    private FilmGrain grain;
    public float grainIntensity;
    [Header("Abberation")]
    private ChromaticAberration chromatic;
    public float chromaticIntensity;
    [Header("Vignette")]
    private Vignette vignette;
    public float vignetteIntensity;
    public bool vignetteEnabled;
    [Header("ColorAdjust")]
    private ColorAdjustments colorAdjust;
    public float postExposure;
    public float saturation;
    private bool colorAdjustEnabled;
    [Header("DOF")]
    private DepthOfField DOF;
    public float DOFDistance;
    private bool depthOfFieldEnabled;
    [Header("Panini")]
    private PaniniProjection panini;
    public float paniniDistance;
    private bool paniniEnabled;
    [Header("Toning")]
    private SplitToning toning;
    public float toningBalance;
    // Start is called before the first frame update
    void Start()
    {
        vignetteEnabled = false;
        colorAdjustEnabled = false;
        saturation = 0f;
        toningBalance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        post.profile.TryGet(out bloom);
        {
            bloom.intensity.value = bloomIntensity;
        }
        post.profile.TryGet(out chromatic);
        {
            chromatic.intensity.value = chromaticIntensity;
        }
        post.profile.TryGet(out vignette);
        {
            vignette.intensity.value = vignetteIntensity;
            vignette.active = vignetteEnabled;
        }
        post.profile.TryGet(out colorAdjust);
        {
            colorAdjust.postExposure.value = postExposure;
            colorAdjust.saturation.value = saturation;
            colorAdjust.active = colorAdjustEnabled;
        }
        post.profile.TryGet(out DOF);
        {
            DOF.focusDistance.value = DOFDistance;
        }
        post.profile.TryGet(out panini);
        {
            panini.distance.value = paniniDistance;
        }
        post.profile.TryGet(out toning);
        {
            toning.balance.value = toningBalance;
        }
    }

    public void Absolved()
    {
        bloomIntensity = 4f;
        colorAdjustEnabled = true;
        toningBalance = 10f;
    }

    public void Guilt()
    {
        vignetteEnabled = true;
        vignetteIntensity = 0.5f;
        colorAdjustEnabled = true;
        saturation = -20f;
        bloomIntensity = 0f;
    }

    public void Damnation()
    {

    }
}

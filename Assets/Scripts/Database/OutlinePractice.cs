using UnityEngine;

public class OutlinePractice : MonoBehaviour
{
    public Color setcolor = Color.white;

    public int outlineSize = 3;

    private SpriteRenderer sr;

    public void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", setcolor);
        mpb.SetFloat("_OutlineSize", outlineSize);
        sr.SetPropertyBlock(mpb);
    }


    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    private void OnDisable()
    {
        UpdateOutline(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            setcolor = Color.black;
            UpdateOutline(true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
            setcolor = Color.red;
        else if (Input.GetKeyDown(KeyCode.D))
            setcolor = Color.blue;


        
    }
}

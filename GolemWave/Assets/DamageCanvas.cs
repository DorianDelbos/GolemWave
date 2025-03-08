using TMPro;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    TextMeshProUGUI text;
    float alpha = 1f;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(-0.5f,1,0) * 1.5f * Time.deltaTime;
        alpha -= Time.deltaTime * 2f;

        Color newColor = text.color;
        newColor.a = alpha;
        text.color = newColor;

        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        if (alpha <= 0f)
        {
            Destroy(gameObject);
        }
    }
}

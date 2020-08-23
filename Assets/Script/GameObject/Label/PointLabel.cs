using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PointLabel : MonoBehaviour
{
    private const float MOVE_SPEED = 0.010f;
    private const float DURATION = 0.5f;

    // public BigInteger value;
    public string displayText;
    private RectTransform rectTransform;
    [SerializeField] Text label;
    
    public float cd;

    private UnityEngine.Vector3 showPosition = new UnityEngine.Vector3(0,0,0);
    private Camera mainCamera;


   /// <summary>
   /// Awake is called when the script instance is being loaded.
   /// </summary>
   void Awake()
   {
       if(label)
            label.text = displayText;
            mainCamera = Camera.main;
   }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(!rectTransform) // not set;
            return;

        cd -= Time.deltaTime;
        rectTransform.position += UnityEngine.Vector3.up * MOVE_SPEED;
        Color c = label.color;

        if(cd/DURATION > 0.5f)
            c.a = 1;
        else
            c.a = (cd/DURATION)*2f;
            label.color = c;
    }

    /// <summary>
    /// 注意：增加錢還是要自己來
    /// </summary>
    /// <param name="startWorldPosition"></param>
    /// <param name="myvalue"></param>
    public void Set(UnityEngine.Vector3 showPosition, string text)
    {
        transform.position = showPosition;
        Debug.Log("mainCamera.transform.position"+ mainCamera.transform.position);
        Debug.Log("transform.position"+ transform.position);
        label.text = text;
        rectTransform = GetComponent<RectTransform>();
        cd = DURATION;
        Destroy(gameObject, 4);
    }

}

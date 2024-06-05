using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    public GameObject photoFrame;
    // Start is called before the first frame update
    void Start()
    {
        Common.Instance().loadImages(new DirectoryInfo(@"C:\Users\sunxy\Pictures\Wallhaven"));
        // instatiate photoFrame
        for (int i = 0; i < Common.Instance().photos.Count; i++)
        {
            Photo p = Common.Instance().photos[i];
            GameObject go = Instantiate(photoFrame, new Vector3(5, 5, i * 2), Quaternion.identity);
            Image img = go.GetComponentInChildren<Image>();
            img.sprite = p.sprite;
            img.preserveAspect = true;
            go.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(4, 4 * p.height / p.width);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

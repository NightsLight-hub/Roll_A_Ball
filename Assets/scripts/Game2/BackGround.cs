using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Concurrent;
using Unity.VisualScripting;

public class BackGround : MonoBehaviour
{
    public string imagePath = @"C:\Users\Sunxy\Pictures\Camera Roll";
    // photoFrame perfab
    public GameObject photoFrame;
    
    // Start is called before the first frame update
    void Start()
    {
        loadImage();
    }

    void Update()
    {

    }

    private void loadImage()
    {
        Common.Instance().loadImages(new DirectoryInfo(imagePath));
        // instatiate photoFrame
        for (int i = 0; i < Common.Instance().photos.Count; i++)
        {
            Photo p = Common.Instance().photos[i];
            // create photo object and its position
            GameObject photoFrameInstance = Instantiate(photoFrame, new Vector3(i / 2 * 3, 2, i % 2 * - 20  + 10), Quaternion.identity);
            Image img = photoFrameInstance.GetComponentInChildren<Image>();
            img.sprite = p.sprite;
            img.preserveAspect = true;

            // 调整photoBoard 匹配照片大小
            GameObject photoBoard = photoFrameInstance.transform.Find("PhotoBoard").gameObject;
            Debug.LogFormat("scale is {0}", photoBoard.transform.localScale);
            // if height > width, than set y to 1 and  scale x, or else scale y
            if (p.height < p.width)
            {
                photoBoard.transform.localScale = Vector3.Scale(photoBoard.transform.localScale, new Vector3(1, p.height / p.width, 1));
            }
            else
            {
                photoBoard.transform.localScale = Vector3.Scale(photoBoard.transform.localScale, new Vector3(p.width / p.height, 1, 1));
            }

            Debug.LogFormat("new scale is {0}", photoBoard.transform.localScale);

            // rotate photo
            photoFrameInstance.transform.Rotate(new Vector3(0, i % 2 * -180 + 180, 0));

            Mesh m = photoBoard.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = m.vertices;
            // print vertices
            for (int j = 0; j < vertices.Length; j++)
            {
                Debug.LogFormat("vertices {0} is {1}", j, vertices[j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

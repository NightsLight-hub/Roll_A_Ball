using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    // photoFrame perfab
    public GameObject photoFrame;
    // Start is called before the first frame update
    void Start()
    {
        Common.Instance().loadImages(new DirectoryInfo(@"C:\Users\sunxy\Pictures\Wallhaven"));
        // instatiate photoFrame
        for (int i = 0; i < Common.Instance().photos.Count; i++)
        {
            Photo p = Common.Instance().photos[i];
            GameObject photoFrameInstance = Instantiate(photoFrame, new Vector3(i % 2 * -20 + 10, 2, 500 - i / 2 * 3), Quaternion.identity);
            Image img = photoFrameInstance.GetComponentInChildren<Image>();
            img.sprite = p.sprite;
            img.preserveAspect = true;
            GameObject photoBoard = photoFrameInstance.transform.Find("PhotoBoard").gameObject;
            Debug.LogFormat("scale is {0}", photoBoard.transform.localScale);
            // 调整photoBoard 匹配照片大小
            photoBoard.transform.localScale = Vector3.Scale(photoBoard.transform.localScale, new Vector3(1, p.height / p.width, 1));
            Debug.LogFormat("new scale is {0}", photoBoard.transform.localScale);
            photoFrameInstance.transform.Rotate(new Vector3(0, i % 2 * 180 - 90, 0));

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

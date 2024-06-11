using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Concurrent;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class BackGround : MonoBehaviour
{
    public string imagePath = @"C:\Users\Sunxy\Pictures\Camera Roll";
    // photoFrame perfab
    public GameObject photoFrame;
    private Task loadImageTask;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        loadImage();
        coroutine = generatePhoto();
        StartCoroutine(coroutine);
    }

    private void loadImage()
    {
        // start task
        loadImageTask = new Task(() => Common.Instance().loadImages(new DirectoryInfo(imagePath)));//无参数无返回值的方法
        loadImageTask.Start();

    }

    IEnumerator generatePhoto()
    {
        int i = -1;
        //instatiate photoFrame
        while (true)
        {
            i++;
            PhotoRawData? pn = Common.Instance().TryGetPhoto();
            if (pn == null)
            {
                yield return null;
                continue;
            }
            PhotoRawData p = (PhotoRawData)pn;
            Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            texture2D.LoadImage(p.rawData);
            if (texture2D.width < 500)
            {
                yield return null;
            }
            float width = texture2D.width;
            float height = texture2D.height;
            //texture2D.format.
            Debug.LogFormat("load image {0}, width {1}, height {2}", p.name, width, height);
            // convert texture to sprite
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            // create photo object and its position
            GameObject photoFrameInstance = Instantiate(photoFrame, new Vector3(i / 2 * 10, 3, i % 2 * -20 + 10), Quaternion.identity);
            Image img = photoFrameInstance.GetComponentInChildren<Image>();
            img.sprite = sprite;
            img.preserveAspect = true;

            // 调整photoBoard 匹配照片大小
            GameObject photoBoard = photoFrameInstance.transform.Find("PhotoBoard").gameObject;
            Debug.LogFormat("scale is {0}", photoBoard.transform.localScale);
            // if height > width, than set y to 1 and  scale x, or else scale y
            if (height < width)
            {
                photoBoard.transform.localScale = Vector3.Scale(photoBoard.transform.localScale, new Vector3(1, height / width, 1));
            }
            else
            {
                photoBoard.transform.localScale = Vector3.Scale(photoBoard.transform.localScale, new Vector3(width / height, 1, 1));
            }

            Debug.LogFormat("new scale is {0}", photoBoard.transform.localScale);

            // rotate photo
            photoFrameInstance.transform.Rotate(new Vector3(0, i % 2 * -180 + 180, 0));

            // debug print vertices of photoBoard
            //Mesh m = photoBoard.GetComponent<MeshFilter>().mesh;
            //Vector3[] vertices = m.vertices;
            //// print vertices
            //for (int j = 0; j < vertices.Length; j++)
            //{
            //    Debug.LogFormat("vertices {0} is {1}", j, vertices[j]);
            //}
            yield return null;
        }
    }

    private void Update()
    {

    }
}

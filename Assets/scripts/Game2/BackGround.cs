using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Concurrent;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System;

public class BackGround : MonoBehaviour
{
    //public string imagePath = @"C:\Users\Sunxy\Pictures\Camera Roll";
    public string imagePath = @"F:\testData";


    // photoFrame perfab
    public GameObject photoFrame;
    private Task loadImageTask;
    private List<Vector3> photoPath;
    private List<Vector3> photoPath2;
    private const string photoPathCursorPrefix = "PhotoPathCursor";
    private const string photoPathCursor2Prefix = "PhotoPathCursor2";
    private Vector3 offset = new Vector3(0, 3f, 0);

    private void Awake()
    {
        Debug.Log("Game2 background start");
        loadImage();
        loadPhotoPath();
        //StartCoroutine(generatePhotoAsync());
        generatePhoto();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void loadPhotoPath()
    {
        photoPath = new List<Vector3>();
        photoPath2 = new List<Vector3>();
        for (int i = 0; i < 11; i++)
        {
            var name = $"/terrains/{photoPathCursorPrefix}_{i}";
            var c = GameObject.Find(name);
            if (c != null)
            {
                photoPath.Add(c.transform.position);
            }
            else
            {
                break;
            }
        }
        for (int i = 0; i < 11; i++)
        {
            var name = $"/terrains/{photoPathCursor2Prefix}_{i}";
            var c = GameObject.Find(name);
            if (c != null)
            {
                photoPath2.Add(c.transform.position);
            }
            else
            {
                break;
            }
        }
    }

    private void loadImage()
    {
        // start task
        loadImageTask = new Task(() => Common.Instance.LoadImages(new DirectoryInfo(imagePath)));//无参数无返回值的方法
        loadImageTask.Start();
    }

    // 照片放在 路径两点之间的 1/8， 3/8， 5/8， 7/8 的位置
    private Vector3? GetPhotoPosition(int i, List<Vector3> photoPath)
    {
        int photoSubpathIndex = i / 4;
        int photoSubpathPos = i % 4 * 2 + 1;
        if (photoSubpathIndex >= photoPath.Count - 1)
        {
            return null;
        }
        return Vector3.Lerp(photoPath[photoSubpathIndex], photoPath[photoSubpathIndex + 1], (float)photoSubpathPos / 8);
    }


    void generatePhoto()
    {
        // todo this coroutine will never stop so far!!!
        Debug.Log("generatePhoto start");
        List<Vector3> path = this.photoPath;
        bool flag = false;

        int i = -1;
        //instatiate photoFrame
        while (true)
        {
            PhotoRawData? pn = Common.Instance.TryGetPhoto();
            if (pn == null)
            {
                if(Common.Instance.isLoadImage)
                {
                    continue;
                }
                else
                {
                    Debug.Log("generatePhoto finished!");
                    break;
                }
            }
            i++;
            // 一条路最多40张图
            if (i == 40)
            {
                if (!flag)
                {
                    i = 0;
                    path = this.photoPath2;
                    flag = true;
                }
                else
                {
                    // 两条路都填满了，退出
                    break;
                }
            }
            PhotoRawData p = (PhotoRawData)pn;
            DateTime startTime = DateTime.Now;
            Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            texture2D.LoadImage(p.rawData);
            if (texture2D.width < 500)
            {
                continue;
            }
            float width = texture2D.width;
            float height = texture2D.height;
            float xscale = 1;
            float yscale = 1;
            if (width > height)
            {
                yscale = height / width;
            }
            else
            {
                xscale = width / height;
            }
            //texture2D.format.
            //Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            // create photo object and its position
            Vector3? position = GetPhotoPosition(i, path);
            if (position == null)
            {
                Debug.LogFormat("no more position to generate photo，generatePhoto exit");
                return;
            }
            position += offset;
            Debug.LogFormat("generate photo at position {0}", position);
            GameObject photoFrameInstance = Instantiate(photoFrame, (Vector3)position, Quaternion.identity);
            RawImage img = photoFrameInstance.GetComponentInChildren<RawImage>();
            img.texture = texture2D;
            photoFrameInstance.transform.localScale = Vector3.Scale(photoFrameInstance.transform.localScale, new Vector3(xscale, yscale, 1));
            //Debug.LogFormat("2 代码执行耗时：{0} 毫秒", (DateTime.Now - startTime).TotalMilliseconds);
        }
    }
}

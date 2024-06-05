using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Common
{
    private static Common instance = null;
    public static Common Instance()
    {
        // single instance
        if (instance == null)
        {
            instance = new Common();
        }
        return instance;
    }

    public List<Photo> photos = new List<Photo>();

    public void loadImages(FileSystemInfo info)
    {
        if (!info.Exists) return;

        DirectoryInfo dir = info as DirectoryInfo;
        //����Ŀ¼
        if (dir == null) return;

        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;
            //���ļ�
            if (file != null)
            {
                var fs = file.OpenRead();
                byte[] rawData = new byte[fs.Length];//�����ֽڣ������洢��ȡ����ͼƬ�ֽ�
                try
                {
                    fs.Read(rawData, 0, rawData.Length);//��ʼ��ȡ�����������trycatch��䣬��ֹ��ȡʧ�ܱ���
                    Texture2D texture2D = new Texture2D(0, 0);
                    texture2D.LoadImage(rawData);
                    Debug.LogFormat("load image {0}, width {1}, height {2}", file.FullName, texture2D.width, texture2D.height);

                    // convert texture to sprite
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                    photos.Add(new Photo(sprite, texture2D.width, texture2D.height));
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                fs.Close();//�мǹر�
            }

            //������Ŀ¼�����еݹ����
            else
            {
                loadImages(files[i]);
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public struct Photo
{
    public Sprite sprite;
    public float width;
    public float height;
    // constructor
    public Photo(Sprite sprite, float width, float height)
    {
        this.sprite = sprite;
        this.width = width;
        this.height = height;
    }
}
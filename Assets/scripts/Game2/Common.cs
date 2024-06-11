using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Common
{
    private static Common instance = null;

    public BlockingCollection<PhotoRawData> queue = new BlockingCollection<PhotoRawData>();

    public bool isLoadImage = false;
    public static Common Instance()
    {
        // single instance
        if (instance == null)
        {
            instance = new Common();
        }
        return instance;
    }

    public PhotoRawData? TryGetPhoto()
    {
        if (queue.TryTake(out PhotoRawData p))
        {
            return p;
        }
        else
        {
            return null;
        }
    }


    public void loadImages(FileSystemInfo info)
    {
        isLoadImage = true;
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
                    queue.Add(new PhotoRawData(rawData, fs.Name));
                                       
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    fs.Close();//�мǹر�
                }
            }
            else
            {
                //������Ŀ¼�����еݹ����
                loadImages(files[i]);
            }
        }
        isLoadImage = false;
        return;
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

public struct PhotoRawData
{
    public byte[] rawData;
    public string name;
    public PhotoRawData(byte[] rawData, string name)
    {
        this.rawData = rawData;
        this.name = name;
    }

}
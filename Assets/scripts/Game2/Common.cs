using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Common
{
    private static readonly Common instance = new Common();

    public BlockingCollection<PhotoRawData> queue = new BlockingCollection<PhotoRawData>();

    public bool isLoadImage = false;
    public static Common Instance
    {
        get { return instance; }
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
        //不是目录
        if (dir == null) return;

        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;
            //是文件
            if (file != null)
            {
                var fs = file.OpenRead();
                byte[] rawData = new byte[fs.Length];//生命字节，用来存储读取到的图片字节
                try
                {
                    fs.Read(rawData, 0, rawData.Length);//开始读取，这里最好用trycatch语句，防止读取失败报错
                    Debug.LogFormat("add photo {0} to queue", fs.Name);
                    queue.Add(new PhotoRawData(rawData, fs.Name));
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    fs.Close();//切记关闭
                }
            }
            else
            {
                //对于子目录，进行递归调用
                loadImages(files[i]);
            }
        }
        isLoadImage = false;
        return;
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
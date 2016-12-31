using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("ItemCollection")]
public class ItemContainer
{
    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<BaseItem> items = new List<BaseItem>();

    public static ItemContainer Load()
    {
        string dbPath = "";
        string realPath;

        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "items.xml");

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            realPath = Application.persistentDataPath + "/db";
            System.IO.File.WriteAllBytes(realPath, reader.bytes);

            dbPath = realPath;
        }
        else
        {
            // iOS
            dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "items.xml");
        }

        var serializer = new XmlSerializer(typeof(ItemContainer));
        using (var stream = new FileStream(dbPath, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ItemContainer;
        }
    }
}

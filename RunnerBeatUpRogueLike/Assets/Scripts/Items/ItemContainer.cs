using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("ItemCollection")]
public class ItemContainer
{
    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<BaseItem> items = new List<BaseItem>();

    public static ItemContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ItemContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ItemContainer;
        }
    }
}

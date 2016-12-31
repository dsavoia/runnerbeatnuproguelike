using System.Xml;
using System.Xml.Serialization;

public class BaseItem
{
    [XmlAttribute("ID")]
    public int itemID;
    public string name;
    public int spriteIndex;
    public int price;
    public string type;
}

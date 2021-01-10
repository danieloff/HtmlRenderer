﻿//MIT, 2014-present, WinterDev
using LayoutFarm.WebDom;
using LayoutFarm.UI;
namespace LayoutFarm.Demo
{
    public delegate void Decorate(DomElement h);
    public static class HtmlDomExtension
    {
        //level 1
        public static DomElement AddChild(this DomElement elem, string elementName)
        {
            var newchild = elem.OwnerDocument.CreateElement(elementName);
            elem.AddChild(newchild);
            return newchild;
        }
        public static DomElement AddChild(this DomElement elem, string elementName, out DomElement elemExit)
        {
            var newchild = elem.OwnerDocument.CreateElement(elementName);
            elem.AddChild(newchild);
            elemExit = newchild;
            return newchild;
        }
        public static DomElement AddChild(this DomElement elem, string elementName, Decorate d)
        {
            var newchild = elem.OwnerDocument.CreateElement(elementName);
            elem.AddChild(newchild);
            if (d != null)
            {
                d(newchild);
            }

            return newchild;
        }


        public static void AddTextContent(this DomElement elem, string text)
        {
            var newTextNode = elem.OwnerDocument.CreateTextNode(text.ToCharArray());
            elem.AddChild(newTextNode);
        }
        //------------------------------------------------------------------------------

        //level 2
        public static void AttachMouseDownEvent(this DomElement elem, HtmlEventHandler hdl)
        {
            elem.AttachEvent(UIEventName.MouseDown, hdl);
        }
        public static void AttachMouseUpEvent(this DomElement elem, HtmlEventHandler hdl)
        {
            elem.AttachEvent(UIEventName.MouseUp, hdl);
        }
    }
}
﻿//Apache2, 2014-present, WinterDev

using LayoutFarm.CustomWidgets;
using LayoutFarm.WebDom;
namespace LayoutFarm
{
    [DemoNote("4.6 dynamic dom1")]
    class Demo_DynamicDom1 : App
    {
        HtmlBox htmlMenuBox;
        HtmlBox testHtmlBox;
        HtmlBoxes.HtmlHost htmlhost;
        AppHost _host;
        bool testToggle;
        protected override void OnStart(AppHost host)
        {
            _host = host;
            this.htmlhost = HtmlHostCreatorHelper.CreateHtmlHost(host, null, null);
            SetupHtmlMenuBox();
            //==================================================
            //box1 test area
            //html box
            this.testHtmlBox = new HtmlBox(htmlhost, 800, 400);
            testHtmlBox.SetLocation(30, 50);
            host.AddChild(testHtmlBox);
            string html = @"<html><head></head><body><div id='div1'>OK1</div><div>OK2</div></body></html>";
            testHtmlBox.LoadHtmlString(html);
            //==================================================  
        }
        void SetupHtmlMenuBox()
        {
            //==================================================
            this.htmlMenuBox = new HtmlBox(htmlhost, 800, 40);
            htmlMenuBox.SetLocation(30, 0);
            _host.AddChild(htmlMenuBox);
            string html = @"<html><head></head><body>
                    <div id='menubox'>
                        <span id='test_dom1'>click to toggle!</span>
                        <span id='test_dom2'>test document fragment</span>
                        <span id='test_dom3'>test showdow dom</span>
                    </div>
            </body></html>";
            htmlMenuBox.LoadHtmlString(html);
            //set event handlers
            var htmldoc = htmlMenuBox.HtmlDoc;
            var div_menuBox = htmldoc.getElementById("menubox") as IHtmlElement;
            if (div_menuBox == null) return;
            //test set innerHTML
            div_menuBox.attachEventListener("mousedown", (e) =>
            {
                var contextE = e.SourceHitElement as IHtmlElement;
                switch (contextE.id)
                {
                    case "test_dom1":
                        {
                            //test toggle with innerHTML
                            IHtmlDocument testHtmlDoc = testHtmlBox.HtmlDoc;
                            var div1 = testHtmlDoc.getElementById("div1") as IHtmlElement;
                            if (div1 != null)
                            {
                                //test set innerHTML
                                div1.innerHTML = testToggle ?
                                        "<div>11111</div>" :
                                        "<div>22222</div>";
                                testToggle = !testToggle;
                            }
                        }
                        break;
                    case "test_dom2":
                        {
                            //test toggle with DocumentFragment
                            IHtmlDocument testHtmlDoc = testHtmlBox.HtmlDoc;

                            var div1 = testHtmlDoc.getElementById("div1") as IHtmlElement;
                            if (div1 != null)
                            {
                                var docFragment = testHtmlDoc.createDocumentFragment();
                                if (testToggle)
                                {
                                    var node1 = docFragment.createElement("div") as IHtmlElement;
                                    node1.appendChild(
                                        docFragment.createTextNode("3333"));//TODO: review this
                                    docFragment.rootNode.appendChild(node1);
                                }
                                else
                                {
                                    var node1 = docFragment.createElement("div") as IHtmlElement;
                                    node1.appendChild(
                                        docFragment.createTextNode("4444"));//TODO: review this
                                    docFragment.rootNode.appendChild(node1);
                                }
                                div1.clear();
                                div1.appendChild(docFragment.rootNode);
                                testToggle = !testToggle;
                            }
                        }
                        break;
                    case "test_dom3":
                        {
                            //shadow dom!
                            IHtmlDocument testHtmlDoc = testHtmlBox.HtmlDoc;
                            var div1 = testHtmlDoc.getElementById("div1") as IHtmlElement;
                            if (div1 != null)
                            {
                                var shadowRoot = testHtmlDoc.createShadowRootElement() as IHtmlElement;
                                if (testToggle)
                                {
                                    var node1 = testHtmlDoc.createElement("div") as IHtmlElement;
                                    node1.appendChild(
                                        testHtmlDoc.createTextNode("XXX"));//TODO: review this 
                                    //add node1 to shadow root element
                                    shadowRoot.appendChild(node1);
                                }
                                else
                                {
                                    var node1 = testHtmlDoc.createElement("div") as IHtmlElement;
                                    node1.appendChild(
                                        testHtmlDoc.createTextNode("YYY"));//TODO: review this 
                                    //add node1 to shadow root element
                                    shadowRoot.appendChild(node1);
                                }

                                div1.clear();
                                div1.appendChild(shadowRoot);
                                testToggle = !testToggle;
                            }
                        }
                        break;
                }
            });
        }
    }
}
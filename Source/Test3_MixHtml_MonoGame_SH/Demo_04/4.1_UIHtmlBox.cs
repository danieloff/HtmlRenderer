//Apache2, 2014-present, WinterDev 
using GeonBit.UI;
using GeonBit.UI.Entities;
using LayoutFarm.CustomWidgets;
using LayoutFarm.UI;
using PixelFarm.Drawing.MonoGame;
using System;

namespace LayoutFarm
{
    [DemoNote("4.1 UIHtmlBox")]
    public class Demo_UIHtmlBox : App
    {

        HtmlBox _htmlBox;
        string _htmltext;
        string _documentRootPath;
        private IGameHTMLUI _pcx;
        AppHost _host;
        private GraphicsViewRoot _viewroot;
        private LayoutFarm.UI.MyWinFormsControl _latest_formCanvas;

        protected override void OnStart(AppHost host, IGameHTMLUI pcx)
        {
            _pcx = pcx;
            //html box
            _host = host;
            var loadingQueueMx = new LayoutFarm.ContentManagers.ImageLoadingQueueManager();
            loadingQueueMx.AskForImage += loadingQueue_AskForImg;

            HtmlBoxes.HtmlHost htmlHost = HtmlHostCreatorHelper.CreateHtmlHost(host,
                (s, e) => loadingQueueMx.AddRequestImage(e.ImageBinder),
                contentMx_LoadStyleSheet);

            //
            _htmlBox = new HtmlBox(htmlHost, 1000, 1000);
            //DEO _htmlBox.SetLocation(0, 10); //test
            host.AddChild(_htmlBox);
            if (_htmltext == null)
            {
                _htmltext = @"<html><head></head><body>NOT FOUND!</body></html>";
            }
            _htmlBox.LoadHtmlString(_htmltext);
        }


        void loadingQueue_AskForImg(object sender, LayoutFarm.ContentManagers.ImageRequestEventArgs e)
        {
            //load resource -- sync or async? 
            //if we enable cache in loadingQueue (default=> enable)
            //if the loading queue dose not have the req img 
            //then it will raise event to here

            //we can resolve the req image to specific img
            //eg. 
            //1. built -in img from control may has special protocol
            //2. check if the req want a local file
            //3. or if req want to download from the network
            //

            //examples ...

            string absolutePath = null;
            if (e.ImagSource.StartsWith("built_in://imgs/"))
            {
                //substring
                absolutePath = _documentRootPath + "/" + e.ImagSource.Substring("built_in://imgs/".Length);
            }
            else
            {
                absolutePath = _documentRootPath + "/" + e.ImagSource;
            }

            if (!_pcx.Game.FileExistsAppData(absolutePath)) //!System.IO.File.Exists(absolutePath)) //DEO file paths
            {
                return;
            }
            //load
            //lets host do img loading... 

            //we can do img resolve or caching here

            e.SetResultImage(_host.LoadImage(absolutePath));
        }
        void contentMx_LoadStyleSheet(object sender, LayoutFarm.ContentManagers.TextRequestEventArgs e)
        {
            string absolutePath = _documentRootPath + "\\" + e.Src;
            if (!System.IO.File.Exists(absolutePath))
            {
                return;
            }
            //if found
            e.TextContent = System.IO.File.ReadAllText(absolutePath);
        }
        public void LoadHtml(string documentRootPath, string htmltext)
        {
            _documentRootPath = System.IO.Path.GetDirectoryName(documentRootPath);
            _htmltext = htmltext;
        }

        public LayoutFarm.UI.MyWinFormsControl GetPanel(IGameHTMLUI pcx)
        {
            _pcx = pcx;

            //1. create blank form
            YourImplementation.DemoFormCreatorHelper.CreateReadyForm(
                InnerViewportKind.MonoGame,
                out _viewroot,
                out _latest_formCanvas,
                _pcx.UI,
                _pcx);

            //_userInterface.UseRenderTarget = false;
            //_userInterface.AddEntity(_latest_formCanvas);

            AppHost appHost = new AppHost(_pcx);
            AppHostConfig config = new AppHostConfig();
            YourImplementation.UISurfaceViewportSetupHelper.SetUISurfaceViewportControl(config, _viewroot);
            appHost.Setup(config);


            /*_latest_formCanvas.FormClosed += (s, e) =>
            {
                //when owner form is disposed
                //we should clear our resource?
                app.OnClosing();
                app.OnClosed();
                _latest_formCanvas = null;
                _viewroot = null;
            };*/


            //2. create app host 
            appHost.StartApp(this);
            //_viewroot.TopDownRecalculateContent();



            //==================================================  
            if (false) //true) //this.chkShowLayoutInspector.Checked)
            {
                ///YourImplementation.LayoutInspectorUtils.ShowFormLayoutInspector(_viewroot);
            }


            return _latest_formCanvas;
        }

    }
}
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using GeneralTool.General.WPFHelper;
using Microsoft.Win32;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainWindowLib.ViewModels
{
    public class ImageViewModel : BaseNotifyModel
    {
        private string testText = "TestText";
        public string TestText { get => this.testText; set => this.RegisterProperty(ref this.testText, value); }

        private string mousePoint;
        public string MousePoint { get => this.mousePoint; set => this.RegisterProperty(ref this.mousePoint, value); }

        private string drawRect;
        public string DrawRect { get => this.drawRect; set => this.RegisterProperty(ref this.drawRect, value); }

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get => this.imageSource; set
            {
                imageSource = null;
                this.RegisterProperty(ref this.imageSource, value);
            }
        }

        public ILog Log { get; set; }
        public ICommand LoadImageCommand
        {
            get => new SimpleCommand(() =>
              {
                  var open = new OpenFileDialog();
                  if (open.ShowDialog().Value)
                  {
                      var fileName = open.FileName;
                      try
                      {
                          this.ImageSource = new BitmapImage(new Uri(fileName, UriKind.Absolute));
                      }
                      catch (Exception ex)
                      {
                          Log.Error($"图片加载出错:{ex.Message}");
                      }
                      return;
                  }

                  Log.Info("没有选择图片文件");
              });
        }


        public void CutImageOkMethod(object sender, ImageEventArgs e)
        {
            this.Log.Info($"截图是否成功:{e.Sucess} - {e.ErroMsg}");
            if (e.Sucess)
            {
                var path = "1.jpeg";
                e.Source.SaveBitmapSouce(path, GeneralTool.General.Enums.BitmapEncoderEnum.Jpeg);
                this.Log.Info($"图片已保存至:{path}");
            }
        }


        public void CutRectMethod(object sender, ImageCutRectEventArgs e)
        {
            this.Log.Info($"当前选择区域:{e.CutPixelRect}");

            //是否阻止向下的调用,如果不阻止,则会继续触发上面的 CutImageOk 事件
            //e.HandleToNext = false;
        }

        public void ImageMouseMove(ImageMouseEventArgs e)
        {
            this.MousePoint = $"{(int)e.CurrentPixelPoint.X} - {(int)e.CurrentPixelPoint.Y}";
        }

        public void CutPanelRectMethod(object sender, ImageCutRectEventArgs e)
        {
            this.DrawRect = e.CutPixelRect + "";
        }

    }
}

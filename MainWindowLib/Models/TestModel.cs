using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeneralTool.CoreLibrary.WPFHelper;

namespace MainWindowLib.Models
{
    public class TestModel:BaseNotifyModel
    {
        private string add;
        public String Add
        {
            get => add;
            set => this.RegisterProperty(ref this.add, value);
        }

        private string startTime;
        public string StartTime
        {
            get => this.startTime;
            set => this.RegisterProperty(ref this.startTime, value);
        }

        private string taskType;
        public string TaskType
        {
            get => this.taskType;
            set => this.RegisterProperty(ref this.taskType, value);
        }

        private string localPath;
        public string LocalPath
        {
            get => this.localPath; 
            set => this.RegisterProperty(ref this.localPath, value); 
        }

        private string remotePath;
        public string RemotePath
        {
            get => this.remotePath; set => this.RegisterProperty(ref this.remotePath, value);
        }

        private int count;
        public int Count
        {
            get => this.count; set => this.RegisterProperty(ref this.count, value);
        }

        private int delCount;
        public int DelCount
        {
            get => this.delCount; set => this.RegisterProperty(ref this.delCount, value);
        }

        private int dirCount;
        public int DirCount
        {
            get => this.dirCount; set => this.RegisterProperty(ref this.dirCount, value);
        }

        private int size;
        public int Size
        {
            get => this.size; set => this.RegisterProperty(ref this.size, value);
        }

        private string sumTime;
        public string SumTime
        {
            get => this.sumTime; set => this.RegisterProperty(ref this.sumTime, value);
        }

        private double progress;
        public double Progress
        {
            get => this.progress; set => this.RegisterProperty(ref this.progress, value);
        }

        private Visibility visibility;

       

        public Visibility Visibility { get => this.visibility; set => this.RegisterProperty(ref this.visibility, value); }
    }
}

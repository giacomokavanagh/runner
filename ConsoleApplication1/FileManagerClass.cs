using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    class FileManagerClass
    {
        public EventHandlersClass hndCopyFileToLocation = new EventHandlersClass();
        private string SourceFile_, DestinationFile_;
        private Dictionary<string, string> Properties_;

        public Dictionary<string, string> Properties
        {
            get
            {
                Properties_ = new Dictionary<string, string>();
                Properties_.Add("SourceFile_", SourceFile_.ToString());
                Properties_.Add("DestinationFile_", DestinationFile_.ToString());
                return Properties_;
            }
        }

        public void copyFileToLocation(string SourceFolder, string DestinationFolder, string FileName)
        {
            SourceFile_ = System.IO.Path.Combine(SourceFolder, FileName);
            DestinationFile_ = System.IO.Path.Combine(DestinationFolder, FileName);

            if (!System.IO.Directory.Exists(DestinationFolder))
            {
                System.IO.Directory.CreateDirectory(DestinationFolder);
            }
            System.IO.File.Copy(SourceFile_, DestinationFile_, true);

            hndCopyFileToLocation.fire(this);
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using FileWatcherService.Responders;
using System.Security.Permissions;

namespace FileWatcherService
{
    public partial class FileWatcherService : ServiceBase
    {        
        FileSystemWatcher _watcher = null;
        public FileWatcherService()
        {
            InitializeComponent();
            EventLog.EnableRaisingEvents = true;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void OnStart(string[] args)
        {
            String directory = ConfigurationManager.AppSettings["WatchedDirectory"];
            String extension = ConfigurationManager.AppSettings["WatchedExtension"];
            String responders = ConfigurationManager.AppSettings["Responders"];
            //Debugger.Launch();
            if (extension == null) extension = "*.*";
            if (Directory.Exists(directory))
            {
                _watcher = new FileSystemWatcher();
                _watcher.Path = directory;
                _watcher.Filter = extension;
                _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                string[] responderArray = responders.Split(new char[] { ',' });
                foreach (string responderName in responderArray)
                {
                    Type t = Type.GetType(responderName, false, true);
                    if (t == null)
                    {
                        EventLog.WriteEntry(String.Format("Could not locate responder '{0}'", responderName), EventLogEntryType.Error, 4);
                    }
                    else
                    {
                        ConstructorInfo constructor = t.GetConstructor(new Type[] { typeof(ServiceBase) });
                        if (constructor == null)
                        {
                            EventLog.WriteEntry(String.Format("Could not locate appropriate constructor for '{0}'. Is it public?", responderName), EventLogEntryType.Error, 5);
                        }
                        else
                        {
                            EventLog.WriteEntry(String.Format("Will respond with '{0}'", responderName), EventLogEntryType.Information, 6);
                            IResponder responder = (IResponder)constructor.Invoke(new object[] { this });
                            //IResponder responder = new EventLogResponder(this);
                            _watcher.Changed += new FileSystemEventHandler(responder.OnChanged);
                            _watcher.Created += new FileSystemEventHandler(responder.OnCreated);
                            _watcher.Deleted += new FileSystemEventHandler(responder.OnDeleted);
                            _watcher.Renamed += new RenamedEventHandler(responder.OnRenamed);

                        }
                    }
                }
                _watcher.EnableRaisingEvents = true;
                EventLog.WriteEntry(String.Format("Now watching '{0}' for '{1}' files", directory, extension), EventLogEntryType.Information, 2);
            }
            else
            {
                EventLog.WriteEntry(String.Format("Directory for watching '{0}' does not exist", directory), EventLogEntryType.Error, 3);
            }
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry("stopping service");
            if (_watcher != null) _watcher.EnableRaisingEvents = false;
            _watcher = null;
        }
    }
}

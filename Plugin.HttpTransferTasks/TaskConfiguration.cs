﻿using System;
using System.Collections.Generic;


namespace Plugin.HttpTransferTasks
{
    public class TaskConfiguration
    {
        /// <summary>
        /// </summary>
        /// <param name="uri">The URI for where to push/pull the file</param>
        /// <param name="localFilePath">
        /// For uploads, localFilePath MUST be set
        /// For downloads, localFilePath can be empty and will be given a temporary filename.  The result set will also contain the remote filename from the server.  You can move the file upon completion
        /// </param>
        public TaskConfiguration(string uri, string localFilePath = null)
        {
            this.Uri = uri;
            this.LocalFilePath = localFilePath;
        }


        public string Uri { get; }
        public string LocalFilePath { get; set; }
        public bool UseMeteredConnection { get; set; }
        public bool AutoStart { get; set; } = true;
        public string HttpMethod { get; set; } = "GET";
        public string PostData { get; set; }
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}

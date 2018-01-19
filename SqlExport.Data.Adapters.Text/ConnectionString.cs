using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Data.Adapters.Text
{
    internal struct ConnectionString
    {
        private const string DefaultBasePath = "./";

        private string basePath;

        public string BasePath
        {
            get { return this.basePath ?? DefaultBasePath; }
            private set { this.basePath = value; }
        }

        public bool HasHeaders { get; private set; }

        internal static ConnectionString Parse(string connectionString)
        {
            var parts = connectionString.Split(';');
            if (parts.Length > 1)
            {
                var cs = new ConnectionString();
                foreach (var item in parts)
                {
                    var keyValue = item.Split('=');
                    if (keyValue.Length == 2)
                    {
                        switch (keyValue[0])
                        {
                            case "Base Path":
                                cs.BasePath = keyValue[1];
                                break;
                            case "Has Headers":
                                cs.HasHeaders = bool.Parse(keyValue[1]);
                                break;
                        }
                    }
                }

                return cs;
            }

            return new ConnectionString { BasePath = connectionString, HasHeaders = true };
        }
    }
}

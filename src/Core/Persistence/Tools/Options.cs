using Persistence.Tools.Enums;

namespace Persistence.Tools
{
    public class Options : object
    {
        public Options(string connectionString , Provider provider) : base()
        {
            ConnectionString = connectionString;
            Provider =  provider;
        }

        public Provider Provider { get; set; }

        public string ConnectionString { get; set; }
    }
}
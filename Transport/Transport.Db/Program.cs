using CommandLine;
using log4net;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Transport.Db
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       var logger = LogManager.GetLogger(typeof(Program));
                       var cnx = new SqlConnection($"Server=localhost;Database={o.Database};Trusted_Connection=yes;");
                       var evolve = new Evolve.Evolve(cnx, msg => logger.Info(msg))
                       {
                           Locations = new List<string> { "scripts" },
                           IsEraseDisabled = true
                       };

                       evolve.Migrate();
                   });


        }
    }

    public class Options
    {
        [Option('d', "database", Required = true, HelpText = "Database to migrate")]
        public string Database { get; set; }
    }
}

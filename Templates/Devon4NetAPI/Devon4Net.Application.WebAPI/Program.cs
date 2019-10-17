using Devon4Net.Application.WebAPI.Configuration.Application;

namespace Devon4Net.Application.WebAPI
{
    /// <summary>
    /// devonfw template
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main devonfw program
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Devonfw.Configure<Startup>(args);
        }
    }
}
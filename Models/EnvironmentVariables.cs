namespace CodeGeneration.BasePlatform.Models
{
    public class EnvironmentVariables
    {
        private static EnvironmentVariables _value;

        public string BasePath { get; set; }

        public string AppName { get; set; }

        public string Lanuguage { get; set; }

        public static EnvironmentVariables Get()
        {
            return _value;
        }

        internal static void Set(EnvironmentVariables environmentVariables)
        {
            _value = environmentVariables;
        }
    }
}

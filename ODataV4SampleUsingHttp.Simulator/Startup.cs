using System;
using OpenSilver.Simulator;

namespace ODataV4SampleUsingHttp.Simulator
{
    internal static class Startup
    {
        [STAThread]
        static int Main(string[] args)
        {
            return SimulatorLauncher.Start(typeof(App));
        }
    }
}

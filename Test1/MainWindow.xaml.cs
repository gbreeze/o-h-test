using Rssdp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IEnumerable<DiscoveredSsdpDevice> Devices { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            tbx.Text = "searching...";
            SearchForDevices();
        }

        //Call this method from somewhere to begin the search.
        public async void SearchForDevices()
        {
            // This code goes in a method somewhere.
            using (var deviceLocator = new SsdpDeviceLocator("192.168.0.2"))
            {
                // Can pass search arguments here (device type, uuid). No arguments means all devices.
                // var foundDevices = await deviceLocator.SearchAsync("urn:av-openhome-org:service:Playlist:1");
                var foundDevices = await deviceLocator.SearchAsync("urn:schemas-upnp-org:service:SwitchPower:1");
                
                this.Devices = foundDevices;

                tbx.Text = "";

                foreach (var foundDevice in foundDevices)
                {
                    // Device data returned only contains basic device details and location ]
                    // of full device description.
                    Console.WriteLine("Found " + foundDevice.Usn + " at " + foundDevice.DescriptionLocation.ToString());

                    tbx.Text += Environment.NewLine + "Found " + foundDevice.Usn + " at " + foundDevice.DescriptionLocation.ToString();

                    // Can retrieve the full device description easily though.
                    var fullDevice = await foundDevice.GetDeviceInfo();
                    Console.WriteLine(fullDevice.FriendlyName);
                    tbx.Text += Environment.NewLine + fullDevice.FriendlyName;
                    Console.WriteLine();
                    tbx.Text += Environment.NewLine;
                }

                if (!foundDevices.Any())
                    tbx.Text = "nothing found :|";
            }
        }

        private void Button_Click_Play(object sender, RoutedEventArgs e)
        {
            if (this.Devices.Any())
            {
                var mainDevice = this.Devices.First();                
            }
        }
    }
}
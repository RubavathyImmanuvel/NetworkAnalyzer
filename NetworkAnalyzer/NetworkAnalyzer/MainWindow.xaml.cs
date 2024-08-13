using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows;

namespace NetworkAnalyzer
{
    public class TcpConnectionModel
    {
        public string LocalAddress { get; set; }
        public string RemoteAddress { get; set; }
        public string State { get; set; }
    }

    public class NetworkInterfaceModel
    {
        public string Name { get; set; }
        public long BytesSent { get; set; }
        public long BytesReceived { get; set; }
        public long PacketsSent { get; set; }
        public long PacketsReceived { get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadNetworkData();
        }

        private void LoadNetworkData()
        {
            LoadTcpConnections();
            LoadNetworkStatistics();
        }

        private void LoadTcpConnections()
        {
            List<TcpConnectionModel> connections = new List<TcpConnectionModel>();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnections = properties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation info in tcpConnections)
            {
                connections.Add(new TcpConnectionModel
                {
                    LocalAddress = info.LocalEndPoint.ToString(),
                    RemoteAddress = info.RemoteEndPoint.ToString(),
                    State = info.State.ToString()
                });
            }

            ConnectionsDataGrid.ItemsSource = connections;
        }

        private void LoadNetworkStatistics()
        {
            NetworkStatsListBox.Items.Clear();
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceStatistics stats = netInterface.GetIPStatistics();
                NetworkStatsListBox.Items.Add($"Interface: {netInterface.Name}");
                NetworkStatsListBox.Items.Add($"Bytes Sent: {stats.BytesSent}");
                NetworkStatsListBox.Items.Add($"Bytes Received: {stats.BytesReceived}");
                NetworkStatsListBox.Items.Add($"Packets Sent: {stats.UnicastPacketsSent}");
                NetworkStatsListBox.Items.Add($"Packets Received: {stats.UnicastPacketsReceived}");
                NetworkStatsListBox.Items.Add(new string('-', 20));
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNetworkData();
        }
    }
}

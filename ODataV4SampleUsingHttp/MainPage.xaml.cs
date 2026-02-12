using Default;
using Microsoft.OData.Client;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Interop;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ODataV4SampleUsingHttp
{
    public class ODataResponse<T>
    {
        public List<T> Value { get; set; }

        [JsonPropertyName("@odata.nextLink")]
        public string NextLink { get; set; }
    }
    public partial class MainPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ObservableCollection<Product> _products = new();

        private string _nextLink;
        private bool _isLoading;

        private const string BaseUrl = "https://localhost:44391/odata/";

        public MainPage()
        {
            InitializeComponent();

            MyDataGrid.ItemsSource = _products;
            UpdateUI();
        }

        private async void LoadProducts_Click(object sender, RoutedEventArgs e)
        {
            await LoadFirstPage();
        }

        private async Task LoadFirstPage()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;
                UpdateUI();

                _products.Clear();
                _nextLink = null;

                string url = $"{BaseUrl}Products/GetExpensiveProducts(minPrice=20000)";

                await LoadFromUrl(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                UpdateUI();
            }
        }

        private async Task LoadNextPage()
        {
            if (_isLoading || string.IsNullOrEmpty(_nextLink))
                return;

            try
            {
                _isLoading = true;
                UpdateUI();

                await LoadFromUrl(_nextLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading more products: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                UpdateUI();
            }
        }

        private async Task LoadFromUrl(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ODataResponse<Product>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            foreach (var item in result.Value)
                _products.Add(item);

            _nextLink = result.NextLink;
        }

        private async void MyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (_isLoading || string.IsNullOrEmpty(_nextLink))
                return;

            if (e.Row.GetIndex() >= _products.Count - 3)
            {
                await LoadNextPage();
            }
        }

        private void UpdateUI()
        {
            LoadProductsButton.IsEnabled = !_isLoading;

            StatusText.Text = _isLoading ? "Loading..." : "";
            LoadingIndicator.Visibility =
                _isLoading ? Visibility.Visible : Visibility.Collapsed;

            CountText.Text = $"Total products: {_products.Count}";

            if (!string.IsNullOrEmpty(_nextLink) && !_isLoading)
                CountText.Text += " (scroll down for more)";
        }
    }    
}
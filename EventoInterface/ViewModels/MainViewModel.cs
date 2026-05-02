using EventoInterface.DTOs_Interface;
using EventoInterface.ViewModels;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Input;

public class MainViewModel : BaseViewModel
{
    public ObservableCollection<EventoDto> Eventos { get; set; } = new();

    public ICommand CarregarEventosCommand { get; }

    public string Status { get; set; } = "Carregando...";

    public MainViewModel()
    {
        CarregarEventosCommand = new RelayCommand(_ => CarregarEventos());

        // CARREGA AUTOMÁTICO AO ABRIR
        CarregarEventos();
    }

    private async void CarregarEventos()
    {
        try
        {
            Status = "Carregando...";
            OnPropertyChanged(nameof(Status));

            var http = new HttpClient();

            var dados = await http.GetFromJsonAsync<List<EventoDto>>(
                "https://localhost:7294/api/v1/evento");

            Eventos.Clear();

            if (dados != null)
            {
                foreach (var item in dados)
                    Eventos.Add(item);
            }

            Status = $"Total: {Eventos.Count} eventos";
        }
        catch (Exception ex)
        {
            Status = "Erro ao conectar com API";

            MessageBox.Show(
                $"Erro ao carregar eventos:\n{ex.Message}",
                "Erro",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        OnPropertyChanged(nameof(Status));
    }
}
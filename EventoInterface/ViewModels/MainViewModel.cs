using EventoInterface.Models;
using EventoInterface.Commands;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;

namespace EventoInterface.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<EventoDto> Eventos { get; set; }

        public ICommand CarregarEventosCommand { get; }

        public MainViewModel()
        {
            Eventos = new ObservableCollection<EventoDto>();

            CarregarEventosCommand = new RelayCommand(async _ => await CarregarEventos());

            // 🔥 CARREGA AUTOMATICAMENTE AO ABRIR
            _ = CarregarEventos();
        }

        private async Task CarregarEventos()
        {
            try
            {
                var http = new HttpClient();

                var response = await http.GetFromJsonAsync<
                    ApiResponseDto<List<EventoDto>>>(
                    "https://localhost:7294/api/v1/evento");

                Eventos.Clear();

                if (response?.Dados != null)
                {
                    foreach (var evento in response.Dados)
                        Eventos.Add(evento);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
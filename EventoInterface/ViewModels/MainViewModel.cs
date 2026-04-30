using EventoInterface.Commands;
using Shared;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;

namespace EventoInterface.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Eventos> Eventos { get; set; }

        public ICommand CarregarEventosCommand { get; }

        public MainViewModel()
        {
            Eventos = new ObservableCollection<Eventos>();

            CarregarEventosCommand =
                new RelayCommand(async _ => await CarregarEventos());
        }

        private async Task CarregarEventos()
        {
            var http = new HttpClient();

            var dados = await http.GetFromJsonAsync<List<Eventos>>(
                "https://localhost:5149/api/v1/evento");

            Eventos.Clear();

            if (dados != null)
            {
                foreach (var e in dados)
                    Eventos.Add(e);
            }
        }
    }
}
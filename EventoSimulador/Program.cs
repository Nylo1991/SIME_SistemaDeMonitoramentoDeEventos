using EventoSimulador.DTOs_Simulador;
using System.Net.Http.Json;

var http = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7294")
};

var random = new Random();

string[] tipos = { "Alerta", "Falha", "Info" };
string[] locais = { "Motor", "Painel", "Sala 1", "Sala 2" };

Console.WriteLine("Simulador iniciado...\n");

while (true)
{
    try
    {
        var evento = new EventoRequestDto
        {
            Tipo_Evento = tipos[random.Next(tipos.Length)],
            local_Evento = locais[random.Next(locais.Length)]
        };

        var response = await http.PostAsJsonAsync("/api/v1/evento", evento);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"{evento.Tipo_Evento} - {evento.local_Evento}");
        }
        else
        {
            var erro = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{response.StatusCode}");
            Console.WriteLine(erro);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{ex.Message}");
    }

    await Task.Delay(2000);
}
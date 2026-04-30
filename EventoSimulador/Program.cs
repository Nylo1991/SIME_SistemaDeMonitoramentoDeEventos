using Shared;
using System.Net.Http.Json;

var http = new HttpClient();
var random = new Random();

string[] tipos = { "Alerta", "Falha", "Info" };
string[] locais = { "Sala 1", "Sala 2", "Painel", "Motor" };

while (true)
{
    var evento = new Eventos
    {
        Tipo_Evento = tipos[random.Next(tipos.Length)],
        local_Evento = locais[random.Next(locais.Length)],
        DataHora = DateTime.Now
    };

    var response = await http.PostAsJsonAsync(
        "https://localhost:5149/api/v1/evento",
        evento);

    Console.WriteLine(response.IsSuccessStatusCode
        ? "Evento enviado"
        : "Erro ao enviar");

    await Task.Delay(2000);
}
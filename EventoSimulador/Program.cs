using System.Net.Http.Json;

var https = new HttpClient();
var random = new Random();

string[] tipos = { "Alerta", "Falha", "Info" };
string[] locais = { "Sala 1", "Sala 2", "Painel", "Motor" };

while (true)
{
    var evento = new
    {
        Tipo = tipos[random.Next(tipos.Length)],
        Mensagem = $"Evento em {locais[random.Next(locais.Length)]}"
    };

    try
    {
        var response = await https.PostAsJsonAsync(
            "https://localhost:5149/api/v1/evento",
            evento);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"OK → {evento.Tipo} | {evento.Mensagem}");
        }
        else
        {
            var erro = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erro: {erro}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Falha: {ex.Message}");
    }

    await Task.Delay(2000);
}
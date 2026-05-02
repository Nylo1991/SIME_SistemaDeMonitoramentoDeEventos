using EventoSimulador.DTOs_Simulador;
using System.Net.Http.Json;

var https = new HttpClient();

// Ignore SSL (apenas dev)
https.BaseAddress = new Uri("https://localhost:7294");
https.DefaultRequestHeaders.Accept.Clear();

var random = new Random();

string[] tipos = { "Alerta", "Falha", "Info" };
string[] mensagens =
{
    "Temperatura alta",
    "Motor desligado",
    "Sistema normal",
    "Pressão baixa",
    "Falha elétrica"
};

Console.WriteLine("Simulador iniciado...\n");

while (true)
{
    try
    {
        var evento = new EventoRequestDto
        {
            Tipo = tipos[random.Next(tipos.Length)],
            //  SEM QUEBRA DE LINHA
            Mensagem = mensagens[random.Next(mensagens.Length)]
        };

        var response = await https.PostAsJsonAsync(
            "/api/v1/evento",
            evento);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($" Enviado: {evento.Tipo} - {evento.Mensagem}");
        }
        else
        {
            var erro = await response.Content.ReadAsStringAsync();
            Console.WriteLine($" Erro: {response.StatusCode}");
            Console.WriteLine(erro);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Exceção: {ex.Message}");
    }

    await Task.Delay(2000);
}
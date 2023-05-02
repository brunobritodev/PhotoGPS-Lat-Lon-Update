using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PhotoGPS_Update;

// O Google timeline salva o horário da Geolocalização em UTC
var localTimeOffset = new TimeSpan(-3, 0, 0);

// Muitas cameras digitais já estão configuradas para o horário local
var photoTimeOffset = new TimeSpan(0, -30, 0);

// Diretorio que contem as fotos
var diretorioFotos = new DirectoryInfo(@"F:\Google Photos");

// arquivo json do Google Timeline (Geralmente é um arquivo bem grande)
var googleTimelineJsonFile = "Records.json";


var fotosSemInformacaoDeGps = new List<PhotoInfo>();
var searchPatterns = new[] { ".jpg", ".jpeg", ".png" };
//var totalArquivos = diretorioFotos.EnumerateFiles("*.*", SearchOption.AllDirectories).Count();
var totalArquivos = 0;
var arquivosProcessados = 0;

var arquivos = new List<FileInfo>();
Console.WriteLine($"Buscando fotos sem informações de GPS ...");
arquivos.AddRange(diretorioFotos.GetFiles());
foreach (var dir in diretorioFotos.GetDirectories())
{
    arquivos.AddRange(dir.GetFiles());
}

foreach (var arquivo in diretorioFotos.EnumerateFiles("*.*", SearchOption.AllDirectories))
{
    // É uma imagem?
    if (searchPatterns.Contains(arquivo.Extension))
    {
        if (!FileGpsData.ImageHasGpsInfo(arquivo, out var horarioFoto))
        {
            if (horarioFoto.HasValue)
                fotosSemInformacaoDeGps.Add(new PhotoInfo(arquivo, horarioFoto.Value.Add(photoTimeOffset)));
        }
    }
    arquivosProcessados++;
    if (arquivosProcessados % 100 == 0)
        BarraProgresso.ExibirBarraDeProgresso(arquivosProcessados, arquivos.Count);
}
BarraProgresso.ExibirBarraDeProgresso(1, 1);
Console.WriteLine();

fotosSemInformacaoDeGps = fotosSemInformacaoDeGps.OrderBy(b => b.DataFoto).ToList();
var primeiraData = fotosSemInformacaoDeGps.FirstOrDefault()?.DataFoto;

Console.WriteLine("Lendo arquivo do Google Timeline para buscar GeoCoordenadas");
var firstLocation = true;
var itens = 0;

using var sr = File.OpenText(googleTimelineJsonFile);
using var reader = new JsonTextReader(sr);
while (reader.Read())
{
    if (reader.TokenType == JsonToken.StartObject)
    {
        if (firstLocation)
        {
            firstLocation = false;
            continue;
        }

        var obj = JObject.Load(reader);
        var locationInfo = obj.ToObject<LocationInfo>();

        if (locationInfo.ObterHorario(localTimeOffset) >= primeiraData)
        {
            // Só vai atualizar as fotos que esteja num intervalo de até duas horas
            foreach (var photoInfo in fotosSemInformacaoDeGps.Where(w =>
                        w.DataFoto < locationInfo.ObterHorario(localTimeOffset) && 
                        w.DataFoto.AddHours(2) > locationInfo.ObterHorario(localTimeOffset) && 
                        !w.PossuiGeoposicao))
            {
                photoInfo.AtualizarLocalizacao(locationInfo);
            }
        }
        BarraProgresso.ExibirQuantidade(++itens, "Google geoposicoes processadas");
    }
}


Console.WriteLine($"Atualizando Informacao de GPS nas fotos ...");
arquivosProcessados = 0;
totalArquivos = fotosSemInformacaoDeGps.Count();
foreach (var foto in fotosSemInformacaoDeGps)
{
    foto.AtualizarGeoposicao();
    arquivosProcessados++;
    BarraProgresso.ExibirBarraDeProgresso(arquivosProcessados, totalArquivos);
}
BarraProgresso.ExibirBarraDeProgresso(1, 1);
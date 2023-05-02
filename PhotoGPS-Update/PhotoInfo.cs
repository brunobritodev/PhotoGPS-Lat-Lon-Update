// See https://aka.ms/new-console-template for more information
using PhotoGPS_Update;
using System.Diagnostics;

[DebuggerDisplay("{Latitude},{Longitude}")]
public class PhotoInfo
{
    public PhotoInfo(FileInfo arquivo, DateTime horarioFoto)
    {
        Arquivo = arquivo;
        DataFoto = horarioFoto.AddMinutes(-30);
        PossuiGeoposicao = false;
    }

    public FileInfo Arquivo { get; }
    public DateTime DataFoto { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool PossuiGeoposicao { get; set; }
    public void AtualizarLocalizacao(LocationInfo locationInfo)
    {
        Latitude = locationInfo.latitudeE7 / 1e7;
        Longitude = locationInfo.longitudeE7 / 1e7;
        PossuiGeoposicao = true;
    }

    public void AtualizarGeoposicao()
    {
        FileGpsData.SetImageLocation(Arquivo, Latitude, Longitude);
        
    }

    private (int degrees, int minutes, double seconds) ConvertToDMS(double decimalDegrees)
    {
        int d = (int)decimalDegrees;
        double m = (decimalDegrees - d) * 60;
        double s = (m - (int)m) * 60;
        return (d, (int)m, s);
    }
}
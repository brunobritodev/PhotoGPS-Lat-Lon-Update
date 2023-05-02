using ExifLibrary;

namespace PhotoGPS_Update
{
    internal class FileGpsData
    {
        public static bool ImageHasGpsInfo(FileInfo imagePath, out DateTime? horarioFoto)
        {
            horarioFoto = null;
            try
            {
                var file = ImageFile.FromFile(imagePath.FullName);
                var latTag = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLatitude);
                var lonTag = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLongitude);
                var fotoHorario = file.Properties.Get<ExifLibrary.ExifDateTime>(ExifTag.DateTimeOriginal);
                if (fotoHorario is not null)
                    horarioFoto = fotoHorario.Value;
                if (latTag is null || lonTag is null)
                    return false;

                return latTag.Degrees.Numerator > 0 && latTag.Degrees.Denominator > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao ler os dados Exif: " + ex.Message);
            }
            return false;
        }

        public static void SetImageLocation(FileInfo imagePath, double latitude, double longitude)
        {
            if (latitude == 0 || longitude == 0)
                return;

            try
            {
                var file = ImageFile.FromFile(imagePath.FullName);

                var latitudeDMS = ConvertToDMS(Math.Abs(latitude));
                var longitudeDMS = ConvertToDMS(Math.Abs(longitude));
                // Atualiza as informações de geolocalização no objeto ExifWriter
                file.Properties.Set(ExifTag.GPSLatitude, latitudeDMS.degrees, latitudeDMS.minutes, latitudeDMS.seconds);
                file.Properties.Set(ExifTag.GPSLatitudeRef, latitude < 0 ? "S" : "N");
                file.Properties.Set(ExifTag.GPSLongitude, longitudeDMS.degrees, longitudeDMS.minutes, longitudeDMS.seconds);
                file.Properties.Set(ExifTag.GPSLongitudeRef, longitude < 0 ? "W" : "E");

                // Salva as informações atualizadas no arquivo de imagem
                file.Save(imagePath.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao escrever os dados Exif: " + ex.Message);
            }
        }

        private static (int degrees, int minutes, float seconds) ConvertToDMS(double decimalDegrees)
        {
            int d = (int)decimalDegrees;
            double m = (decimalDegrees - d) * 60;
            double s = (m - (int)m) * 60;
            return (d, (int)m, (float)s);
        }
    }
}

# Photo Geolocation Updater

This project aims to update the geolocation information of photos using Google Timeline data. It searches for photos without GPS data and then updates their geolocation information based on the location data obtained from the Google Timeline.

## Usage
Change `Program.cs`:

1. Set the `localTimeOffset` variable to your local time offset from UTC.
2. Set the `photoTimeOffset` variable to the time offset of your digital camera.
3. Set the `diretorioFotos` variable to the directory containing your photos.
4. Set the `googleTimelineJsonFile` variable to the path of your Google Timeline JSON file.
5. `dotnet run`

Please make sure to adjust the file paths, offsets, and other variables according to your needs.

## Example

```csharp
var localTimeOffset = new TimeSpan(-3, 0, 0);
var photoTimeOffset = new TimeSpan(0, -30, 0);
var diretorioFotos = new DirectoryInfo(@"F:\Google Photos");
var googleTimelineJsonFile = "Records.json";
```
## Dependencies

This project depends on the following libraries:

* Newtonsoft.Json
* ExifLibrary

## Classes

### PhotoInfo
Represents the information of a photo, including the file, date, latitude, longitude, and whether it has geolocation information.

### LocationInfo
Represents the location information of a Google Timeline entry, including latitude, longitude, accuracy, activity, source, device tag, and timestamp.

### Activity
Represents an activity related to the location information, including the activity type and confidence.

### FileGpsData
Handles the reading and writing of GPS data from image files using the ExifLibrary.

### BarraProgresso (ProgressBar)
Displays the progress of the geolocation update process.

### License
This project is licensed under the MIT License.

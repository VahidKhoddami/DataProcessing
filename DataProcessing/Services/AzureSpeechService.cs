using FFMpegCore;
using KeyManagement.Interfaces;

namespace DataProcessing.Services
{
    public class AzureSpeechService : IConversionService
    {
        private readonly ISecretKeyService _keyService;
        private bool _convertedToWav = false;
        public AzureSpeechService(ISecretKeyService keyService) => _keyService = keyService;

        public async Task<string> AnalyseAsync(string path)
        {

            Microsoft.CognitiveServices.Speech.SpeechConfig config = Microsoft.CognitiveServices.Speech.SpeechConfig.FromSubscription(await _keyService.GetSecret("SpeechServicesKey"), "canadacentral");

            //get the audio file and transcribe it
            var convertedPath = await ConvertToWav(path);
            var result = "";

            using (var audioInput = Microsoft.CognitiveServices.Speech.Audio.AudioConfig.FromWavFileInput(convertedPath))
            {
                using var recognizer = new Microsoft.CognitiveServices.Speech.SpeechRecognizer(config, audioInput);

                var stopRecognition = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

                recognizer.Recognized += (s, e) =>
                {
                    result += e.Result.Text;
                };

                recognizer.Canceled += (s, e) =>
                {
                    stopRecognition.TrySetResult(0);
                };

                recognizer.SessionStopped += (s, e) =>
                {
                    stopRecognition.TrySetResult(0);
                };

                // For long-running multi-utterance recognition, we use StartContinuousRecognitionAsync()
                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
                Task.WaitAny(new[] { stopRecognition.Task });
                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            }

            //Release if file is in use and then Remove if the file was converted to wav 
            if (_convertedToWav)
                File.Delete(convertedPath);

            return result;
        }

        private async Task<string> ConvertToWav(string path)
        {
            if (Path.GetExtension(path) == ".wav")
                return path;

            //get file name
            var fileName = Path.GetFileNameWithoutExtension(path);
            // get the directory
            var directoryName = Path.GetDirectoryName(path);
            var output = $"{directoryName}\\{fileName}.wav";

            //convert the file to wav
            await FFMpegArguments.FromFileInput(path)
                .OutputToFile(output)
                .ProcessAsynchronously();

            _convertedToWav = true;

            return output;
        }

    }
}

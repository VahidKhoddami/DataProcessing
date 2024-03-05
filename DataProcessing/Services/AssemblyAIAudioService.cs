
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;


namespace DataProcessing.Services
{
    internal class Transcript
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }

        [JsonPropertyName("language_code")]
        public string LanguageCode { get; set; }

        public string Error { get; set; }
    }

    public class AssemblyAIAudioService : IConversionService
    {
        private readonly ISecretKeyService _secretKeyService;
        public AssemblyAIAudioService(ISecretKeyService secretKeyService) => _secretKeyService = secretKeyService;

        public async Task<string> AnalyseAsync(string path)
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(await _secretKeyService.GetSecret("AssemblyAI"));

            var uploadUrl = await UploadFileAsync(path, httpClient);
            var transcript = await CreateTranscriptAsync(uploadUrl, httpClient);
            transcript = await WaitForTranscriptToProcess(transcript, httpClient);

            return transcript.Text;
        }

        private async Task<string> UploadFileAsync(string filePath, HttpClient httpClient)
        {
            using (var fileStream = File.OpenRead(filePath))
            using (var fileContent = new StreamContent(fileStream))
            {
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                using (var response = await httpClient.PostAsync("https://api.assemblyai.com/v2/upload", fileContent))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonDoc = await response.Content.ReadFromJsonAsync<JsonDocument>();
                    return jsonDoc.RootElement.GetProperty("upload_url").GetString();
                }
            }
        }

        private async Task<Transcript> CreateTranscriptAsync(string audioUrl, HttpClient httpClient)
        {
            var data = new { audio_url = audioUrl };
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync("https://api.assemblyai.com/v2/transcript", content))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Transcript>();
            }
        }

        private async Task<Transcript> WaitForTranscriptToProcess(Transcript transcript, HttpClient httpClient)
        {
            var pollingEndpoint = $"https://api.assemblyai.com/v2/transcript/{transcript.Id}";

            while (true)
            {
                var pollingResponse = await httpClient.GetAsync(pollingEndpoint);
                transcript = await pollingResponse.Content.ReadFromJsonAsync<Transcript>();
                switch (transcript.Status)
                {
                    case "processing":
                    case "queued":
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        break;
                    case "completed":
                        return transcript;
                    case "error":
                        throw new Exception($"Transcription failed: {transcript.Error}");
                    default:
                        throw new Exception("This code shouldn't be reachable.");
                }
            }
        }
    }
}
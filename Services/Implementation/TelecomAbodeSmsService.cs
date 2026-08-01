using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using TicketFlow_v2.Models;
using TicketFlow_v2.Services.Interface;

namespace TicketFlow_v2.Services.Implementation;

public class TelecomAbodeSmsService(
    HttpClient httpClient,
    IOptions<TelecomAbode> telecomSmsSettings,
    ILogger<TelecomAbodeSmsService> logger) : ISmsService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly TelecomAbode _telecomSmsSettings = telecomSmsSettings.Value;
    private readonly ILogger<TelecomAbodeSmsService> _logger = logger;

    public async Task SendSmsAsync(string recipientPhoneNumber, string message)
    {
        if (string.IsNullOrWhiteSpace(recipientPhoneNumber))
        {
            _logger.LogInformation("SMS sending cancelled: recipient phone number is empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_telecomSmsSettings.BaseUrl) ||
            string.IsNullOrWhiteSpace(_telecomSmsSettings.ApiKey))
        {
            _logger.LogInformation("TelecomAbode SMS settings are incomplete. Skipping SMS.");
            return;
        }

        var recipients = recipientPhoneNumber
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(NormalizePhoneNumber)
            .Where(number => !string.IsNullOrWhiteSpace(number))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        if (recipients.Count == 0)
        {
            _logger.LogInformation("SMS sending cancelled: no valid recipient phone numbers.");
            return;
        }

        AddFallbackRecipientWhenRequired(recipients);

        var bulkTo = string.Join(",", recipients);
        var payload = new Dictionary<string, string>
        {
            { "subject", _telecomSmsSettings.Subject },
            { "bulkPhones", bulkTo },
            { "message", message }
        };

        try
        {
            using var content = new FormUrlEncodedContent(payload);
            using var request = new HttpRequestMessage(HttpMethod.Post, _telecomSmsSettings.BaseUrl)
            {
                Content = content
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _telecomSmsSettings.ApiKey);
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SMS sent successfully to {Recipients}.", bulkTo);
            }
            else
            {
                _logger.LogInformation(
                    "Failed to send SMS to {Recipients}. Status {Status}. Response: {Response}",
                    bulkTo,
                    response.StatusCode,
                    responseContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending SMS to {Recipients}.", bulkTo);
        }
    }

    private static string NormalizePhoneNumber(string number)
    {
        var cleanPhone = number.Trim();
        if (cleanPhone.StartsWith("+234", StringComparison.Ordinal))
        {
            return "0" + cleanPhone[4..];
        }

        if (cleanPhone.StartsWith("234", StringComparison.Ordinal))
        {
            return "0" + cleanPhone[3..];
        }

        if (cleanPhone.StartsWith("+", StringComparison.Ordinal))
        {
            return cleanPhone[1..];
        }

        return cleanPhone;
    }

    private void AddFallbackRecipientWhenRequired(List<string> recipients)
    {
        if (recipients.Count != 1 || string.IsNullOrWhiteSpace(_telecomSmsSettings.FallbackRecipient))
        {
            return;
        }

        var fallbackRecipient = NormalizePhoneNumber(_telecomSmsSettings.FallbackRecipient);
        if (!string.IsNullOrWhiteSpace(fallbackRecipient) &&
            !recipients.Contains(fallbackRecipient, StringComparer.Ordinal))
        {
            recipients.Add(fallbackRecipient);
        }
    }
}

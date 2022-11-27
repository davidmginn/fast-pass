﻿using FastPass.Models;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System.Net.Http.Json;
using Task = System.Threading.Tasks.Task;

namespace FastPass.UI.Services;

public class ApiService
{
    private const string TEXT_ANALYTICS_ENDPOINT = "api/TextAnalyticsServiceProxy";
    private const string FHIR_SUBMISSION_ENDPOINT = "api/AddPatient";
    private readonly HttpClient _client;
    private FhirJsonParser _parser = new FhirJsonParser();
    private FhirJsonSerializer _serializer = new FhirJsonSerializer();

    public ApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Bundle> AnalyzeTextAsync(TextAnalyticsProxyRequest request)
    {
        var result = await _client.PostAsJsonAsync(TEXT_ANALYTICS_ENDPOINT, request);

        var json = await result.Content.ReadAsStringAsync();

        return _parser.Parse<Bundle>(json);
    }

    public async Task SubmitPatientAsync(Patient patient)
    {
        var json = await patient.ToJsonAsync();

        var result = await _client.PostAsync(FHIR_SUBMISSION_ENDPOINT, new StringContent(json));

        Console.WriteLine(result);
    }
}
SemaphoreSlim _gate = new SemaphoreSlim(500);

HttpClient _client = new HttpClient()
{
    Timeout = TimeSpan.FromSeconds(5)
};   

Task.WaitAll(CreateCalls().ToArray());

IEnumerable<Task> CreateCalls()
{
    for (int i = 0; i < 10000; i++)
    {
        yield return CallGoogle();
    }
}

async Task CallGoogle()
{
    try
    {
        await _gate.WaitAsync();
        var response = await _client.GetAsync("https://google.com");
        Console.WriteLine(response.StatusCode);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        _gate.Release();
    }
}
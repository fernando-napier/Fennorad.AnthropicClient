# Fennorad.AnthropicClient

This is a library that allows for easy integration into Claude

### Integrate

In your `Startup.cs` or `Program.cs` (wherever you build your services) you just need to add the following registration:
```
services.RegisterAnthropicClient(apiKey, anthropicVersion);
```
Now you can inject `IAnthropicClient _anthropicClient;` into any of your services 
<br/>
### GetMessagesAsync(AnthropicRequest, CancellationToken)
[Link to Message Endpoint](https://docs.anthropic.com/claude/reference/messages_post)  
This endpoint returns an AnthropicResponse with the entire message body
```
var anthropicResponse = await _anthropicClient.SendMessagesAsync(AnthropicRequest);
```

### GetMessageStreamAsync(AnthropicRequest, CancellationToken)
[Link to Message Streaming](https://docs.anthropic.com/claude/reference/messages-streaming)  
This endpoint returns an IAsyncEnumerable of the Server Sent Event records returned by Anthropic.
```
await (var event in _anthropicClient.SendMessageStreamAsync(AnthropicRequest))
{
  // process incoming events
}
```

### Future work
Add functionality for [using Tools](https://docs.anthropic.com/claude/docs/tool-use) currently in beta by Anthropic

# Fennorad.AnthropicClient

The IAnthropicClient has two integration points.

1. GetMessagesAsync(AnthropicRequest, CancellationToken)
This endpoint returns an AnthropicResponse with the entire message body
Integration: var anthropicResponse = _anthropicClient.SendMessagesAsync(AnthropicRequest);

2. GetMessageStreamAsync(AnthropicRequest, CancellationToken)
This endpoint returns an IAsyncEnumerable of the Server Sent Event records returned by Anthropic.

Integration: await (var event in _anthropicClient.SendMessageStreamAsync(AnthropicRequest))
{
  // process incoming events
}

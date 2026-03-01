using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MyBlazorApp.Services
{
    public class UserSessionService
    {
        private readonly IJSRuntime _js;

        public UserSessionService(IJSRuntime js)
        {
            _js = js;
        }

        public string? UserName { get; private set; }
        public string? Email { get; private set; }
        public Guid SessionId { get; private set; } = Guid.NewGuid();

        public async Task SetUserAsync(string name, string email)
        {
            UserName = name;
            Email = email;
            SessionId = Guid.NewGuid();
            await _js.InvokeVoidAsync("eventEaseSession.set", "user", new { name, email, sessionId = SessionId.ToString() });
        }

        public async Task LoadAsync()
        {
            try
            {
                var data = await _js.InvokeAsync<JsonElement?>("eventEaseSession.get", "user");
                if (data.HasValue)
                {
                    var elem = data.Value;
                    if (elem.TryGetProperty("name", out var n)) UserName = n.GetString();
                    if (elem.TryGetProperty("email", out var e)) Email = e.GetString();
                    if (elem.TryGetProperty("sessionId", out var s) && Guid.TryParse(s.GetString(), out var g)) SessionId = g;
                }
            }
            catch
            {
                // ignore JS errors
            }
        }

        public async Task ClearAsync()
        {
            UserName = null;
            Email = null;
            SessionId = Guid.NewGuid();
            await _js.InvokeVoidAsync("eventEaseSession.remove", "user");
        }
    }
}

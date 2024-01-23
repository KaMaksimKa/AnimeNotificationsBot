

static string Config(string what)
{
    switch (what)
    {
        case "api_id": return "24815852";
        case "api_hash": return "3dfb9fd36f1455173a83b533d01f5210";
        case "phone_number": return "+212689079781";
        case "verification_code": Console.Write("Code: "); return Console.ReadLine();
        case "first_name": return null;      // if sign-up is required
        case "last_name": return null;        // if sign-up is required
        case "password": return "ylikeMNjq7S8+";     // if user has enabled 2FA
        default: return null;                  // let WTelegramClient decide the default config
    }
}

using var client = new WTelegram.Client(Config);
var myself = await client.LoginUserIfNeeded();
Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");
var chats = await client.Messages_GetAllChats();

var inputFile = await client.UploadFileAsync("video.mp4");

await client.SendMediaAsync(chats.chats[2089161809], "Here is the video", inputFile);


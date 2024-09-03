using DalDocBackend.Data;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly AppDbContext database;

    public ChatHub(AppDbContext db)
    {
        database = db;
    }
    public async Task JoinSpecificGroup(string department)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, department);
    }

    public async Task SendMessage(string department, string message)
    {
        var comment = new Comment
        {
            Department = department,
            Message = message,
            Timestamp = DateTime.UtcNow
        };
        database.Comments.Add(comment);
        await database.SaveChangesAsync();

        await Clients.Group(department).SendAsync("ReceiveMessage", comment);
    }
}

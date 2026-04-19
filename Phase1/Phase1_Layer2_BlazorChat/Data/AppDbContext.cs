using Microsoft.EntityFrameworkCore;
using Phase1_Layer2_BlazorChat.Models;

namespace Phase1_Layer2_BlazorChat.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
}
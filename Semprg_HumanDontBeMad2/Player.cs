namespace Semprg_HumanDontBeMad2;

public record Player(int SessionId, string Name)
{
    public List<Figure> HeldFigures { get; set; } = new();
}
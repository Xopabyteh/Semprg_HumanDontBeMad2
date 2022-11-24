namespace Semprg_HumanDontBeMad2;

public record Tile
{
    public List<Figure> Figures { get; set; } = new List<Figure>();
    public bool IsEnd { get; set; }
    public bool IsStart { get; set; }
}
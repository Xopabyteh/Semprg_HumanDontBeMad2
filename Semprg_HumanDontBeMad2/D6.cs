namespace Semprg_HumanDontBeMad2;

public record D6 :IDice
{
    public int Roll()
    {
        return Random.Shared.Next(1,7);
    }
}
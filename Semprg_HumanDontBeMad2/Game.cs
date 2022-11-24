using System.Diagnostics;
using System.Text;

namespace Semprg_HumanDontBeMad2;

public record Game
{
    private List<Player> activePlayers = new();
    //private List<Player> finishedPlayers = new();
    private Rules rules;
    private Board board;
    private IDice dice;
    public Game(Rules rules, Board board, IDice dice, params string[] playerNames)
    {
        this.rules = rules;
        this.board = board;
        startTile = board.TileSet.First(x => x.IsStart);
        if (startTile is null)
            throw new ArgumentNullException($"{nameof(startTile)}", "The given board contains no start til");
        this.dice = dice;
        for (int i = 0; i < playerNames.Length; i++)
        {
            activePlayers.Add(new Player(i, playerNames[i]));
        }
    }
    public void Awake()
    {
        Start();
        while (true)
        {
            Update();
            if (activePlayers.Count == 0)
            {
                break;
            }
        }

        Console.WriteLine("gg");
    }

    //private readonly List<Figure> figures = new();
    private Tile startTile;
    private void Start()
    {
        //Fill figures
        foreach (var player in activePlayers)
        {
            for (int i = 0; i < rules.FigureAmount; i++)
            {
                var newFigure = new Figure(i,player);
                //figures.Add(newFigure);
                player.HeldFigures.Add(newFigure);

                //Put them on start
                startTile.Figures.Add(newFigure);
            }
        }

        DrawAll();
    }

    private void Update()
    {
        foreach (var player in activePlayers)
        {
            //Move using a random figure in the players hand
            var figureToMoveIndex = Random.Shared.Next(player.HeldFigures.Count);
            var figureToMove = player.HeldFigures.ElementAt(figureToMoveIndex);

            var rng = dice.Roll();
            var figuresCurrentTileNode = board.TileSet.Find(board.TileSet.First(x => x.Figures.Contains(figureToMove)));
            Debug.Assert(figuresCurrentTileNode != null, nameof(figuresCurrentTileNode) + " != null");
                
            var moveToTileNode = figuresCurrentTileNode;
            for (int i = 0; i < rng; i++)
            {
                if (moveToTileNode.Next is not null)
                {
                    moveToTileNode = moveToTileNode.Next;
                    continue;
                }

                break;
            }

            //Check if the tile we move our figure to is the end
            if (moveToTileNode.Value.IsEnd)
            {
                //Remove from players held figures to prevent from further using it
                player.HeldFigures.Remove(figureToMove);
            }
            else
            {
                //Check for other figures on the tile
                if (moveToTileNode.Value.Figures.Count != 0)
                {
                    bool nonSameOwnerFiguresOnTile = false;
                    foreach (var occupant in moveToTileNode.Value.Figures)
                    {
                        if (occupant.Owner != figureToMove.Owner)
                        {
                            nonSameOwnerFiguresOnTile = true;
                            break;
                        }
                    }
                    //If any of occupants is not the owners figure, move them all to start
                    if (nonSameOwnerFiguresOnTile)
                    {
                        startTile.Figures.AddRange(moveToTileNode.Value.Figures);
                        moveToTileNode.Value.Figures.Clear();
                    }
                }
            }

            //Move our figure
            figuresCurrentTileNode.Value.Figures.Remove(figureToMove);
            moveToTileNode.Value.Figures.Add(figureToMove);

            //Draw the fields state
            DrawAll();
        }
        //Mark players that have no more active figures as inactive
        //To array to immediately run the query
        var playersToMark = activePlayers.Where(x => x.HeldFigures.Count == 0).ToArray();
        foreach (var playerToMark in playersToMark)
        {
            activePlayers.Remove(playerToMark);
            //finishedPlayers.Add(playerToMark);
        }
    }

    private void DrawAll()
    {
        var sb = new StringBuilder();
        foreach (var tile in board.TileSet)
        {
            if (tile.IsStart)
            {
                sb.Append("S");
            } else if (tile.IsEnd)
            {
                sb.Append("E");
            }
            sb.Append("[");
            
            foreach (var figureOnTile in tile.Figures)
            {
                sb.Append(figureOnTile.Owner.Name)
                    .Append(figureOnTile.Id)
                    .Append("; ");
            }

            sb.Append("]");
        }

        Console.WriteLine(sb);
    }
}
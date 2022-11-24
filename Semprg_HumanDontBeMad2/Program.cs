using Semprg_HumanDontBeMad2;

var rules = new Rules(2);

var linearTileSet = new List<Tile>()
{
    new Tile() {IsStart = true},

    new Tile(),
    new Tile(),
    new Tile(),
    new Tile(),
    new Tile(),

    new Tile(),
    new Tile(),
    new Tile(),

    new Tile() {IsEnd = true}
};
var board = new Board(new LinkedList<Tile>(linearTileSet));
var dice = new D6();

var g = new Game(
    rules,
    board,
    dice,
    "Martin", "Adam", "Robert"
);

g.Awake();

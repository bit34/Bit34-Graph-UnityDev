using Com.Bit34Games.Graphs;

public class MyHexGraph : HexGraph<MyHexGraphConfig, MyHexNode, MyHexEdge>
{
    public MyHexGraph(int columnCount, int rowCount, MyHexGraphConfig config) :
     base(config, new MyHexGraphAllocator(), columnCount, rowCount){}
}
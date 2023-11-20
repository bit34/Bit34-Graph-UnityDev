using Com.Bit34Games.Graphs;

public class MyRectGraph : RectGraph<MyRectGraphConfig, MyRectNode, MyRectEdge>
{
    public MyRectGraph(int columnCount, int rowCount, MyRectGraphConfig config) :
     base(config, new MyRectGraphAllocator(), columnCount, rowCount){}
}
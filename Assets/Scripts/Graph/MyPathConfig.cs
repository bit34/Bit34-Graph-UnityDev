using Com.Bit34Games.Graphs;

public class MyPathConfig : PathConfig<MyGraphNode, MyGraphConnection>
{
    public MyPathConfig() : base(false, true, null){}
}
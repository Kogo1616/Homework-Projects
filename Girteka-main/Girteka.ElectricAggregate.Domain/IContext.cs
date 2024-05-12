namespace Girteka.ElectricAggregate.Domain;

public interface IContext<TInput, TPath, TResult>
{
    public TResult Do(TInput param, TPath path);
}
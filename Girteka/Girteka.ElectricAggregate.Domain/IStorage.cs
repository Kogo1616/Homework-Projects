namespace Girteka.ElectricAggregate.Domain;

public interface IStorage<TInput, TResult>
{
    public void Do(TInput param, TInput name);
}
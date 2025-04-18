using Classes;
using UnityEngine;
using Utils;

public class Test_Singleton_1 : Singleton<Test_Singleton_1>
{
    protected override void Init()
    {
        // print("Im exist");
        QuickBenchmark
            .Bench(() =>
            {
                Invoke(nameof(DoNothing), 0f);
                return true;
            })
            .Print("Gettype");
    }

    public static Test_Singleton_1 Method()
    {
        return GetInstance();
    }

    void DoNothing() { }
}

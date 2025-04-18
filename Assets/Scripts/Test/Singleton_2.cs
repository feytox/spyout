using Classes;
using UnityEngine;
using Utils;

public class Test_Singleton_2 : Singleton<Test_Singleton_2>
{
    protected override void Init()
    {
        // print("Im exist");
        // print(Method());
    }

    public static Test_Singleton_1 Method()
    {
        return Test_Singleton_1.Method();
    }
}

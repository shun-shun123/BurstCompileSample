using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class BusrstMonoBehaviour : MonoBehaviour
{
    void Start()
    {
        var input = new NativeArray<float>(10, Allocator.Persistent);
        var output = new NativeArray<float>(1, Allocator.Persistent);
        for (var i = 0; i < input.Length; i++)
        {
            input[i] = 1.0f * i;
        }

        var job = new MyJob
        {
            Input = input,
            Output = output
        };
        
        job.Schedule().Complete();

        Debug.Log("The result of the sum is: " + output[0]);
        input.Dispose();
        output.Dispose();
    }

    [BurstCompile(CompileSynchronously = true)]
    private struct MyJob : IJob
    {
        [ReadOnly]
        public NativeArray<float> Input;

        [WriteOnly]
        public NativeArray<float> Output;
        
        public void Execute()
        {
            float result = 0.0f;
            for (var i = 0; i < Input.Length; i++)
            {
                result += Input[i];
            }

            Output[0] = result;
        }
    }
}

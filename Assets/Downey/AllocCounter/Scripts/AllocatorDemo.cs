using System;
using UnityEngine;
using UnityEngine.Profiling;

public class AllocatorDemo : MonoBehaviour
{
    const int BYTES_TO_MB =  1024 * 1024;

    void Start()
    {
        var ac = new AllocCounter();
        long memUsed = System.GC.GetTotalMemory(false);
        long monoBytes = Profiler.GetMonoUsedSizeLong();
        byte[] junk = new byte[BYTES_TO_MB];

        var allocCount = ac.Stop();
        
        long memUsedAfter = GC.GetTotalMemory(false);
        long monoBytesAfter = Profiler.GetMonoUsedSizeLong();
        
        Debug.Log("Allocations during Start: "+allocCount);
        Debug.Log("Memory used before: "+memUsed+", after: "+ memUsedAfter+", diff: "+(memUsedAfter - memUsed));
        Debug.Log("Mono used before: "+monoBytes+", after: "+ monoBytesAfter+", diff: "+(monoBytesAfter - monoBytes));
    }

    void Update()
    {
        // if (Time.frameCount % 60 == 0)
        // {
        //     byte[] junk = new byte[BYTES_TO_MB];
        //     
        // }
    }
}

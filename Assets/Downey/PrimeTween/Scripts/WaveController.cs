using Cysharp.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(EntitySpawner))]
public class WaveController : MonoBehaviour
{
    EntitySpawner entitySpawner;

    async void Start()
    {
        entitySpawner = GetComponent<EntitySpawner>();
        
        Debug.Log("Starting wave....");

        for (int i = 0; i < 3; i++)
        {
            await entitySpawner.SpawnEntityAsync(new Vector3(i * 2f, 0f, 0f));
            await UniTask.Delay(500);
        }
        
        Debug.Log("Wave completed.");
    }
}

using UnityEngine;
using System.Collections;

namespace Slonersoft.SloneUtil {
  public class DestroyObjAfterTime : MonoBehaviour {
    public float lifetime = 1.0f;

    // TODO: reconcile this with the one that sends blips (DestroyAfterTime in the game itself).
    private IEnumerator DestroyAfterTime_coroutine()
    {
      yield return new WaitForSeconds (lifetime);
      GameObject.Destroy (gameObject);
    }

    // Use this for initialization
    void Start () {
      StartCoroutine ( DestroyAfterTime_coroutine());
    }
  }
}
